using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        saveData.Load();
        Debug.Log("retreiving savedata");
        Debug.Log(saveData.savedGame);
        if (saveData.savedGame != null)
        {
            Debug.Log("OpenedBefore: " + saveData.savedGame.openedBefore);
            if (saveData.savedGame.openedBefore)
            {
                SceneManager.LoadScene("Menu");
                SceneManager.UnloadSceneAsync("Start");
            }
            else
            {
                SceneManager.LoadScene("Opening");
                SceneManager.UnloadSceneAsync("Start");
            }
        }
        else
        {
            SceneManager.LoadScene("Opening");
            SceneManager.UnloadSceneAsync("Start");
        }

    }

    // Update is called once per frame
    void Update () {
		
	    }
}