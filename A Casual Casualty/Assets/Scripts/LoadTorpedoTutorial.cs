using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTorpedoTutorial : MonoBehaviour {

    public int phase = -1;
    public bool phoneAnswered;
    public bool taskSelected = false;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(phase);
        if (taskSelected)
        {
            if (phase == -1)
            {
                GameObject.FindWithTag("Phone").GetComponent<InteractableObject>().canInteract = true;
                transform.Find("CaptainDialogues").Find("Start").
                        GetComponent<CaptainDialogueStart>().canTalk = true;
                phase += 1;
            }

            if (phase == 0)
            {
                if (phoneAnswered)
                {
                    phoneAnswered = false;
                    //set up load torpedo task in the task UI;
                    transform.Find("CaptainDialogues").Find("Start").
                        GetComponent<CaptainDialogueStart>().StopDialogue();
                    transform.Find("CaptainDialogues").Find("Task1").
                        GetComponent<CaptainDialogueStart>().canTalk = true;
                    GameObject.FindWithTag("TorpedoLoaderTubeLever").
                        GetComponent<InteractableObject>().canInteract = true;
                    phase += 1;
                }
            }
            if (phase == 1)
            {
                if (GameObject.FindWithTag("TorpedoLoaderTubeLever").
                    GetComponent<InteractableObject>().canInteract == false)
                {
                    transform.Find("CaptainDialogues").Find("Task1").
                        GetComponent<CaptainDialogueStart>().StopDialogue();
                    transform.Find("CaptainDialogues").Find("Task2").
                        GetComponent<CaptainDialogueStart>().canTalk = true;
                    GameObject.FindWithTag("HatchLever").GetComponent<InteractableObject>().canInteract = true;
                    phase += 1;
                }
            }
            if (phase == 2)
            {
                if (GameObject.FindWithTag("HatchLever").
                    GetComponent<InteractableObject>().canInteract == false)
                {
                    transform.Find("CaptainDialogues").Find("Task2").
                        GetComponent<CaptainDialogueStart>().StopDialogue();
                    transform.Find("CaptainDialogues").Find("Task3").
                        GetComponent<CaptainDialogueStart>().canTalk = true;
                    GameObject.FindWithTag("Table").GetComponent<InteractableObject>().canInteract = true;
                    phase += 1;
                }
            }
            if (phase == 3)
            {
                if (GameObject.FindWithTag("Table").
                    GetComponent<InteractableObject>().tableNextToTorpedoRack == true)
                {
                    transform.Find("CaptainDialogues").Find("Task3").
                        GetComponent<CaptainDialogueStart>().StopDialogue();
                    transform.Find("CaptainDialogues").Find("Task4").
                        GetComponent<CaptainDialogueStart>().canTalk = true;
                    phase += 1;
                }
            }
            if (phase == 4)
            {
                if (GameObject.Find("Table").GetComponent<Table>().isLoaded)
                {
                    transform.Find("CaptainDialogues").Find("Task4").
                        GetComponent<CaptainDialogueStart>().StopDialogue();
                    transform.Find("CaptainDialogues").Find("Task5").
                        GetComponent<CaptainDialogueStart>().canTalk = true;
                    phase += 1;
                }
            }

            if (phase == 5)
            {
                if (!GameObject.Find("Table").GetComponent<Table>().isLoaded)
                {
                    transform.Find("CaptainDialogues").Find("Task5").
                        GetComponent<CaptainDialogueStart>().StopDialogue();
                    transform.Find("CaptainDialogues").Find("Task6").
                        GetComponent<CaptainDialogueStart>().canTalk = true;
                    GameObject.FindWithTag("HatchLever").GetComponent<InteractableObject>().canInteract = true;
                    phase += 1;
                }
            }

            if (phase == 6)
            {
                if (!GameObject.FindWithTag("HatchLever").GetComponent<InteractableObject>().canInteract)
                {
                    transform.Find("CaptainDialogues").Find("Task6").
                        GetComponent<CaptainDialogueStart>().StopDialogue();
                    transform.Find("CaptainDialogues").Find("Task7").
                        GetComponent<CaptainDialogueStart>().canTalk = true;
                    GameObject.FindWithTag("TorpedoLoaderTubeLever").
                        GetComponent<InteractableObject>().canInteract = true;
                    phase += 1;
                }
            }

            if (phase == 7)
            {
                if (!GameObject.FindWithTag("TorpedoLoaderTubeLever").
                    GetComponent<InteractableObject>().canInteract)
                {
                    transform.Find("CaptainDialogues").Find("Task7").
                        GetComponent<CaptainDialogueStart>().StopDialogue();
                    transform.Find("CaptainDialogues").Find("Task8").
                        GetComponent<CaptainDialogueStart>().canTalk = true;
                    phase += 1;
                }
            }
            if (phase == 8)
            {
                if (!GameObject.FindWithTag("PressurizedTubeSlider").GetComponent<Hatch>().isTorpedoInHatch)
                {
                    transform.Find("CaptainDialogues").Find("Task8").
                        GetComponent<CaptainDialogueStart>().StopDialogue();
                    transform.Find("CaptainDialogues").Find("Finished").
                        GetComponent<CaptainDialogueStart>().textBoxDisappears = true;

                    // Changes from tutorial to Level 1 and frees all canInteracts in interactableObject
                    transform.Find("CaptainDialogues").Find("Finished").
                        GetComponent<CaptainDialogueStart>().roundStart = true;
                    transform.Find("CaptainDialogues").Find("Finished").
                        GetComponent<CaptainDialogueStart>().canTalk = true;
                    phase += 1;
                }
            }
        }
    }
}
