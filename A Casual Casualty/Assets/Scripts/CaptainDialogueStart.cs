using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptainDialogueStart : MonoBehaviour
{

    public GameObject captainTextBox;
    public bool canTalk = false;
    public bool startClock = false; //start the gameTime when the dialogue starts
    public bool textBoxDisappears = false;
    public bool roundStart = false;

    public Dialogue dialogue;
    public Text nameText;
    public Text dialogueText;

    private float gameTime = 0f;
    private Queue<string> sentences;

    // Initialize the Queue
    public void Start()
    {
        sentences = new Queue<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startClock)
        {
            gameTime += Time.deltaTime;
        }
        if (canTalk)
        {
            canTalk = false;
            captainTextBox.SetActive(true);
            StartDialogue(dialogue);
        }
        //next message is displayed everytime gameTime loops
        if (gameTime > 4f)
        {
            gameTime = 0f;
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
        startClock = true;
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        startClock = false;
        if (textBoxDisappears)
        {
            captainTextBox.SetActive(false);
            textBoxDisappears = false;
        }
        if (roundStart)
        {
            roundStart = false;
            GameObject.Find("GameManager").GetComponent<GameManager>().currentLevel += 1;
            GameObject.Find("GameManager").GetComponent<GameManager>().levelInProgress = false;
        }
    }
    
    public void StopDialogue()
    {
        startClock = false;
    }
}