using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour {

    public Sprite unloadedSprite;
    public Sprite loadedSprite;
    public GameObject torpedo;
    public float timeToLoad;
    public float launchDistance = 2;
    public Vector3 tubePos;
    public bool isLoaded = false;
    public AudioSource missileLaunch;

    private Vector3 originalPos;
    private Transform torpedoParent;
    private Vector3 loadingPosition = new Vector2(0, 2.075f);

    // Loads a torpedo from the rack onto the table.
    public void RackToTable() {
        // When activated, snap the table into position.
        gameObject.GetComponent<Transform>().position = loadingPosition;
        isLoaded = true;
        LoadSprite();
    }

    // Unloads a torpedo from table into the tube.
    public void TableToTube() {
        isLoaded = false;
        LoadSprite();
        torpedo.GetComponent<Rigidbody2D>().velocity = new Vector2(-5, 0);
    }

    // Activates or decativates the torpedo sprite that's on top of the table.
    public void LoadSprite() {
        if (!isLoaded) {
            torpedo.GetComponent<SpriteRenderer>().enabled = true;
            Debug.Log("Torpedo Loaded!");
        } else {
            StartCoroutine(LoadTorpedo());
            Debug.Log("Torpedo Unloaded!");
        }
    }

    // launch and hide the torpedo
    public void DeactivateSprite()
    {
        StartCoroutine(ResetTorpedo());
    }

    IEnumerator LoadTorpedo()
    {
        Debug.Log("loading the torpedo");
        // store torpedo's position relative to the table
        originalPos = torpedo.transform.localPosition;
        torpedoParent = torpedo.transform.parent;
        torpedo.transform.parent = null;
        // load the torpedo into the tube
        float elapsedTime = 0f;
        while (elapsedTime < timeToLoad)
        {
            elapsedTime += Time.deltaTime;
            torpedo.transform.position = Vector3.Lerp(originalPos + torpedoParent.position, tubePos, elapsedTime / timeToLoad);
            yield return null;
        }
    }

    IEnumerator ResetTorpedo()
    {
        Debug.Log("resetting the torpedo");
        missileLaunch.Play();

        // launch the torpedo
        float elapsedTime = 0f;
        while (elapsedTime < timeToLoad)
        {
            elapsedTime += Time.deltaTime;
            torpedo.transform.position += new Vector3(-launchDistance * elapsedTime / timeToLoad, 0, 0);
            Debug.Log(torpedo.transform.position);
            yield return null;
        }

        // reset torpedo to be on top of the table again
        torpedo.GetComponent<SpriteRenderer>().enabled = false;
        torpedo.transform.parent = torpedoParent;
        torpedo.transform.position = torpedo.transform.parent.position + originalPos;
    }
}
