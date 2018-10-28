using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

    public Text timer;
    public float timeLeft;
    public AudioSource victoryHorn;
    private int currentLevel;
    private bool disabled = false;
    public bool timerStart = false;

    public bool timerReachesZeroHappened = false;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        currentLevel = GameObject.Find("GameManager").GetComponent<GameManager>().currentLevel;
        if (currentLevel != 0 && timerStart)
        {
            timeLeft -= Time.deltaTime;
        }

        // When theres no time left players will stop moving.
        if ((int) timeLeft <= 0 && !timerReachesZeroHappened)
        {
            // variable is so this loop doesn't repeat
            timerReachesZeroHappened = true;

            GameObject.Find("GameManager").GetComponent<GameManager>().NoMusic();
            timer.text = ":00";
            GameObject.Find("Player_1").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GameObject.Find("Player_2").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

            // decrease health if timers at 0 but you didn't finish loading torpedos
            if (GameObject.Find("GameManager").GetComponent<GameManager>().torpedosLeft != 0)
            {
                GameObject.Find("ControlPanel").transform.Find("DecreaseHealthLever").
                    GetComponent<InteractableObject>().DecreaseHealthLeverInteraction();
            }

            // show message when finishing a level
            GameObject.Find("GameManager").transform.GetChild(currentLevel).Find("Finished").
                GetComponent<CaptainDialogueStart>().canTalk = true;
            GameObject.Find("GameManager").transform.GetChild(currentLevel).Find("Finished").
                GetComponent<CaptainDialogueStart>().textBoxDisappears = true;

            // queues the next level up
            GameObject.Find("GameManager").transform.GetChild(currentLevel).Find("Finished").
                GetComponent<CaptainDialogueStart>().roundStart = true;

            //stops the timer so it doesn't fall below 0
            timerStart = false;
        }

        // Displays the time remaining
        else
        {
            int minutesLeft = (int) timeLeft / 60;
            int secondsLeft = (int)timeLeft % 60;
            if (secondsLeft < 10)
            {
                timer.text = minutesLeft + ":0" + (int)secondsLeft;
            }
            else
            {
                timer.text = minutesLeft + ":" + (int)secondsLeft;
            }
        }
	}

    public void HornPlay()
    {
        if (!disabled)
        {
            victoryHorn.Play();
            disabled = true;
        }
    }
}
