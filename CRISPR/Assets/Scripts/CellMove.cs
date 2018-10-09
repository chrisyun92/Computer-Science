using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMove : MonoBehaviour {

    public float speed = 0.3f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(0, speed, 0);
	}
}
