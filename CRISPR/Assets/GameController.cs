using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    [SerializeField]
    private Text score;

    [SerializeField]
    private Camera myCamera;
    [SerializeField]
    private GameObject myButton;
    private float sizeLerpFactor = 0f;
    private float yLerpFactor = 0f;
    private bool CameraAdjusted = false;
    [SerializeField]
    private int stepInTutorial = 0;
    [SerializeField]
    private bool MoveOn = false;
    [SerializeField]
    private GameObject spawner;
    [SerializeField]
    private GameObject spawnerOne;
    [SerializeField]
    private GameObject spawnerTwo;
    [SerializeField]
    private GameObject[] cas9killers = new GameObject[3];

    // Update is called once per frame
    void Update()
    {
        if (!CameraAdjusted)
        {
            AdjustCamera();
        } else
        {
            if (stepInTutorial == 100)
            {
                return;
            }
            else if (stepInTutorial == 0)
            {
                Time.timeScale = 0;
                score.text = "Welcome to CRISPR Defense!";
                myButton.gameObject.SetActive(true);
                stepInTutorial += 1;
            }
            else if (stepInTutorial == 1 && MoveOn)
            {
                score.text = "You will defend your bacterial cell from virus invaders.";
                stepInTutorial += 1;
                MoveOn = false;
            }
            else if (stepInTutorial == 2 && MoveOn)
            {
                score.text = "There are three steps to success. The first is adaptation.";
                stepInTutorial += 1;
                MoveOn = false;
            }
            else if (stepInTutorial == 3 && MoveOn)
            {
                score.text = "The green Cas1-Cas2 complex picks up new viral DNAs, and takes them to the CRISPR array, a virus memory bank.";
                stepInTutorial += 1;
                MoveOn = false;
                
            }
            else if (stepInTutorial == 4 && MoveOn)
            {
                Time.timeScale = 1;
                myButton.SetActive(false);
                score.text = "Pick up the viral DNA by dragging the Cas1-Cas2 to it, and take it to a DNA slot below.";
                stepInTutorial += 1;
                MoveOn = false;
                spawner.GetComponent<Spawn>().SpawnVirus(1);
            }
            else if (stepInTutorial == 5 && MoveOn)
            {
                myButton.SetActive(true);
                score.text = "Great! Good work!";
                stepInTutorial += 1;
                MoveOn = false;
            }
            else if (stepInTutorial == 6 && MoveOn)
            {
                score.text = "Second step is expression. Your bacterium makes guide-RNAs and loads them into Cas9, a precise DNA-cutting protein.";
                stepInTutorial += 1;
                MoveOn = false;
            }
            else if (stepInTutorial == 7 && MoveOn)
            {
                myButton.SetActive(false);
                score.text = "Swipe up on the CRISPR array segment holding the desired gRNA.";
                stepInTutorial += 1;
                MoveOn = false;
            }
            else if (stepInTutorial == 8 && MoveOn)
            {
                score.text = "Move the pink-blue Cas9 proteins to pick up the gRNA.";
                stepInTutorial += 1;
                MoveOn = false;
            }
            else if (stepInTutorial == 9 && MoveOn)
            {
                spawner.GetComponent<Spawn>().SpawnVirus(1);
                score.text = "Nice work! Last step is interference. Fire the loaded protein at the invading viral DNA.";
                stepInTutorial += 1;
                MoveOn = false;
            }
            else if (stepInTutorial == 10 && MoveOn)
            {
                myButton.SetActive(true);
                score.text = "Cas9 cuts apart any DNA that matches its gRNA.";
                stepInTutorial += 1;
                MoveOn = false;
            }
            else if (stepInTutorial == 11 && MoveOn)
            {
                score.text = "If your Cas9 is holding an undesired gRNA, swipe it off the screen to respawn.";
                stepInTutorial += 1;
                MoveOn = false;
            }
            else if (stepInTutorial == 12 && MoveOn)
            {
                spawnerOne.GetComponent<Spawn>().SpawnVirus(2);
                spawnerTwo.GetComponent<Spawn>().SpawnVirus(3);
                score.text = "There are many viruses with different DNA. You must adapt to destroy all of them.";
                stepInTutorial += 1;
                MoveOn = false;
            }
            
            else if (stepInTutorial == 13 && MoveOn)
            {
                score.text = "Don’t wait too long. The viral DNA will replicate more viruses and burst your cell! Game over.";
                stepInTutorial += 1;
                MoveOn = false;
            }
            else if (stepInTutorial == 14 && MoveOn)
            {
                foreach (GameObject cas9killer in cas9killers)
                {
                    cas9killer.SetActive(true);
                }
                score.text = "Watch out! Some viruses make anti-CRISPR proteins that disable your loaded Cas9.";
                stepInTutorial += 1;
                MoveOn = false;
            }
            else if (stepInTutorial == 15 && MoveOn)
            {
                myButton.SetActive(false);
                score.text = "Destroy as many viruses as you can for the high score. Good luck!";
                stepInTutorial = 100;
                MoveOn = false;
                StartCoroutine(LoadMenu());
            }
        }
    }

    private void AdjustCamera()
    {
        if (myCamera.orthographicSize != 7.66f)
        {
            myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, 7.66f, sizeLerpFactor);
            sizeLerpFactor += 0.05f * Time.deltaTime;
        }
        if (myCamera.transform.position.y != 0)
        {
            myCamera.transform.position = new Vector3(0, Mathf.Lerp(myCamera.transform.position.y, 0, yLerpFactor), -10);
            yLerpFactor += 0.05f * Time.deltaTime;
        }
        if (myCamera.orthographicSize < 8.0f)
        {
            CameraAdjusted = true;
        }
    }

    public void InitiateNextStep()
    {
        MoveOn = true;
    }

    private IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("Menu");
        SceneManager.UnloadSceneAsync("Tutorial");
    }
}
