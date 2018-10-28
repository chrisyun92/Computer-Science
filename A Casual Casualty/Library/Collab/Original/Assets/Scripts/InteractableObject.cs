using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    public bool nextToPlayer;
    public bool canInteract;
    string parentTag;
    private string playerNextToMe;

    public Animator animator;
    public Animator targetAnimator;
    public Rigidbody2D rb;
    private bool holding = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        parentTag = transform.tag;
    }

    // Update is called once per frame
    void Update () {
        PlayerObjectInteraction("P1");
        PlayerObjectInteraction("P2");
    }

    // When Colliding with an object, check if colliding object is player.
    // If its a player, change nextToPlayer to true.
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag == "Player1")
        {
            nextToPlayer = true;
            playerNextToMe = "P1";
        } else if (coll.transform.tag == "Player2")
        {
            nextToPlayer = true;
            playerNextToMe = "P2";
        }
    }

    // Change nextToPlayer to false when leaving collision.
    void OnCollisionExit2D(Collision2D coll)
    {
        nextToPlayer = false;
        playerNextToMe = null;
    }


    // Handle all player's interaction with objects.
    void PlayerObjectInteraction(string interacter) {
        if (nextToPlayer) {
            bool interact = Input.GetButton("Interact" + interacter);
            if (interact && canInteract) {
                canInteract = false;
                Debug.Log("INTERACTION by: " + interacter);

                if (parentTag == "Phone" && playerNextToMe == interacter)
                {
                    FindObjectOfType<GameManagerTutorial>().phoneAnswered = true;

                    // Maybe we should stop animating the phone when it isn't ringing,
                    // as in, after player interacts with it, set it to idle.
                    GameObject phone = GameObject.FindGameObjectWithTag("Phone");
                    phone.transform.Find("PhoneSprite").GetComponent<Animator>().SetBool("isRinging", false);

                }

                if (parentTag == "TorpedoLoaderTubeLever" && playerNextToMe == interacter)
                {
                    // switch activation state
                    animator.SetBool("IsActivated", !animator.GetBool("IsActivated"));
                    GameObject tubeSlider = GameObject.FindWithTag("PressurizedTubeSlider");
                    tubeSlider.GetComponent<PressurizedTubeScript>().Activate();

                    // FIXME: make a coroutine so you have to wait for the animation to complete first
                    //canInteract = true;
                }

                if (parentTag == "Table" && playerNextToMe == interacter)
                {
                    canInteract = true;
                    float moveHor = Input.GetAxis("Horizontal" + interacter);
                    float moveVer = Input.GetAxis("Vertical" + interacter);
                    float speed = GameObject.FindWithTag("Player").GetComponent<PlayerScript>().speed;
                    Vector2 movement = new Vector2(speed * moveHor * Time.deltaTime, speed * moveVer * Time.deltaTime);
                    rb.velocity = movement;
                }

                if (parentTag == "HatchLever" && playerNextToMe == interacter)
                {
                    // switch activation state
                    animator.SetBool("IsActivated", !animator.GetBool("IsActivated"));
                    targetAnimator.SetBool("IsActivated", !targetAnimator.GetBool("IsActivated"));

                    // FIXME: make a coroutine so you have to wait for the animation to complete first
                    //canInteract = true;
                }
            }
        }
    }
}
