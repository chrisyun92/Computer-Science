using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsVolume : MonoBehaviour {

    public AudioMixer main;
    public AudioMixer mainMenu;

    public void MasterVolume(float volume)
    {
        main.SetFloat("volume", volume);
        mainMenu.SetFloat("music", volume);
    }
}
