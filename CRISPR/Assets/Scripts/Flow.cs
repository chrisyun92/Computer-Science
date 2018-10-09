using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Flow : MonoBehaviour {

    Rigidbody2D rb;
    public Transform to;
    public float strength = 1.0f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        if (to != null)
        {
            if (Time.timeScale != 0)
            {
                Vector2 force = (to.position - transform.position).normalized * strength;
                rb.AddForceAtPosition(force, (Vector2)transform.position + Random.insideUnitCircle, ForceMode2D.Force);
            }
        }
	}

    void Spawned(SpawnArgs args) {
        to = args.flowObject.transform;
    }
}
 