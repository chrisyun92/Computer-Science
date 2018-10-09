using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOscillator : MonoBehaviour {

    public float range = 1.0f;
    public float frequency = 1.0f;
    public bool onPhysicsBody = true;

    private TargetJoint2D tj;
    private float time = 0.0f;
    private Vector3 startingPoint;

	// Use this for initialization
	void Start () {
        startingPoint = transform.position;
		if (onPhysicsBody) {
            tj = gameObject.AddComponent<TargetJoint2D>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPosition = startingPoint + (transform.right * Mathf.Sin(time * frequency * 2 * Mathf.PI) * (range / 2));
        if (onPhysicsBody) {
            tj.target = newPosition;
        } else {
            transform.position = newPosition;
        }
        time += Time.deltaTime;
	}

    private void OnDrawGizmos() {
        Vector3 half = transform.right * range / 2;
        Vector3 offset = transform.right * Mathf.Sin(time * 2 * frequency * Mathf.PI) * (range / 2);
        Gizmos.DrawLine(transform.position - half - offset, transform.position + half - offset);
    }
}
