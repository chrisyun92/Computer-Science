using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    public bool canInteract;
    public AudioSource phone;
    public AudioSource hatch;
    public AudioSource water;
    public Animator animator;
    public Animator targetAnimator;
    public Rigidbody2D rb;
    public float deactivationTime;
    public float elapsedTime;

    private bool unanswered = true;
    private string parentTag;
    private bool p1Holding = false;
    private bool p2Holding = false;
    private bool hatchIsOpen = false;


    //FOR TUTORIAL PURPOSES
    public bool tableNextToTorpedoRack = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (transform.tag == "Phone")
        {
            animator.SetBool("isRinging", false);
            if (GameObject.Find("GameManager").GetComponent<GameManager>().currentLevel == 0)
            {
                StartCoroutine(PhoneRinging());
            }
        }
        
    }

    IEnumerator PhoneRinging()
    {
        while (unanswered)
        {
            phone.Play();
            animator.SetBool("isRinging", true);
            yield return new WaitForSeconds(5);
        }
    }

    /* Improved PlayerObjectInteraction method */
    // Handle all player's interaction with objects.
    public void PlayerObjectInteraction(GameObject player) {
        // Only care if the object can currently be interacted with.
        if (canInteract) {
            // To prevent spam-interactions, set canInteract to false. ***TODO: Coroutine to re-enable canInteract?
            canInteract = false;

            Debug.Log("What's this? " + gameObject.name);
            /* What object is it? Check the name. */
            switch (gameObject.name) {
                case "Phone":
                    // Phone -> Answers the phone to receive messages from the bridge (captain)
                    PhoneInteraction();
                    break;

                case "TubeLever":
                    // TubeLever -> Drains the tube of water.
                    TubeLeverInteraction();
                    break;

                case "HatchLever":
                    // HatchLever -> Opens the torpedo tube hatch.
                    HatchLeverInteraction();
                    break;

                case "Table":
                    // Table -> Try to load a torpedo from table into tube.
                    TableInteraction(player);
                    break;

                case "RackLeverLeft":
                    RackInteraction(player);
                    break;

                case "RackLeverRight":
                    RackInteraction(player);
                    break;

                case "Mop":
                    MopInteraction(player);
                    break;

                case "IncreaseHealthLever":
                    IncreaseHealthLeverInteraction();
                    break;

                case "DecreaseHealthLever":
                    DecreaseHealthLeverInteraction();
                    break;

                case "Button":
                    ButtonInteraction(player);
                    break;
            }
        }
    }

    // Phone Interaction
    void PhoneInteraction() {

        unanswered = false;
        FindObjectOfType<LoadTorpedoTutorial>().phoneAnswered = true; // ***What does this do?***

        // Stop animating the phone when it isn't ringing, i.e. after player interacts with it, set it to idle.
        animator.SetBool("isRinging", false);
    }

    // TubeLever Interaction
    void TubeLeverInteraction() {
        // If the lever is animating, don't interact again.
        if (true) {
            Debug.Log("Animator: " + animator);
            Debug.Log("Animator.name: " + animator.name);
        }

        if (GameObject.Find("GameManager").GetComponent<GameManager>().currentLevel != 0)
        {
            canInteract = true;
        }

        // switch activation state
        animator.SetBool("isActivated", !animator.GetBool("isActivated"));
        GameObject tubeSlider = GameObject.FindWithTag("PressurizedTubeSlider");
        tubeSlider.GetComponent<PressurizedTubeScript>().Activate();
        GameObject.FindGameObjectWithTag("PressurizedTubeSlider").GetComponent<Hatch>().isTubeFilled =
            !GameObject.FindGameObjectWithTag("PressurizedTubeSlider").GetComponent<Hatch>().isTubeFilled;
        water.Play();
        // FIXME: make a coroutine so you have to wait for the animation to complete first
        //canInteract = true;
    }

    // HatchLever Interaction
    void HatchLeverInteraction() {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().currentLevel != 0)
        {
            canInteract = true;
        }

        hatchIsOpen = !hatchIsOpen;

        // switch activation state
        animator.SetBool("isActivated", !animator.GetBool("isActivated"));
        targetAnimator.SetBool("isActivated", !targetAnimator.GetBool("isActivated"));
        GameObject.FindGameObjectWithTag("PressurizedTubeSlider").GetComponent<Hatch>().isHatchClosed =
            !GameObject.FindGameObjectWithTag("PressurizedTubeSlider").GetComponent<Hatch>().isHatchClosed;
        hatch.Play();

        // Check for flooding: If tube was full of water and hatch gets opened, call GameManager.Flood()
        bool tubeEmpty = GameObject.Find("PressurizedTubeSlider").GetComponent<PressurizedTubeScript>().m_isEmpty;
        Debug.Log("WTF: tube empty? " + tubeEmpty);
        if (tubeEmpty != true && GameObject.Find("PressurizedTubeSlider").GetComponent<PressurizedTubeScript>().IsTubeFull()) {
            // Tube was not empty and it was not full from the start
            if (hatchIsOpen)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().Flood();
            }
        }

    }

    // Table Interactions (along with pushing the torpedo into the hatch)
    void TableInteraction(GameObject player) {
        canInteract = true;

        // Try to load a torpedo IF (1) table has a torpedo on it, (2) the hatch is open
        Hatch hatch = GameObject.FindGameObjectWithTag("PressurizedTubeSlider").GetComponent<Hatch>();
        if (GameObject.Find("Table").GetComponent<Table>().isLoaded && !hatch.isHatchClosed && !hatch.isTubeFilled) {
            hatch.DetectTable();

            // If a player is in the killZone, kill the player.
            List<GameObject> deadPlayers = GameObject.Find("Tube").GetComponent<Tube>().KillZone(true);
            if (deadPlayers != null && deadPlayers.Count > 0) {
                foreach (var p in deadPlayers) {
                    PlayerScript ps = p.GetComponent<PlayerScript>();

                    // Set player fixed joint, animator's holding bool to false, and respawn.
                    p.GetComponent<FixedJoint2D>().enabled = false;
                    p.GetComponent<Animator>().SetBool("holding", false);
                    ps.RespawnPlayer();
                }
            }
        }

        // If both players are holding the table and one player interacts with it.
        if (p1Holding && p2Holding) {
            if (player == GameObject.Find("Player_1")) {
                p1Holding = false;
            }
            if (player == GameObject.Find("Player_2")) {
                p2Holding = false;
            }
            transform.GetComponent<Rigidbody2D>().mass = 20;
            transform.GetComponent<Rigidbody2D>().drag = 100000;
            player.GetComponent<FixedJoint2D>().enabled = false;

            // Make the player's animator it's no longer holding on to the table.
            player.GetComponent<Animator>().SetBool("holding", false);
        }

        // If one player is holding onto the table and the other isnt.
        else if (p1Holding && !p2Holding) {
            if (player == GameObject.Find("Player_1")) {
                p1Holding = false;
                player.GetComponent<FixedJoint2D>().enabled = false;
                transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

                player.GetComponent<Animator>().SetBool("holding", false);
            }
            if (player == GameObject.Find("Player_2")) {
                p2Holding = true;
                player.GetComponent<FixedJoint2D>().connectedBody = rb;
                player.GetComponent<FixedJoint2D>().enabled = true;
                transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                transform.GetComponent<Rigidbody2D>().mass = 1;
                transform.GetComponent<Rigidbody2D>().drag = 0;

                // Make the player's animator realize it's holding on to the table.
                player.GetComponent<Animator>().SetBool("holding", true);
            }
        } else if (!p1Holding && p2Holding) {
            if (player == GameObject.Find("Player_1")) {
                p1Holding = true;
                player.GetComponent<FixedJoint2D>().connectedBody = rb;
                player.GetComponent<FixedJoint2D>().enabled = true;
                transform.GetComponent<Rigidbody2D>().mass = 1;
                transform.GetComponent<Rigidbody2D>().drag = 0;

                // Make the player's animator realize it's holding on to the table.
                player.GetComponent<Animator>().SetBool("holding", true);
            }
            if (player == GameObject.Find("Player_2")) {
                p2Holding = false;
                player.GetComponent<FixedJoint2D>().enabled = false;
                transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

                player.GetComponent<Animator>().SetBool("holding", false);
            }
        }
          //when no one is holding onto the table.
          else {
            if (player == GameObject.Find("Player_1")) {
                p1Holding = true;
            }
            if (player == GameObject.Find("Player_2")) {
                p2Holding = true;
            }

            player.GetComponent<FixedJoint2D>().connectedBody = rb;
            player.GetComponent<FixedJoint2D>().enabled = true;
            transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            transform.GetComponent<Rigidbody2D>().mass = 20;
            transform.GetComponent<Rigidbody2D>().drag = 100000;

            player.GetComponent<Animator>().SetBool("holding", true);
        }
    }

    // Alternate Table Interaction
    void ALTTableInteraction() {
        // Only load if the table is loaded, the table is in range, the hatch is open, and the tube is unloaded.
        bool isLoaded = gameObject.GetComponent<Table>().isLoaded;
        if (isLoaded == false) {
            return;
        }

        // Raycast to detect hatch.
        Vector2 rayOrigin = gameObject.GetComponent<Rigidbody2D>().position;
        // rayOrigin.y -= 0.3f; // Adjustments to the raycast origin.
        int layerMask = LayerMask.GetMask("Tube"); // The "Tube" layer detects is the tube is in range to be loaded.
        float interactCastDistance = 0.5f; // Adjust as needed to load from further away
        RaycastHit2D hit = Physics2D.CircleCast(rayOrigin, 0.1f, Vector2.left, interactCastDistance, layerMask);

        // If tube wasn't hit, return.
        if (!hit) {
            return;
        }

        // Check if the the hatch is open and table unloaded.
        GameObject hitObject = hit.transform.gameObject;
        if (hitObject.GetComponent<Hatch>().isHatchClosed || hitObject.GetComponent<Hatch>().isTorpedoInHatch) {
            return;
        }

        // Torpedo meets conditions for being loaded. Load it!
        gameObject.GetComponent<Table>().TableToTube();

        // If a player is in the killZone, kill the player.
        List<GameObject> deadPlayers = GameObject.Find("Tube").GetComponent<Tube>().KillZone(true);
            if (deadPlayers != null && deadPlayers.Count > 0) {
                foreach (var p in deadPlayers) {
                    PlayerScript ps = p.GetComponent<PlayerScript>();

                    // Find which player this is and set that player's holding bool to false.
                    if (ps.playerSuffix == "_P1") {
                        p1Holding = false;
                    } else {
                        p2Holding = false;
                    }

                    // Set player fixed joint, animator's holding bool to false, and respawn.
                    p.GetComponent<FixedJoint2D>().enabled = false;
                    p.GetComponent<Animator>().SetBool("holding", false);
                    ps.RespawnPlayer();
                }
            }
    }

    // Rack Interaction
    void RackInteraction(GameObject player) {
        // If the lever is deactivated, activate it and start a timer.
        if (!animator.GetBool("isActivated")) {
            if (this.gameObject == GameObject.Find("RackLeverLeft")) {
                if (GameObject.Find("RackLeverRight").GetComponent<InteractableObject>().
                    animator.GetBool("isActivated")) {
                    GameObject.Find("TorpedoRack").GetComponent<MissileRack>().DetectTable();
                }
            }
            if (this.gameObject == GameObject.Find("RackLeverRight")) {
                if (GameObject.Find("RackLeverLeft").GetComponent<InteractableObject>().
                    animator.GetBool("isActivated")) {
                    GameObject.Find("TorpedoRack").GetComponent<MissileRack>().DetectTable();
                }
            }
            // Activate the lever. 
            animator.SetBool("isActivated", true);

            // Start a timer to deactivate the lever.
            StartCoroutine(DeactivationTimer());
        }

        /* Alternate Rack interaction */
        //gameObject.GetComponent<TimerSwitch>().PlayerObjectInteraction(player);


        // *** TEMPORARY DEBUG
        GameObject.Find("Tube").GetComponent<Tube>().KillZone(true);
    }

    // Deactivation timer coroutine
    IEnumerator DeactivationTimer() {
        // While the elapsed time is less than the deactivationTime, don't do anything. 
        elapsedTime = 0f;
        while (elapsedTime < deactivationTime) {
            // Incremenet elapsedTime by the time that's passed.
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Now that the deactivationTime has passed, deactivate the object and reenable interaction.
        animator.SetBool("isActivated", false);
        canInteract = true;
    }

    // IncreaseHealthLever Interaction
    public void IncreaseHealthLeverInteraction() {
        // If the lever is animating, don't interact again.
        if (true) {
            Debug.Log("Animator: " + animator);
            Debug.Log("Animator.name: " + animator.name);
        }

        // switch activation state
        animator.SetBool("isActivated", !animator.GetBool("isActivated"));
        GameObject subHealth = GameObject.FindWithTag("SubmarineHealth");
        subHealth.GetComponent<SubmarineHealthScript>().GainHealth();
        // FIXME: make a coroutine so you have to wait for the animation to complete first
        canInteract = true;
    }

    // IncreaseHealthLever Interaction
    public void DecreaseHealthLeverInteraction() {
        // If the lever is animating, don't interact again.
        if (true) {
            Debug.Log("Animator: " + animator);
            Debug.Log("Animator.name: " + animator.name);
        }

        // switch activation state
        animator.SetBool("isActivated", !animator.GetBool("isActivated"));
        GameObject subHealth = GameObject.FindWithTag("SubmarineHealth");
        subHealth.GetComponent<SubmarineHealthScript>().LoseHealth();
        // FIXME: make a coroutine so you have to wait for the animation to complete first
        canInteract = true;

        GameObject.Find("GameManager").GetComponent<GameManager>().health -= 1;
    }

    // MopInteraction
    void MopInteraction(GameObject player) {
        canInteract = true;
        if (!player.GetComponent<FixedJoint2D>().enabled) {
            player.GetComponent<FixedJoint2D>().connectedBody = rb;
            player.GetComponent<FixedJoint2D>().enabled = true;
            transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            player.GetComponent<Animator>().SetBool("holding", true);

            player.GetComponent<PlayerScript>().hasMop = true;
        } else {
            player.GetComponent<FixedJoint2D>().enabled = false;
            transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

            player.GetComponent<Animator>().SetBool("holding", false);

            player.GetComponent<PlayerScript>().hasMop = false;
        }
    }

    // Button interaction.
    public void ButtonInteraction(GameObject player)
    {
        canInteract = true;

        // Increase score when loading a missile
        Hatch hatchVariable = GameObject.FindGameObjectWithTag("PressurizedTubeSlider").GetComponent<Hatch>();
        if (hatchVariable.isTorpedoInHatch && hatchVariable.isHatchClosed && hatchVariable.isTubeFilled)
        {
            GameObject.Find("Button").GetComponent<Animator>().SetTrigger("pressed");
            GameObject.Find("GameManager").GetComponent<GameManager>().torpedosLeft -= 1;
            GameObject.Find("GameManager").GetComponent<GameManager>().currentScore += 20;
            GameObject.Find("Table").GetComponent<Table>().DeactivateSprite();
            hatchVariable.isTorpedoInHatch = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject == GameObject.Find("TorpedoRack")) {
            tableNextToTorpedoRack = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject == GameObject.Find("TorpedoRack")) {
            tableNextToTorpedoRack = false;
        }
    }
}
