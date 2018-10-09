using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cas1Cas2 : MonoBehaviour {

    public bool grabbingSomething = false;
    public Color fadedColor;
    public int deadVirusLayer;
    public int dnaType;

    [SerializeField]
    private bool isTutorial = false;
    private int currentType = 0;
    private int[] dnaTransported = new int[8];
    private bool markedForDeath = false;
    private Transform myCas;
    private SpriteRenderer sr;
    private Material material;
    private Color oriColor;
    // Use this for initialization
    void Start () {
        myCas = transform.Find("Cas1Cas2");
        sr = myCas.GetComponent<SpriteRenderer>();
        oriColor = sr.color;
        material = sr.material;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GrabOn()
    {
        grabbingSomething = true;
    }

    public bool IsGrabbing()
    {
        return grabbingSomething;
    }

    public void SetDNA(int type)
    {
        dnaType = type;
        Color currColor = myCas.GetComponent<SpriteRenderer>().color;
        if (oriColor.Equals(currColor))
        {
            Color newColor = Types.GetColor(dnaType);
            StartCoroutine(changeColor(newColor));
        }
    }

    public int ReturnDNA()
    {
        return dnaType;
    }

    public void MarkForDeath()
    {
        markedForDeath = true;
    }

    public bool IsMarked()
    {
        return markedForDeath;
    }

    public void Death()
    {
        grabbingSomething = false;
        StartCoroutine(Die());
    }

    public void AddTypeTransported(int virus)
    {
        currentType = virus;
        dnaTransported[virus] = virus;
    }

    public void RemoveTypeTransported(int virus)
    {
        currentType = 0;
        dnaTransported[virus] = 0;
    }

    IEnumerator changeColor(Color col)
    {
        GameObject.Find("SFXController").GetComponent<AudioButtonController>().Play("LowClick");
        Debug.Log("hit color");
        Debug.Log(col);
        Color currColor = myCas.GetComponent<SpriteRenderer>().color;
        float i = 0;
        while (!col.Equals(currColor) && i < 3)
        {
            currColor.r = Mathf.Lerp(currColor.r, col.r, 0.5f);
            currColor.b = Mathf.Lerp(currColor.b, col.b, 0.5f);
            currColor.g = Mathf.Lerp(currColor.g, col.g, 0.5f);
            myCas.GetComponent<SpriteRenderer>().color = Color.Lerp(currColor, col, 0.5f * Time.deltaTime);
            currColor = myCas.GetComponent<SpriteRenderer>().color;
            yield return new WaitForSeconds(0.1f);
            i += 0.1f;
        }
    }

    IEnumerator Die()
    {
        //Destroy(myCas.GetComponent<Dragable>());
        //SwitchCollisions(true);
        GameObject.Find("ScoreController").GetComponent<ScoreController>().updateScore(10);
        this.gameObject.layer = 15;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = 15;
        }
        float time = 0.0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            float percent = Mathf.Clamp01(time / 1);
            if (material.HasProperty("_EffectAmount"))
            {
                float current = material.GetFloat("_EffectAmount");
                material.SetFloat("_EffectAmount", Mathf.Lerp(0, 1, percent));
                material.color = Color.Lerp(Color.white, fadedColor, percent);
            }
        }
        grabbingSomething = false;
        myCas.GetComponent<Dragable>().MakeUndraggable();
        yield return new WaitForSeconds(1);
        myCas.GetComponent<BoxCollider2D>().enabled = false;
        dnaType = 0;
        Color casColor;
        for (int i = 0; i < 200; i++)
        {
            casColor = myCas.GetComponent<SpriteRenderer>().color;
            myCas.GetComponent<SpriteRenderer>().color = new Color(casColor.r, casColor.g, casColor.b, casColor.a - 0.005f);
            yield return new WaitForSeconds(0.01f);
        }
        myCas.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        markedForDeath = false;
        myCas.position = new Vector3(0f, 0f, 0f);
        transform.position = new Vector3(-2f, -7f, 0f);
        //SwitchCollisions(false);
        myCas.GetComponent<Dragable>().MakeDraggable();
        myCas.GetComponent<SpriteRenderer>().enabled = true;
        casColor = myCas.GetComponent<SpriteRenderer>().color;
        myCas.GetComponent<SpriteRenderer>().color = new Color(oriColor.r, oriColor.g, oriColor.b, 1);
        //myCas.GetComponent<Dragable>().enabled = true;
        myCas.GetComponent<BoxCollider2D>().enabled = true;
        this.gameObject.layer = 18;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = 18;
        }
        yield return null;
    }

    IEnumerator SpecialDie()
    {
        //Destroy(myCas.GetComponent<Dragable>());
        //SwitchCollisions(true);

        float time = 0.0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            float percent = Mathf.Clamp01(time / 1);
            if (material.HasProperty("_EffectAmount"))
            {
                float current = material.GetFloat("_EffectAmount");
                material.SetFloat("_EffectAmount", Mathf.Lerp(0, 1, percent));
                material.color = Color.Lerp(Color.white, fadedColor, percent);
            }
        }
        grabbingSomething = false;
        myCas.GetComponent<Dragable>().MakeUndraggable();
        yield return new WaitForSeconds(1);
        myCas.GetComponent<BoxCollider2D>().enabled = false;
        dnaType = 0;
        Color casColor;
        for (int i = 0; i < 200; i++)
        {
            casColor = myCas.GetComponent<SpriteRenderer>().color;
            myCas.GetComponent<SpriteRenderer>().color = new Color(casColor.r, casColor.g, casColor.b, casColor.a - 0.005f);
            yield return new WaitForSeconds(0.01f);
        }
        myCas.GetComponent<SpriteRenderer>().enabled = false;
        if (currentType != 0)
        {
            RemoveTypeTransported(currentType);
        }
        yield return new WaitForSeconds(2);
        markedForDeath = false;
        //SwitchCollisions(false);
        myCas.position = new Vector3(0f, 0f, 0f);
        transform.position = new Vector3(-2f, -7f, 0f);
        myCas.GetComponent<Dragable>().MakeDraggable();
        myCas.GetComponent<SpriteRenderer>().enabled = true;
        casColor = myCas.GetComponent<SpriteRenderer>().color;
        myCas.GetComponent<SpriteRenderer>().color = new Color(oriColor.r, oriColor.g, oriColor.b, 1);
        //myCas.GetComponent<Dragable>().enabled = true;
        myCas.GetComponent<BoxCollider2D>().enabled = true;
        yield return null;
    }

    public void callSpecialDie()
    {
        StartCoroutine(SpecialDie());
    }

    public bool HasTransported(int transport)
    {
        foreach (int num in dnaTransported)
        {
            if (num == transport)
            {
                return true;
            }
        }
        return false;
    }

    void SwitchCollisions(bool ignore)
    {
        Physics.IgnoreLayerCollision(9, 9, ignore);
        Physics.IgnoreLayerCollision(9, 12, ignore);
    }

    public bool isItTutorial()
    {
        return isTutorial;
    }
}
