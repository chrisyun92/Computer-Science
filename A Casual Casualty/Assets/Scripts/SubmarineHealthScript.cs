using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmarineHealthScript : MonoBehaviour {

    int totalHealth = 4;
    int currentHealth;

    private string posHealthName = "submarine-positive-health";
    private string negHealthName = "submarine-negative-health";
    private string healthChunkPrefix = "Health";

    // Use this for initialization
    void Start () {
        currentHealth = totalHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoseHealth()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("this shouldn't happen, u have negative health now!! " + currentHealth);
            return;
        }

        // do camera shake + damage warning flash
        FlashingDamageWarningScript warning = (FlashingDamageWarningScript) GameObject.Find("DamageWarning").GetComponent("FlashingDamageWarningScript");
        CameraShakeScript shake = (CameraShakeScript) GameObject.Find("Main Camera").GetComponent("CameraShakeScript");
        warning.FlashDamageWarning();
        shake.ShakeCamera();

        // 4 - 4 = 0, 4 - 3 = 1, etc.
        int healthChunkIndex = totalHealth - currentHealth;

        // change positive health chunk to negative health
        GameObject healthChunk = GameObject.Find(healthChunkPrefix + healthChunkIndex);
        Image img = healthChunk.GetComponent<Image>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Submarine/" + negHealthName);
        img.sprite = sprites[healthChunkIndex];
        currentHealth -= 1;
    }

    public void GainHealth()
    {
        if (currentHealth >= 4)
        {
            Debug.Log("this shouldn't happen, u have excess health now!! " + currentHealth);
            return;
        }

        // 4 - 3 - 1 = 0, 4 - 2 - 1 = 1, etc
        int healthChunkIndex = totalHealth - currentHealth - 1;

        // change negative health chunk to positive health
        GameObject healthChunk = GameObject.Find(healthChunkPrefix + healthChunkIndex);
        Image img = healthChunk.GetComponent<Image>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Submarine/" + posHealthName);
        img.sprite = sprites[healthChunkIndex];
        currentHealth += 1;
    }

}
