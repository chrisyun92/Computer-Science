using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cas9Killer : MonoBehaviour {

    public float TimeTillDeath;
    public float minTimetoRespawn;
    public float maxTimetoRespawn;

    private BoxCollider2D bc;
    private SpriteRenderer sp;
    private bool CantSpawn = false;
    private Color ogColor;

    // Use this for initialization
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();
        ogColor = sp.color;
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator RandomSpawn()
    {
        yield return new WaitForSeconds(Random.Range(minTimetoRespawn, maxTimetoRespawn));
        StartCoroutine(FadeIn());
    }

    public IEnumerator TimedDeath()
    {
        yield return new WaitForSeconds(TimeTillDeath);
        StartCoroutine(FadeOut());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("cas9"))
        {
            if (!col.gameObject.GetComponent<Cas9>().CanIChangeColor())
            {
                GameObject.Find("SFXController").GetComponent<AudioButtonController>().Play("Slip");
                col.gameObject.GetComponent<Cas9>().Death();
                StartCoroutine(FadeOut());
            }
        }
    }

    public IEnumerator FadeOut()
    {
        bc.enabled = false;
        Color casColor = Color.black;
        for (int i = 0; i < 100; i++)
        {
            casColor = GetComponent<SpriteRenderer>().color;
            sp.color = new Color(casColor.r, casColor.g, casColor.b, casColor.a - 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
        sp.enabled = false;
        CantSpawn = false;
        StartCoroutine(RandomSpawn());
    }

    public IEnumerator FadeIn()
    {
        sp.enabled = true;
        Color casColor = Color.black;
        for (int i = 0; i < 200; i++)
        {
            casColor = GetComponent<SpriteRenderer>().color;
            sp.color = new Color(casColor.r, casColor.g, casColor.b, casColor.a + 0.005f);
            yield return new WaitForSeconds(0.01f);
        }
        bc.enabled = true;
        CantSpawn = true;
    }
}
