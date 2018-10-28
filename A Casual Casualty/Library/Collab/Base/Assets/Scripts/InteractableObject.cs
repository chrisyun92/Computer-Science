using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    public bool nextToPlayer;
    public bool canInteract;
    string parentTag;

    private Queue<string> tasks;
    public Animator animator;
    public Rigidbody2D rb;
    private bool holding = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        parentTag = transform.tag;
        tasks = new Queue<string>();
        for (int i = 1; i < 3; i++)
        {
            tasks.Enqueue("Task" + i);
        }
    }

    // Update is called once per frame
    void Update () {
        PlayerObjectInteraction("_P1");
        PlayerObjectInteraction("_P2");
    }

    // When Colliding with an object, check if colliding object is player.
    // If its a player, change nextToPlayer to true.
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag == "Player")
        {
            nextToPlayer = true;
        }
    }

    // Change nextToPlayer to false when leaving collision.
    void OnCollisionExit2D(Collision2D coll)
    {
        nextToPlayer = false;
    }


    // Handle all player's interaction with objects.
    void PlayerObjectInteraction(string playerSuffix) {
        if (nextToPlayer) {
            bool interact = Input.GetButton("Interact" + playerSuffix);
            if (interact && canInteract) {
                canInteract = false;
                Debug.Log("INTERACTION by: " + playerSuffix);

                if (parentTag == "Phone")
                {
                    string task = tasks.Dequeue();
                    FindObjectOfType<GameManagerTutorial>().phoneAnswered = true;
                    GameObject cpt = FindObjectOfType<CaptainDialogueStart>().gameObject;
                    cpt.GetComponent<CaptainDialogueStart>().StartTask(task);
                    Debug.Log(FindObjectOfType<CaptainDialogueStart>().gameObject.name);
                    FindObjectOfType<CaptainDialogueStart>().StartTask(task);

                    // Maybe we should stop animating the phone when it isn't ringing,
                    // as in, after player interacts with it, set it to idle.
                    bool isRinging = animator.GetBool("isRinging");
                    Debug.Log("I just interacted with the phone. isRinging is: " + isRinging);
                    animator.SetBool("isRinging", false);
                    Debug.Log("isRinging is now: " + isRinging);
                }

                if (parentTag == "TorpedoLoaderTubeLever")
                {
                    // switch activation state
                    animator.SetBool("IsActivated", !animator.GetBool("IsActivated"));
                    GameObject tubeSlider = GameObject.FindWithTag("PressurizedTubeSlider");
                    tubeSlider.GetComponent<PressurizedTubeScript>().Activate();

                    // FIXME: make a coroutine so you have to wait for the animation to complete first
                    //canInteract = true;
                }

                if (parentTag == "Table")
                {
                    canInteract = true;
                    float moveHor = Input.GetAxis("Horizontal" + playerSuffix);
                    float moveVer = Input.GetAxis("Vertical" + playerSuffix);
                    float speed = GameObject.FindWithTag("Player").GetComponent<PlayerScript>().speed;
                    Vector2 movement = new Vector2(speed * moveHor * Time.deltaTime, speed * moveVer * Time.deltaTime);
                    rb.velocity = movement;
                }
            }
        }
    }
}
