using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningController : MonoBehaviour {

    Transform dest;
    Camera camera;
    SpriteRenderer background;
    float lerpFactor = 0f;
    float sizeLerpFactor = 0f;
    float transLerpFactor = 0f;
    float finalCamSize = 0.05f;
    float totalZoomTime = 6f;
    float totalTransitionTime = 2f;
    float zoomIncrementation = 0.01f;
    float lerpIncrementation = 0.00005f;
    SpriteRenderer blackBack;

	// Use this for initialization
	void Start () {
        dest = GameObject.Find("Camera Destination").GetComponent<Transform>(); 
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        background = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        blackBack = GameObject.Find("Black").GetComponent<SpriteRenderer>();
        StartCoroutine("Zoom");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Zoom()
    {
        for (float i = 0; i < totalZoomTime; i += zoomIncrementation)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, finalCamSize, lerpFactor);
            lerpFactor += lerpIncrementation;
            camera.transform.position = new Vector3(Mathf.Lerp(camera.transform.position.x, dest.transform.position.x, lerpFactor), Mathf.Lerp(camera.transform.position.y, dest.transform.position.y, transLerpFactor), -10);
            yield return new WaitForSeconds(zoomIncrementation);
        }
        lerpFactor = 0;
        StartCoroutine("Transition");
    }

    IEnumerator Transition()
    {
        Debug.Log("started transition");
        Color backCol;
        float backAlpha;
        float backAlphaLerp;
        for (float i = 0; i < totalTransitionTime; i += zoomIncrementation)
        {
            backCol = blackBack.color;
            backAlpha = Mathf.Lerp(backCol.a, 255, lerpFactor);
            blackBack.color = Color.Lerp(backCol, new Color(backCol.r, backCol.g, backCol.b, backAlpha), lerpFactor);
            //backCol.r = Mathf.Lerp(backCol.r, 0, lerpFactor);
            //backCol.g = Mathf.Lerp(backCol.g, 0, lerpFactor);
            //backCol.b = Mathf.Lerp(backCol.b, 0, lerpFactor);
            //background.color = Color.Lerp(backCol, new Color(0, 0, 0), lerpFactor);

            lerpFactor += lerpIncrementation;
            yield return new WaitForSeconds(zoomIncrementation);
        }
        Game.current = new global::Game();
        Debug.Log("Created new game object");
        Debug.Log(Game.current.openedBefore);
        saveData.Save();
        Debug.Log("Saved data");
        SceneManager.LoadScene("Tutorial");
        SceneManager.UnloadSceneAsync("Opening");
    }

    public bool OpenedBefore()
    {
        Debug.Log("returned true");
        return true;
    }
}