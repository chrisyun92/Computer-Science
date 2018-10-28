using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* HeldObject Script:
 * Purpose: attach this script to all objects which are holdable by the players. Objects with
 *  this script will receive a signal from the player letting the object know that it is held
 *  or released, allowing for changes of state, like making the item heavy when not held but 
 *  light when held (as is the case with the table).
 *  
 * Variables: 
 *      heldMassFactor: MUST NEVER BE ZERO. If < 1.0f, the object gets lighter by some factor when held.
 *          If == 1.0f, no change when held. If > 1.0f, it gets heavier when held.
 *      heldDragFactor: MUST NEVER BE ZERO. If < 1.0f, the object has less drag by some factor when held.
 *          If == 1.0f, no change when held. If > 1.0f, it gets more drag when held.
 *      
 */
public class HeldObject : MonoBehaviour {

    public float heldMassFactor = 1.0f;
    public float heldDragFactor = 1.0f;

    // PlayerToObjectHold: Lets the object know it is being held.
    public void PlayerToObjectHold(GameObject player) {
        // Adjust object mass and drag by their factors.
        gameObject.GetComponent<Rigidbody2D>().mass = gameObject.GetComponent<Rigidbody2D>().mass * heldMassFactor;
        gameObject.GetComponent<Rigidbody2D>().drag = gameObject.GetComponent<Rigidbody2D>().drag * heldDragFactor;
    }

    // PlayerToObjectRelease: Lets the object know it is released.
    public void PlayerToObjectRelease(GameObject player) {
        // Adjust object mass and drag by their factors.
        gameObject.GetComponent<Rigidbody2D>().mass = gameObject.GetComponent<Rigidbody2D>().mass / heldMassFactor;
        gameObject.GetComponent<Rigidbody2D>().drag = gameObject.GetComponent<Rigidbody2D>().drag / heldDragFactor;
    }
}
