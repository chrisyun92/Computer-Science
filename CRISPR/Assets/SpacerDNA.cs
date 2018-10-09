using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacerDNA : MonoBehaviour {

    private BoxCollider2D bc;
    private SpriteRenderer sp;
    private int type = 0;
    private Spacer father;
    private int positionInArray;
    private bool canBePickedUp = false;
    private bool advanceTutorial = false;
    private bool hasAdvanced = false;
    private SpriteRenderer[] rnaBits;

    void Awake()
    {
        rnaBits = GetComponentsInChildren<SpriteRenderer>();
        //rnaBits[0] = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        //rnaBits[1] = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();
        StartCoroutine(TimeTillDeath());
    }

    IEnumerator TimeTillDeath()
    {
        yield return new WaitForSeconds(6);
        Apoptosis();
    }


    public void StartFade()
    {
        StartCoroutine(Fade());
    }

    public IEnumerator Fade()
    {
        Color casColor;
        for (int i = 0; i < 200; i++)
        {
            casColor = GetComponent<SpriteRenderer>().color;
            sp.color = new Color(casColor.r, casColor.g, casColor.b, casColor.a - 0.005f);

            foreach (SpriteRenderer rnaBit in rnaBits)
            {
                rnaBit.color = sp.color;
            }
            yield return new WaitForSeconds(0.01f);
        }
        sp.enabled = false;
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("cas9"))
        {
            if (canBePickedUp)
            {
                if (col.gameObject.GetComponent<Cas9>().CanIChangeColor()) {
                    bc.enabled = false;
                    if (advanceTutorial)
                    {
                        FindObjectOfType<GameController>().InitiateNextStep();
                        advanceTutorial = false;
                    }
                    col.gameObject.GetComponent<Cas9>().AbsorbRNA(this.gameObject);
                }
            }
        }
    }

    public void SetType(int newType)
    {
        type = newType;
        ChangeColor();
    }

    public void SetPickedToTrue()
    {
        canBePickedUp = true;
    }

    public int ReturnDNAType()
    {
        return type;
    }

    public void Apoptosis()
    {
        father.SetSpacerFather(true);
        father.SubtractCount();
        Destroy(gameObject);
    }

    public void SetPositionInArray(int pos)
    {
        positionInArray = pos;
    }

    public void SetSpacer(Spacer newFather)
    {
        father = newFather;
    }

    public void ChangeColor()
    {
        if (type == 1)
        {
            sp.color = Color.red;
        }
        else if (type == 2)
        {
            sp.color = Color.blue;
        }
        else if (type == 3)
        {
            sp.color = Color.green;
        }
        else if (type == 4)
        {
            sp.color = Color.magenta;
        }
        else if (type == 5)
        {
            sp.color = Color.yellow;
        }
        else if (type == 6)
        {
            sp.color = Color.cyan;
        }
        else if (type == 7)
        {
            sp.color = Color.black;
        }
    }

    public void AdvanceTutorial()
    {
        if (!hasAdvanced)
        {
            advanceTutorial = true;
        }
    }

    public void HasAdvanced()
    {
        hasAdvanced = true;
    }
}
