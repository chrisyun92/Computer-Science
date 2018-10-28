using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingDamageWarningScript : MonoBehaviour {

    public Image overlay;
    public float maxAlpha = 100.0f / 255.0f;
    public float minAlpha = 0.0f;
    public float halfDuration;
    public int numFlashes;

    private float currAlpha;
    //private Color currentColor;

	// Use this for initialization
	void Start () {
        overlay.color = new Color(255, 0, 0, minAlpha);
        //FlashDamageWarning();
    }

    // Update is called once per frame
    void Update () {
	}

    // flashes the damage warning overlay from minAlpha to maxAlpha using sine interpolation
    //   for <duration> amount of time
    public void FlashDamageWarning()
    {
        StartCoroutine(Flash(halfDuration, numFlashes));
    }

    IEnumerator Flash(float halfDuration, int numFlashes)
    {
        int count = 0;
        while (count < numFlashes)
        {
            float elapsedTime = 0f;
            while (elapsedTime < halfDuration)
            {
                currAlpha = Mathf.SmoothStep(minAlpha, maxAlpha, elapsedTime / halfDuration);
                overlay.color = new Color(255, 0, 0, currAlpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0f;
            while (elapsedTime < halfDuration)
            {
                currAlpha = Mathf.SmoothStep(maxAlpha, minAlpha, elapsedTime / halfDuration);
                overlay.color = new Color(255, 0, 0, currAlpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            count++;
        }
    }
}
