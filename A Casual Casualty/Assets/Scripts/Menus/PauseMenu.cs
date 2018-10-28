using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public string menuSceneName = "MainMenu";

    private void Update() {
        if (Input.GetButtonDown("Cancel")){
            if (gameIsPaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    public void ResumeGame() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void PauseGame() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void LoadMenu(string sceneName) {
        Time.timeScale = 1f;
        gameIsPaused = false;
        Debug.Log("menuSceneName: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public bool GameIsPaused() {
        return gameIsPaused;
    }
}
