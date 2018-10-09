using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewportResizing : MonoBehaviour {

    Transform child;
    RectTransform rt;

	// Use this for initialization
	void Start () {
        child = GetComponentInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
