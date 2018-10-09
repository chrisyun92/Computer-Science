using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class MobileLeaderBoard : MonoBehaviour
{
    ////FOR CHRIS' COMPUTER:
    //public void OnShowLeaderBoard()
    //{
    //    return;
    //}

    //public void OnAddScoreToLeaderBoard()
    //{
    //    return;
    //}


    //DELETE COMMENT MARK BELOW
    #region PUBLIC_VAR
    string leaderboard;
    private static bool loggedIn;
    #endregion
    #region DEFAULT_UNITY_CALLBACKS

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
#if UNITY_ANDROID
        leaderboard = "CgkIhZ-5_roCEAIQAA";
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;

        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
        if (!loggedIn)
        {
            LogIn();
            loggedIn = false;
        }
#endif

#if UNITY_IPHONE
        leaderboard = "crispr_high_score";
        Social.localUser.Authenticate(success =>
                                      {
            if (success)
            {
                Debug.Log("Authentication successful");
            }
            else
            {
                Debug.Log("Authentication failed");
            }
        });
#endif
    }
    #endregion
    #region BUTTON_CALLBACKS
    /// <summary>
    /// Login In Into Your Google+ Account
    /// </summary>
    public void LogIn()
    {
#if UNITY_ANDROID
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Login Sucess");
            }
            else
            {
                Debug.Log("Login failed");
            }
        });
#endif
    }
    /// <summary>
    /// Shows All Available Leaderborad
    /// </summary>
    public void OnShowLeaderBoard()
    {
#if UNITY_ANDROID
        //        Social.ShowLeaderboardUI (); // Show all leaderboard
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboard); // Show current (Active) leaderboard
#endif

#if UNITY_IPHONE
            Social.ShowLeaderboardUI();
#endif
    }
    /// <summary>
    /// Adds Score To leader board
    /// </summary>
    public void OnAddScoreToLeaderBoard()
    {
        int score = GameObject.Find("ScoreController").GetComponent<ScoreController>().getScore();
#if UNITY_ANDROID
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, leaderboard, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Update Score Success");

                }
                else
                {
                    Debug.Log("Update Score Fail");
                }
            });
        }
        else
        {
            Debug.Log("Not logged in");
        }
#endif

#if UNITY_IPHONE
        //Debug.Log("Reporting score " + score + " on leaderboard " + leaderboardID);
        Social.ReportScore(score, leaderboard, success =>
           {
            if (success)
            {
                Debug.Log("Reported score successfully");
            }
            else
            {
                Debug.Log("Failed to report score");
            }
 
            Debug.Log(success ? "Reported score successfully" : "Failed to report score"); Debug.Log("New Score:"+score);  
        });
#endif
    }



    /// <summary>
    /// On Logout of your Google+ Account
    /// </summary>
    public void OnLogOut()
    {
#if UNITY_ANDROID
        ((PlayGamesPlatform)Social.Active).SignOut();
#endif
    }
    #endregion
    
}
//DELETE COMMENT MARK ABOVE
