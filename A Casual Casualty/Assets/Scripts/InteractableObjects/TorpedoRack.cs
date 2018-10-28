using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This class controls the logic of torpedo rack. It is activated by a pair of timer switches.
While both switches are activated, the torpedo rack will check for the table using a raycast.
If the table is found, it will push a torpedo onto the table and then stop checking for the table.
*/
public class TorpedoRack : MonoBehaviour {
    /* Public Variables */
    /* 
     * switch1Active: A bool to track the activation status of switch 1.
     * switch2Active: A bool to track the activation status of switch 2.
     * audioSource: The audio source sound effect to be played on activation.
     * animator: The animator, which allows for control of the object sprite and animation.
     * targetObject: The object that this object will interact with and cause changes to.
     */
    public bool switch1Active = false;
    public bool switch2Active = false;
    public AudioSource aud;
    public Animator animator;
    public GameObject targetObject;
    public float interactCastRadius = 0.35f;
    public float interactCastDistance = 0.5f;

    // Activates this object. For the Tropedo Rack, this means both switches must be active to work.
    public void ActivateObject() {
        // Logic to determine which switches need to be set to active.
        if (switch1Active) {
            switch2Active = true;

            // Both switches are now active. Check for Table.
            TableCheck();

            // Activate the object.
            //animator.SetBool("isActivated", true);
            //aud.Play();
        } else {
            // No switches had been active, so activate the first.
            switch1Active = true;
        }
    }

    // Deactivation means de-priming the switch acivations bools.
    public void DeactivateObject() {
        // Logic to determine which switches need to be deactivated.
        if (switch2Active) {
            // Both are active, deactivate 2.
            switch2Active = false;
        } else {
            // Only switch 1 is active. Deactivate it.
            switch1Active = false;
        }
    }

    private void TableCheck() {
        // Raycasting to see if table interacts with an object.
        Vector2 rayOrigin = gameObject.GetComponent<Rigidbody2D>().position;
        rayOrigin.y -= 0.3f;
        Vector2 facing = new Vector2(0,-1);
        int layerMask = LayerMask.GetMask("Interactables"); // The "Interactables" layer is the one with our interactable objects.
        RaycastHit2D hit = Physics2D.CircleCast(rayOrigin, interactCastRadius, facing, interactCastDistance, layerMask);

        // If something was hit, hit won't be null. 
        if (hit && hit.transform.gameObject.name == "Table") {
            GameObject hitObject = hit.transform.gameObject;

            // Now that we know the table was hit, we can call its ActivateObject method to load a torpedo onto it.
            hitObject.GetComponent<Table>().RackToTable();
        }
    }
}
