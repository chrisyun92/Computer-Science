using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTutorialnfo : MonoBehaviour {
    private bool skipTutorial;

    private void Start()
    {
        skipTutorial = false;
    }
    public void SkipButton()
    {
        skipTutorial = !skipTutorial;
    }

    public bool shouldISkipTutorial()
    {
        return skipTutorial;
    }
}
