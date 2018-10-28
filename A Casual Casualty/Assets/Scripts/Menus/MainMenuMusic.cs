using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusic : MonoBehaviour {
    public AudioSource music;

	// Use this for initialization
	void Start () {
        StartCoroutine(Music());
	}
	
	IEnumerator Music()
    {
        while (true)
        {
            music.Play();
            yield return new WaitForSeconds(16);
        }
    }
}
