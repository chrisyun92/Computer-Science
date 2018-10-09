using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour {

    public string sliderName;
    AudioSource audio;
    AudioClass[] fxControl;
    Slider selfSlide;

	// Use this for initialization
	void Start () {
        selfSlide = GetComponent<Slider>();
        StartCoroutine("SetSlider");

	}
	
	// Update is called once per frame
	void Update () {
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("starting on scene loaded");
        StartCoroutine("SetSlider");
    }

    IEnumerator SetSlider()
    {
        Debug.Log("inside set slider");
        yield return new WaitForEndOfFrame();
        if (sliderName.Equals("AudioController"))
        {
            GameObject temp = GameObject.Find(sliderName);
            if (temp != null)
            {
                audio = temp.GetComponent<AudioSource>();
            }
            selfSlide.value = audio.volume;
            //audio.volume = selfSlide.value;
        } else if (sliderName.Equals("SFXController"))
        {
            GameObject temp = GameObject.Find("SFXController");
            if (temp != null)
            {
                Debug.Log("object found");
                Debug.Log(temp.GetComponent<AudioButtonController>());
                fxControl = temp.GetComponent<AudioButtonController>().sounds;
            }
            Debug.Log(fxControl);
            foreach (AudioClass s in fxControl)
            {
                selfSlide.value = s.volume;
                //s.volume = selfSlide.value;
            }
        }
    }

    public void ChangeVolume()
    {
        Debug.Log(audio);
        if (audio == null)
        {
            StartCoroutine("SetSlider");
        }
        audio.volume = selfSlide.value;
    }

    public void ChangeSFXVolume()
    {
        if (fxControl == null)
        {
            StartCoroutine("SetSlider");
        }
        foreach (AudioClass s in fxControl)
        {
            s.volume = selfSlide.value;
        }
    }
}
