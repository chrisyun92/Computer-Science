using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Chain : MonoBehaviour {

	public Transform player;

	public float speed = 2f;

	public static bool IsFired;

    public static bool laserNoise = true;

	// Use this for initialization
	void Start () {
		IsFired = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown("Fire1") && laserNoise)
		{
			IsFired = true;
            if (laserNoise)
            {
                FindObjectOfType<AudioManager>().Play("Laser");
            }
        }
		
		if (IsFired)
		{
            transform.localScale = transform.localScale + Vector3.up * Time.deltaTime * speed;
            laserNoise = false;
		} else
		{
			transform.position = player.position;
			transform.localScale = new Vector3(1f, 0f, 1f);
		}

	}

    public void ChangeNoise ()
    {
        laserNoise = !laserNoise;
    }
}
