using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {


    public Sound[] sounds;


    public AudioMixerGroup musicGroup;


    public AudioMixerGroup foleyGroup;
	// Use this for initialization
	void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;

            if (sound.name == "Music")
            {
                sound.source.outputAudioMixerGroup = musicGroup;
                sound.source.loop = true;
            } else
            {
                sound.source.outputAudioMixerGroup = foleyGroup;
            }

        }
    }

    void Start()
    {
        Play("Music");
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("No music");
            return;
        }
        s.source.Play();
    }
}
