using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager audioInstance;

    private void Awake()
    {
        if (audioInstance == null)
        {
            audioInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("destroyed");
            Object.Destroy(gameObject);
        }
    }
}
