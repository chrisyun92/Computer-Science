using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* FillTube is designed to fill or drain a torpedo tube.
 */
public class FillTube : MonoBehaviour {

    public bool canInteract;
    public Slider slider;
    public float maxWaterLevel = 100f;
    public float timeToActivate = 1f;
    public float fillVelocity = 0.0f;
    public AudioSource audio;

    private bool isEmpty = false;

    public bool Activate() {
        // If canInteract is false, return false; the interaction failed.
        if (canInteract == false) {
            return false;
        }

        // Play the water drain / fill audio.
        audio.Play();

        // Start the fill coroutine.
        StartCoroutine(ActivateTube(isEmpty));

        // Change is empty and return true for a successful interaction.
        isEmpty = !isEmpty;
        return true;
    }

    IEnumerator ActivateTube(bool shouldFill) {
        float elapsedTime = 0f;
        while (elapsedTime < timeToActivate) {
            elapsedTime += Time.deltaTime;
            if (shouldFill) {
                slider.value = maxWaterLevel * elapsedTime / timeToActivate;
            } else {
                slider.value = maxWaterLevel * (1 - elapsedTime / timeToActivate);
            }
            yield return null;
        }
    }
}
