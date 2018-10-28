using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodSize : MonoBehaviour {

    public bool isFlooding;
    public bool playerMopping;
    public float mopSpeed;
    public float floodSpeed;
    public float maxSize;
    private Vector3 killFlood;
    public float killFloodAtThisSize;
    private int level;
    private bool level0;
    private GameObject[] playerHolder = new GameObject[2];
    private float startX;
    private float startY;

	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        level = 0;
        startX = transform.localScale.x;
        startY = transform.localScale.y;
        isFlooding = false;
        playerMopping = false;
        killFlood = new Vector3(killFloodAtThisSize, killFloodAtThisSize, 0);
    }

    // Update is called once per frame
    void Update () {
        Detect();
		if (isFlooding)
        {
            IncreaseSize();
        }
        if (playerMopping)
        {
            DecreaseSize();
        }
        checkAndKill();
	}

    public void Flooding()
    {
        isFlooding = true;
    }

    public void IncreaseSize()
    {
        if (transform.localScale.x < maxSize || transform.localScale.y < maxSize)
        {
            transform.localScale += new Vector3(floodSpeed, floodSpeed, 0);
        }
    }

    public void DecreaseSize()
    {
        transform.localScale -= new Vector3(mopSpeed, mopSpeed, 0);
    }

    private void Detect()
    {
        if (playerHolder[0] == null && playerHolder[1] == null)
        {
            playerMopping = false;
        }
        else
        {
            if (playerHolder[0] != null && playerHolder[1] == null)
            {
                if (playerHolder[0].gameObject.GetComponent<PlayerScript>().HasMop())
                {
                    playerMopping = true;
                }
                else
                {
                    playerMopping = false;
                }
            }
            else if (playerHolder[1] != null && playerHolder[0] == null)
            {
                if (playerHolder[1].gameObject.GetComponent<PlayerScript>().HasMop())
                {
                    playerMopping = true;
                }
                else
                {
                    playerMopping = false;
                }
            } else
            {
                if (playerHolder[0].gameObject.GetComponent<PlayerScript>().HasMop() || 
                    playerHolder[1].gameObject.GetComponent<PlayerScript>().HasMop())
                {
                    playerMopping = true;
                } else
                {
                    playerMopping = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1")
        {
            playerHolder[0] = other.gameObject;
            other.gameObject.GetComponent<PlayerScript>().ReduceSpeed();
            Detect();
        } else if (other.gameObject.tag == "Player2")
        {
            playerHolder[1] = other.gameObject;
            other.gameObject.GetComponent<PlayerScript>().ReduceSpeed();
            Detect();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player1")
        {
            playerHolder[0].gameObject.GetComponent<PlayerScript>().RestoreSpeed();
            playerHolder[0] = null;
        }
        else if (collision.gameObject.tag == "Player2")
        {
            playerHolder[1].gameObject.GetComponent<PlayerScript>().RestoreSpeed();
            playerHolder[1] = null;
        }
    }

    private void checkAndKill()
    {
        if ((transform.localScale.x <= killFloodAtThisSize || transform.localScale.y <= killFloodAtThisSize) && playerMopping)
        {
            isFlooding = false;
            playerMopping = false;
            GameObject.FindGameObjectWithTag("Player1").gameObject.GetComponent<PlayerScript>().RestoreSpeed();
            GameObject.FindGameObjectWithTag("Player2").gameObject.GetComponent<PlayerScript>().RestoreSpeed();
            Restart();
        }
    }

    private void Restart()
    {
        playerHolder = new GameObject[2];
        transform.localScale = new Vector3(startX, startY, 0);
        gameObject.SetActive(true);
    }
}
