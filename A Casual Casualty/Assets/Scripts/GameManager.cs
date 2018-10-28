using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private IEnumerator coroutine;
    public int currentLevel;
    public int currentScore;
    public int torpedosLeft;
    public float FloodPercent;
    public float WaitTimeForCall;
    public float angle = 0.0f;
    public float gravity = -10f;
    public AudioSource music;
    public AudioSource mainMusic;
    public AudioSource victoryMusic;
    public bool levelInProgress = false;
    public int health = 4;

    public Text countdownUI;
    public Text torpedosLeftUI;
    public Text scoreUI;

    private bool playMusic = true;
    private int myInt = 0;
    private int countdownTime = 3;

    // Use this for initialization
    void Start () {
        countdownUI.enabled = false;
        coroutine = ShouldIFlood();
        StartCoroutine(coroutine);
        StartCoroutine(AdjustGravity());
        StartCoroutine(MusicLoop());
        if (GameObject.Find("SkipTutorialInfo").GetComponent<SkipTutorialnfo>().shouldISkipTutorial())
        {
            currentLevel = 1;
        }
    }

    private void Update()
    {
        checkMusic();
        if (health == 0)
        {
            //Change game to menu screen
            //CHRIs CHRiS

            SceneManager.LoadScene(0);
        }
        if (torpedosLeft == 0)
        {
            GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                GetComponent<TimerScript>().timerStart = false;
            
            // Increases score by timeLeft
            currentScore += (int) GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                GetComponent<TimerScript>().timeLeft;
            GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                GetComponent<TimerScript>().timeLeft = 0;

            GameObject.Find("Player_1").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GameObject.Find("Player_2").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }

        if (!levelInProgress)
        {
            CheckLevel();
        }

        // UI for how many torpedos are left
        torpedosLeftUI.text = "x " + torpedosLeft;

        scoreUI.text = "Score: " + currentScore;

        // UI Countdown before levels
        
        countdownUI.text = "" + countdownTime;
        if (countdownTime == 0)
        {
            countdownUI.text = "Go!";
        }
        if (countdownTime <= -1)
        {
            countdownUI.enabled = false;
            countdownTime = 3;
            StopCoroutine("RoundCountdown");

            GameObject.Find("Player_1").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GameObject.Find("Player_2").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                GetComponent<TimerScript>().timerStart = true;
        }
    }

    //Checks which level the game is currently on.
    private void CheckLevel()
    {
        if (currentLevel == 0)
        {
            transform.Find("Tutorial").GetComponent<LoadTorpedoTutorial>().taskSelected = true;
            levelInProgress = true;
        }
        else
        {
            if (currentLevel == 1)
            {
                levelInProgress = true;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timeLeft = 120;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timerReachesZeroHappened = false;
                torpedosLeft = 2;

                GameObject.FindWithTag("HatchLever").GetComponent<InteractableObject>().canInteract = true;
                GameObject.FindWithTag("TorpedoLoaderTubeLever").
                    GetComponent<InteractableObject>().canInteract = true;
                GameObject.FindWithTag("Table").GetComponent<InteractableObject>().canInteract = true;
                StartRound();
            }
            if (currentLevel == 2)
            {
                levelInProgress = true;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timeLeft = 120;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timerReachesZeroHappened = false;
                torpedosLeft = 3;
                StartRound();
            }
            if (currentLevel == 3)
            {
                levelInProgress = true;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timeLeft = 110;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timerReachesZeroHappened = false;
                torpedosLeft = 3;
                StartRound();
            }
            if (currentLevel == 4)
            {
                levelInProgress = true;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timeLeft = 100;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timerReachesZeroHappened = false;
                torpedosLeft = 3;
                StartRound();
            }
            if (currentLevel == 5)
            {
                levelInProgress = true;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timeLeft = 90;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timerReachesZeroHappened = false;
                torpedosLeft = 3;
                StartRound();
            }
            if (currentLevel == 6)
            {
                levelInProgress = true;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timeLeft = 90;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timerReachesZeroHappened = false;
                torpedosLeft = 4;
                StartRound();
            }
            if (currentLevel == 7)
            {
                levelInProgress = true;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timeLeft = 85;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timerReachesZeroHappened = false;
                torpedosLeft = 4;
                StartRound();
            }
            if (currentLevel == 8)
            {
                levelInProgress = true;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timeLeft = 80;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timerReachesZeroHappened = false;
                torpedosLeft = 4;
                StartRound();
            }
            if (currentLevel == 9)
            {
                levelInProgress = true;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timeLeft = 80;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timerReachesZeroHappened = false;
                torpedosLeft = 5;
                StartRound();
            }
            if (currentLevel == 10)
            {
                levelInProgress = true;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timeLeft = 75;
                GameObject.Find("Main Camera").transform.Find("TimerCanvas").Find("Timer").
                    GetComponent<TimerScript>().timerReachesZeroHappened = false;
                torpedosLeft = 5;
                StartRound();
            }
        }

    }

    // Update is called once per frame
    private void FixedUpdate () {
        
	}

    public IEnumerator MusicLoop()
    {

        while (true)
        {
            //music.Play();
            //yield return new WaitForSeconds(3);
            mainMusic.Play();
            yield return new WaitForSeconds(29);
        }
    }

    public IEnumerator ShouldIFlood()
    {
        while (true)
        {
            float flood = Random.value;
            if (flood <= FloodPercent)
            {
                GameObject.Find("FloodWater").GetComponent<FloodSize>().Flooding();
            }
            yield return new WaitForSeconds(WaitTimeForCall);
        }
    }

    public void Flood()
    {
        GameObject.Find("FloodWater").GetComponent<FloodSize>().Flooding();
    }

    // Coroutine for constantly adjusting gravity from bottom to top as a sin function.
    public IEnumerator AdjustGravity () {
        while(true) {
            // Each step, set gravity to be the value returned by the var angle.
            Physics2D.gravity = gravity * new Vector2(0f, Mathf.Cos(angle / (2 * Mathf.PI)));

            // Increment angle, return to zero if mod 360.
            angle += 1;
            if (angle >= 360f) {
                angle = 0f;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void checkMusic()
    {
        if (!playMusic)
        {
            PlayVictory();
            StopCoroutine(coroutine);
            StopCoroutine(AdjustGravity());
            StopCoroutine(MusicLoop());
        }
    }

    public void NoMusic()
    {
        playMusic = false;
    }

    public void PlayVictory()
    {
        if (myInt == 0)
        {
            victoryMusic.PlayDelayed(2);
            myInt += 1;
        }
    }

    // starts the game timer
    public void StartRound()
    {
        GameObject.Find("Player_1").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GameObject.Find("Player_2").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        countdownUI.enabled = true;
        StartCoroutine("RoundCountdown");
    }

    IEnumerator RoundCountdown()
    {

        while (true)
        {
            yield return new WaitForSeconds(1);
            countdownTime -= 1;
            Debug.Log(countdownTime);
        }

    }
}
