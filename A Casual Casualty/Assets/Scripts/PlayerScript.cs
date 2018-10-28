using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerScript : MonoBehaviour {

    public string playerSuffix;
    public float speed;
    public Vector2 facing;
    public Vector2 movement;
    public bool collided = false;
    public bool hasMop;
    public float interactCastRadius = 0.35f;
    public float interactCastDistance = 0.5f;
    public float respawnTime = 2.0f;
    public float playerSpeedReduction = 1.0f;
    public GameObject heldObject;
    public AudioSource audSource;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D playerBody;
    private Collider2D playerCollider;
    private float elapsedTime = 0f;
    private float floodResetSpeedValue;

    // Use this for initialization
    void Awake() {
        floodResetSpeedValue = speed;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        facing = new Vector2(0,-1); // Down vector. Begin facing down.
    }

    private void Update() {
        /* Paused Detection*/
        bool gameIsPaused = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PauseMenu>().GameIsPaused();
        if (gameIsPaused) {
            return;
        }

        /* Player Input: Holding detection */
        bool hold = Input.GetButtonDown("Hold" + playerSuffix);
        if (hold) {
            // If player has pressed the hold button, call HoldObject.
            Debug.Log("Hold? " + hold);
            HoldObject();
        }

        /* Player Input: Release from holding detection */
        bool release = Input.GetButtonUp("Hold" + playerSuffix);
        if (release) {
            // If player has let go of the hold button, call ReleaseObject
            Debug.Log("Release? " + release);
            ReleaseObject();
        }

        /* Player Input: Interact key */
        bool interact = Input.GetButtonDown("Interact" + playerSuffix);
        if (interact) {
            // If player has pressed the hold button, call HoldObject.
            Debug.Log("Interact? " + interact);

            // Trigger player interaction animation.
            animator.SetTrigger("interact");

            // Raycasting to see if player interacts with an object.
            Vector2 rayOrigin = gameObject.GetComponent<Rigidbody2D>().position;
            rayOrigin.y -= 0.3f;
            int layerMask = LayerMask.GetMask("Interactables"); // The "Interactables" layer is the one with our interactable objects.
            RaycastHit2D hit = Physics2D.CircleCast(rayOrigin, interactCastRadius, facing, interactCastDistance, layerMask);

            // If something was hit, hit won't be null. 
            if (hit) {
                GameObject hitObject = hit.transform.gameObject;
                Debug.Log("Object hit: " + hitObject.name);

                // Now that we know the object hit, we can call that objects PlayerObjectInteraction script.
                hitObject.GetComponent<InteractableObject>().PlayerObjectInteraction(gameObject);
            }
        }
    }

    private void FixedUpdate() {
        /* Player Input: Movement keys */
        float moveHor = Mathf.Clamp(Input.GetAxis("Horizontal" + playerSuffix) + Input.GetAxis("J_Horizontal" + playerSuffix), -1.0f, 1.0f);
        float moveVer = Mathf.Clamp(Input.GetAxis("Vertical" + playerSuffix) + Input.GetAxis("J_Vertical" + playerSuffix), -1.0f, 1.0f);
        movement = new Vector2(moveHor, moveVer);
        movement.Normalize(); // Normalize the movement vector.
        movement.x = movement.x * speed * Time.deltaTime * playerSpeedReduction;
        movement.y = movement.y * speed * Time.deltaTime * playerSpeedReduction;
        playerBody.velocity = movement;

        /* Set Walking / Idle BlendTree and set the player facing vector */
        // Check for player movement as well as if they're holding object, facing held in place.
        if (Math.Abs(movement.x) + Math.Abs(movement.y) > 0) { 
            // If not holding, move and change facing normally.
            if (!animator.GetBool("holding")) {
                animator.SetFloat("dirX", movement.x);
                animator.SetFloat("dirY", movement.y);
                animator.SetBool("walking", true);

                // Set player facing vector to be equal to the movement vector. By updating here but not in idle,
                // the player's facing will not change until they move again.
                facing = movement;
            } else {
                // Player is holding a heavy object. Set animator parameters based on last facing.
                animator.SetFloat("dirX", facing.x);
                animator.SetFloat("dirY", facing.y);
                animator.SetBool("walking", true);
            }
        } else {
            animator.SetBool("walking", false);
        }

        spriteRenderer.sortingOrder = 15 * 5 - ((int)(5 * playerBody.position[1]));  // Sprites with higher position.y values render on a lower order.
    }

    public string PlayerSuffix()
    {
        return playerSuffix;
    }

    public bool HasMop()
    {
        return hasMop;
    }

    public void pickedUpMop()
    {
        hasMop = true;
    }

    // Player killer and respawner
    public void RespawnPlayer() {
        StartCoroutine(RespawnPlayerTimer());
    }

    IEnumerator RespawnPlayerTimer() {

        // Stop player movement.
        speed = 0f;

        // Wait a few seconds before spawning the player back in.
        elapsedTime = 0f;

        // Player disappears when dead
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        audSource.Play();

        while (elapsedTime < respawnTime) {
            // Incremenet elapsedTime by the time that's passed.
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set player position down to the bottom right corner: x,y = 12,-4
        gameObject.GetComponent<Transform>().position = new Vector2(12, -4);

        // Make sure players aren't holding anything when they respawn.
        transform.GetComponent<FixedJoint2D>().enabled = false;
        transform.GetComponent<Animator>().SetBool("holding", false);

        // Player reappears
        gameObject.GetComponent<SpriteRenderer>().enabled = true;

        // resume player movement.
        speed = 300f;
        

    }

    public void ReduceSpeed()
    {
        playerSpeedReduction = 0.5f;
    }

    public void RestoreSpeed()
    {
        playerSpeedReduction = 1.0f;
    }

    /* Holding */
    //      Attempt to grab hold of an object, if any are nearby.
    public GameObject HoldObject() {
        // Throw a short cast out in front of player.
        // Trigger player interaction animation.
        animator.SetTrigger("interact");

        // Raycasting to see if player interacts with an object.
        Vector2 rayOrigin = gameObject.GetComponent<Rigidbody2D>().position;
        rayOrigin.y -= 0.3f;
        int layerMask = LayerMask.GetMask("Holdables"); // The "Holdables" layer is the one with holdable objects.
        RaycastHit2D hit = Physics2D.CircleCast(rayOrigin, interactCastRadius, facing, interactCastDistance, layerMask);

        // If something was hit, hit won't be null. 
        if (hit) {
            GameObject hitObject = hit.transform.gameObject;
            Debug.Log("Object held: " + hitObject.name); // ***DEBUG***

            // Enable player's FixedJoint2D and connect the hit objects RigidBody2D to it.
            gameObject.GetComponent<FixedJoint2D>().connectedBody = hitObject.GetComponent<Rigidbody2D>();
            gameObject.GetComponent<FixedJoint2D>().enabled = true;

            // Send a message to the held object to have it change it's state, if needed.
            hitObject.GetComponent<HeldObject>().PlayerToObjectHold(gameObject);

            // Set our player's animator to "holding", and store the held object as a variable.
            gameObject.GetComponent<Animator>().SetBool("holding", true);
            heldObject = hitObject;

            // Return the held Object.
            return hitObject;
        } else {
            // No object is hit, return null.
            return null;
        }
    }

    /* Release Object */
    //      Release an object when the "hold" button is no longer held down.
    public void ReleaseObject() {
        // If heldObject is null, return, as there is no object to release.
        if (heldObject == null) {
            return;
        }

        // Send a message to the held object to have it change it's state, if needed.
        heldObject.GetComponent<HeldObject>().PlayerToObjectRelease(gameObject);

        Debug.Log("In: ReleaseObject. What is held? " + heldObject.name);

        // Disable fixed joint and set the connected body (held object) to null.
        gameObject.GetComponent<FixedJoint2D>().connectedBody = null;
        gameObject.GetComponent<FixedJoint2D>().enabled = false;

        // Set player's animator to not holding. Set heldObject to null;
        gameObject.GetComponent<Animator>().SetBool("holding", false);
        heldObject = null;
    }
}
