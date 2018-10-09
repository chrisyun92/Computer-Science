using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public int TimeBetweenSpawns = 5;


    //All public variables below should be private, but for checking it is public for now    
    public int numSpawns;
    public Transform[] spawns;
    public int numTrans = 0;
    public int chosenSpawner;
    public bool amISpawnManager;
    public int NumViruses;
    public GameObject spacerHolder;
    public GameObject cas93;
    public GameObject cellScene;
    public GameObject particleSystemHolder;

    private int top;
    private int type;
    private Transform cellSceneTransform;
    [SerializeField]
    private GameObject[] cas9killers = new GameObject[4];

    // Use this for initialization
    void Start()
    {
        cellSceneTransform = cellScene.transform;
        spawns = new Transform[17];
        foreach (Transform child in transform)
        {
            spawns[numTrans] = child;
            spawns[numTrans].GetComponent<Spawn>().SetTransformforVirus(cellSceneTransform);
            numTrans += 1;
        }
        if (amISpawnManager)
        {
            StartCoroutine(Spawn());
        }
    }

    //Updates EVERYTHING. The number of viruses, spacers, etc
    void Update()
    {
        if (numSpawns == 2)
        {
            NumVirusesWanted(3);
            TimeBetweenSpawns = 8;
            spacerHolder.transform.Find("dnaFour").gameObject.SetActive(false);
            spacerHolder.transform.Find("spacerFour").gameObject.SetActive(true);
        } else if (numSpawns == 10)
        {
            NumVirusesWanted(5);
            TimeBetweenSpawns = 6;
            spacerHolder.transform.Find("dnaThree").gameObject.SetActive(false);
            spacerHolder.transform.Find("spacerThree").gameObject.SetActive(true);
            cas9killers[0].SetActive(true);
            cas9killers[0].GetComponent<Cas9Killer>().StartFadeIn();
        } else if (numSpawns == 16)
        {
            spacerHolder.transform.Find("dnaTwo").gameObject.SetActive(false);
            spacerHolder.transform.Find("spacerTwo").gameObject.SetActive(true);
            NumVirusesWanted(7);
            cas93.SetActive(true);
        } else if (numSpawns == 25)
        {
            spacerHolder.transform.Find("dnaOne").gameObject.SetActive(false);
            spacerHolder.transform.Find("spacerOne").gameObject.SetActive(true);
            cas9killers[1].SetActive(true);
            cas9killers[1].GetComponent<Cas9Killer>().StartFadeIn();
            TimeBetweenSpawns = 5;
        } else if (numSpawns == 35)
        {
            cas9killers[2].SetActive(true);
            cas9killers[2].GetComponent<Cas9Killer>().StartFadeIn();
            TimeBetweenSpawns = 4;
        } else if (numSpawns == 48)
        {
            cas9killers[3].SetActive(true);
            cas9killers[3].GetComponent<Cas9Killer>().StartFadeIn();
            TimeBetweenSpawns = 2;
        }
    }

    public void EndGame() {
        StartCoroutine(GameOver());
    }

    public void NumVirusesWanted(int newNumber)
    {
        NumViruses = newNumber;
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            top = 7 - (6 - NumViruses);
            type = Random.Range(1, top);
            chosenSpawner = Random.Range(0, numTrans);
            spawns[chosenSpawner].GetComponent<Spawn>().SpawnVirus(type);
            numSpawns += 1;
            yield return new WaitForSeconds(TimeBetweenSpawns);
        }
    }

    public Transform ReturnCellTransform()
    {
        return cellSceneTransform;
    }

    IEnumerator GameOver()
    {
        foreach(Transform child in particleSystemHolder.transform)
        {
            child.gameObject.SetActive(true);
        }
        yield return null;
    }


}
