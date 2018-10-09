using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBubbler : MonoBehaviour {

    public interface CollisionBubblerReceiver {
        void OnChildCollisionEnter(string collisionTag, Collision2D col);
    }

    public GameObject parentObject;
    private CollisionBubblerReceiver parent;
    public string collisionTag = "";

    private void OnCollisionEnter2D(Collision2D collision) {
        parent.OnChildCollisionEnter(collisionTag, collision);
    }

	// Use this for initialization
	void Start () {
        parent = parentObject.GetComponent<CollisionBubblerReceiver>();
	}
}
