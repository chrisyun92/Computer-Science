using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoStopper : MonoBehaviour {

    // Torpedo Stopper should stop a torpedo when this objects collider is triggered.
    private void OnTriggerEnter2D(Collider2D collision) {
        // Get the object that is the subject of the collision: the torpedo.
        GameObject torpedo = collision.transform.gameObject;

        // Set the torpedo velocity to zero.
        torpedo.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
