using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {

    public GameObject pauseScreen;
    public GameObject gameUI;
    public GameObject gameOverMaster;
    public GameObject gameOverUI;
    public GameObject submitScoreUI;
    public GameObject newGameUI;

    GameObject curr;

    bool isPaused;
    bool scoreSubmitted;

	// Use this for initialization
	void Start () {
        gameUI.SetActive(true);
        curr = gameUI;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1;
        }
    }

    public void goToPause()
    {
        curr.SetActive(false);
        pauseScreen.SetActive(true);
        curr = pauseScreen;
        OnApplicationPause(true);
    }

    public void goToGame()
    {
        curr.SetActive(false);
        gameUI.SetActive(true);
        curr = gameUI;
        OnApplicationPause(false);
    }

    public void gameOver()
    {
        gameOverMaster.SetActive(true);
        curr.SetActive(false);
        gameOverUI.SetActive(true);
        curr = gameOverUI;
    }

    public void submitScore()
    {
        curr.SetActive(false);
        //submitScoreUI.SetActive(true);
        gameObject.GetComponent<MobileLeaderBoard>().OnAddScoreToLeaderBoard();
        scoreSubmitted = true;
        curr = submitScoreUI;
        gameObject.GetComponent<MobileLeaderBoard>().OnShowLeaderBoard();
        newGame();
    }

    public void newGame()
    {
        //do stuff to submit score
        //MobileLeaderBoard.OnAddScoreToLeaderBoard("1000"); //temporary placeholder
        curr.SetActive(false);
        newGameUI.SetActive(true);
        GameObject submitText = GameObject.Find("SubmittedText");
        if (!scoreSubmitted)
        {
            submitText.SetActive(false);
        }
        curr = newGameUI;
    }

    public void goToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("Menu");
    }

    public void restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void goToLead()
    {
        //MobileLeaderBoard.OnShowLeaderBoard();
    }
}
