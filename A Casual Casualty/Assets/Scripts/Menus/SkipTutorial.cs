using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkipTutorial : MonoBehaviour {

    public GameObject redText;
    public GameObject greenText;

    private bool skipping = false;

    private void Start()
    {
        greenText.SetActive(false);
    }

    public void CallSkipButton()
    {
        skipping = !skipping;
        GameObject.FindGameObjectWithTag("skip").GetComponent<SkipTutorialnfo>().SkipButton();
        if (skipping)
        {
            redText.SetActive(false);
            greenText.SetActive(true);
        } else
        {
            redText.SetActive(true);
            greenText.SetActive(false);
        }
    }
}
