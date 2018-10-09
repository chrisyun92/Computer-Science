using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEverythingScript : MonoBehaviour {

    private ArrayList items;
    private PauseController pauseController;

    private void Start()
    {
        pauseController = GameObject.Find("ScenePauseController").GetComponent<PauseController>();
    }

    public void KillAll()
    {
        Destroy(GameObject.Find("CellScene"));
        GameObject.Find("SpawnHolder").SetActive(false);
        GameObject.Find("DeathSpawner").GetComponent<SpawnManager>().EndGame();
        pauseController.gameOver();
    }
}
