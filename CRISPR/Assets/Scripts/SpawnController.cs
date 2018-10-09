using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {
    GameObject outBound;

    public GameObject thingToSpawn;
    public float timeToSpawn = 1.0f;
    private SpawnArgs spawnArgs;

    private float elapsedTime = 0;

    private void Start() {
        spawnArgs = GetComponent<SpawnArgs>();
    }

    void Update () {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= timeToSpawn) {
            elapsedTime = 0;
            GameObject newThing = Instantiate(thingToSpawn, transform.position, transform.rotation);
            newThing.BroadcastMessage("Spawned", spawnArgs, SendMessageOptions.DontRequireReceiver);
        }
	}
}
