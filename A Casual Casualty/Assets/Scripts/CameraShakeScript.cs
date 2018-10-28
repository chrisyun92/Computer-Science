using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeScript : MonoBehaviour {
    public Camera cam;
    public Vector3 originalPosition = new Vector3(0, 0.4f, -10);
    public float x_range;  // total range the camera can move along x axis
    public float y_range;  // total range that the camera can move along y axis
    public float duration;

	// Use this for initialization
	void Start () {
        originalPosition = cam.transform.position;
        //ShakeCamera();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShakeCamera()
    {
        StartCoroutine(Flash(duration));
    }

    IEnumerator Flash(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float x_displacement = Random.Range(-x_range, x_range);
            float y_displacement = Random.Range(-y_range, y_range);
            cam.transform.position += new Vector3(x_displacement, 0, 0);
            cam.transform.position += new Vector3(y_displacement, 0, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
            cam.transform.position = originalPosition;
        }
        cam.transform.position = originalPosition;
    }
}
