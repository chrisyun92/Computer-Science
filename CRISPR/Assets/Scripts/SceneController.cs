using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject options;
    public GameObject leaderBoard;
    public GameObject crispr;
    public GameObject credits;
    public GameObject global;
    public GameObject weekly;
    public Text leadText;

    GameObject curr;

    private void Awake()
    {
        curr = mainMenu;
        curr.SetActive(true);
        global.GetComponentInChildren<Image>().color = new Color(137, 172, 212);
        weekly.GetComponentInChildren<Image>().color = Color.gray;
        leadText.text = "showing all of the text for global";
    }

    public void goToCrisprInfo()
    {
        Application.OpenURL("https://innovativegenomics.org/");
    }

    public void goToCLEAR()
    {
        Application.OpenURL("https://clear-project.org/");
    }

    public void goToScene(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }

    public void goToOptions()
    {
        curr.SetActive(false);
        options.SetActive(true);
        curr = options;
    }

    public void goToMenu()
    {
        curr.SetActive(false);
        mainMenu.SetActive(true);
        curr = mainMenu;
    }

    public void goToLead()
    {
        curr.SetActive(false);
        leaderBoard.SetActive(true);
        //MobileLeaderBoard.OnShowLeaderBoard();
        curr = leaderBoard;
    }

    public void goToCrispr()
    {
        curr.SetActive(false);
        crispr.SetActive(true);
        curr = crispr;
    }

    public void goToCredits()
    {
        curr.SetActive(false);
        credits.SetActive(true);
        curr = credits;
    }

    public void goToTutorial()
    {
        curr.SetActive(false);
        mainMenu.SetActive(true);
        curr = mainMenu;
        SceneManager.LoadScene("Tutorial");
        SceneManager.UnloadSceneAsync("Menu");
    }

    public void getLead(string name)
    {
        switch(name){
            case "Global":
                global.GetComponentInChildren<Image>().color = new Color(137, 172, 212);
                weekly.GetComponentInChildren<Image>().color = Color.gray;
                break;
            case "Weekly":
                weekly.GetComponentInChildren<Image>().color = new Color(137, 172, 212);
                global.GetComponentInChildren<Image>().color = Color.gray;
                break;
        }
        //get leaderboard for global or weekly, methods to be implemented
        leadText.text = "showing all of the text for " + name;
    }
}
