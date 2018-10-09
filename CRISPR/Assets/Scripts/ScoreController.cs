using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    [SerializeField]
    private Text score;
    int scoreNum;

    public void updateScore(int num)
    {
        scoreNum += num;
        score.text = "Score: " + scoreNum.ToString();
    }

    public int getScore()
    {
        return scoreNum;
    }
}
