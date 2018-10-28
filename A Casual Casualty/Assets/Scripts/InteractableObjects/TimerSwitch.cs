using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    This class controls the logic of timer switches. Timer switches are objects which are activatable by the players,
then stay activated for some time before automatically deactivating. Once activated, players cannot then deactivate
the switch until it automatically deactivates itself.

    One instance of timer switches are the Torpedo Rack levers, which, in addition to being timed, must also be 
activated as a pair. 
*/
public class TimerSwitch : MonoBehaviour {

    /* Public Variables */
    /* 
     * deactivationTime: Time that this switch is activated before automatically deactivating.
     * elapsedTime: Time that has elapsed since activating.
     * canInteract: A boolean to track whether the player can interact with this object.
     * audioSource: The audio source sound effect to be played on activation.
     * animator: The animator, which allows for control of the object sprite and animation.
     * targetObject: The object that this switch will interact with and cause changes to.
     */
    public float deactivationTime;
    public float elapsedTime;
    public bool canInteract;
    public AudioSource aud;
    public Animator animator;
    public GameObject targetObject;

    /* Player-Object Interaction */
    // PlayerScript expects this method of all interactable objects.
    public void PlayerObjectInteraction(GameObject player) {
        // If the lever is deactivated, activate it and start a timer.
        if (!animator.GetBool("isActivated")) {
            // Activate the lever. 
            animator.SetBool("isActivated", true);
            aud.Play();

            // Start a timer to deactivate the lever.
            StartCoroutine(DeactivationTimer(player));

            // Send information to the targetObject to potentially activate it.
            TargetObjectInteraction(true, player);
        }
    }

    // Deactivation timer coroutine
    IEnumerator DeactivationTimer(GameObject player) {
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

        // Must also send a signal to the target object that we're no longer interacting with it.
        TargetObjectInteraction(false, player);
    }

    // Target Object Interaction.
    public void TargetObjectInteraction(bool interaction, GameObject player) {
        if (interaction) {
            targetObject.GetComponent<TorpedoRack>().ActivateObject();
        } else {
            targetObject.GetComponent<TorpedoRack>().DeactivateObject();
        }
    }
}
