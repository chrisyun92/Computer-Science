using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileControls : MonoBehaviour {

    [SerializeField]
    static bool isPlayerLoggedIn;
    
    //static MobileControls mobileInstance;

    //public InputField scoreInput;

    //public Text success;

    //private void Awake()
    //{
    //    if (mobileInstance == null)
    //    {
    //        mobileInstance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Object.Destroy(gameObject);
    //        string asdf = LeaderBoard.leaderboard_crispr_defense_high_score;
    //    }

    //    Screen.orientation = ScreenOrientation.Portrait;
    //    MobileLeaderBoard.configureLeaderBoard();
    //    //isPlayerLoggedIn = LeaderBoard.LogIn();

    //    //if (!isPlayerLoggedIn)
    //    //{
    //    //    LeaderBoard.configureLeaderBoard();
    //    //    isPlayerLoggedIn = LeaderBoard.LogIn();
    //    //}
    //}

    //public void logIn()
    //{
    //    string loggingIn = MobileLeaderBoard.LogIn().ToString();
    //    success.text = loggingIn;
    //}

    //public void showLeaderBoard()
    //{
    //    MobileLeaderBoard.OnShowLeaderBoard();
    //}

    //public void addScore()
    //{
    //    MobileLeaderBoard.OnAddScoreToLeaderBoard(scoreInput.text);
    //}
}
