using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class playerScores : MonoBehaviour, IScore {
    public DateTime date
    {
        get
        {
            return System.DateTime.Now.Date;
        }
    }

    public string formattedValue
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public string leaderboardID
    {
        get
        {
            return leaderboardID;
        }

        set
        {
            leaderboardID = "HighScores";
        }
    }

    public int rank
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public string userID
    {
        get
        {
            return userID;
        }
    }

    public long value
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public void ReportScore(Action<bool> callback)
    {
        //Social.ReportScore(value, leaderboardID);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
