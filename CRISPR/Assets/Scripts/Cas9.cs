using UnityEngine;
using System.Collections;


[RequireComponent(typeof(DNASlot))]
public class Cas9 : MonoBehaviour {

    public SpriteRenderer indicator;
    DNASlot slot;
    Color oriColor;

    public int DNAType = 0;

    private CircleCollider2D myCollider;
    private bool canChangeColor = true;
    private SpriteRenderer sp;
    private bool canKill = true;
    private Dragable dragger;
    private TrailRenderer tr;
    private Gradient gradient;
    private float alpha;

    void Start() {
        sp = GetComponent<SpriteRenderer>();
        oriColor = GetComponent<SpriteRenderer>().color;
        slot = GetComponent<DNASlot>();
        slot.OnDNASet += OnDNaSet;
        myCollider = GetComponent<CircleCollider2D>();
        dragger = GetComponent<Dragable>();

        //Getting components to change color of the trail
        tr = GetComponent<TrailRenderer>();
        tr.material = new Material(Shader.Find("Sprites/Default"));
        alpha = 1.0f;
        gradient = new Gradient();
    }

    private void OnDNaSet(Types.DNA type) {
        indicator.color = Types.GetDNAColor(type);
    }

    public void SetType(int type)
    {
        DNAType = type;
    }

    public void AbsorbRNA(GameObject other)
    {
        if (canChangeColor)
        {
            DNAType = other.GetComponent<SpacerDNA>().ReturnDNAType();
            canChangeColor = false;
            other.GetComponent<SpacerDNA>().Apoptosis();
            Color currColor = GetComponent<SpriteRenderer>().color;
            if (DNAType != 0 && oriColor.Equals(currColor))
            {
                Color newColor = Types.GetColor(DNAType);
                StartCoroutine(changeColor(newColor));
                TurnOnTrail();
            }
        }
    }

    public bool CanIChangeColor()
    {
        return canChangeColor;
    }

    public bool StartDeath(int othersType)
    {
        if (canKill)
        {
            if (DNAType == othersType)
            {
                Debug.Log("killed");
                canKill = false;
                GameObject.Find("SFXController").GetComponent<AudioButtonController>().Play("Cut");
                StartCoroutine(Die());
                return true;
            }
            return false;
        }
        return false;
    }


    IEnumerator changeColor(Color col)
    {
        GameObject.Find("SFXController").GetComponent<AudioButtonController>().Play("Scissors");
        Color currColor = GetComponent<SpriteRenderer>().color;
        float i = 0;
        while (!col.Equals(currColor) && i < 3)
        {
            currColor.r = Mathf.Lerp(currColor.r, col.r, 0.5f);
            currColor.b = Mathf.Lerp(currColor.b, col.b, 0.5f);
            currColor.g = Mathf.Lerp(currColor.g, col.g, 0.5f);
            GetComponent<SpriteRenderer>().color = Color.Lerp(currColor, col, 0.5f * Time.deltaTime);
            currColor = GetComponent<SpriteRenderer>().color;
            Debug.Log("changed color to" + currColor.ToString());
            yield return new WaitForSeconds(0.1f);
            i += 0.1f;
        }
    }

    public void TurnOnTrail()
    {
        tr.enabled = true;
        if (DNAType == 1)
        {
            gradient.SetKeys(
                                new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) },
                                   new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
            tr.colorGradient = gradient;
        }
        else if (DNAType == 2)
        {
            gradient.SetKeys(
                                new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.blue, 1.0f) },
                                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
    );
            tr.colorGradient = gradient;
        }
        else if (DNAType == 3)
        {
            gradient.SetKeys(
                                new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 1.0f) },
                                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
    );
            tr.colorGradient = gradient;
        }
        else if (DNAType == 4)
        {
            gradient.SetKeys(
                                new GradientColorKey[] { new GradientColorKey(Color.magenta, 0.0f), new GradientColorKey(Color.magenta, 1.0f) },
                                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
    );
            tr.colorGradient = gradient;
        }
        else if (DNAType == 5)
        {
            gradient.SetKeys(
                                new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.yellow, 1.0f) },
                                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
    );
            tr.colorGradient = gradient;
        }
        else if (DNAType == 6)
        {
            gradient.SetKeys(
                                new GradientColorKey[] { new GradientColorKey(Color.cyan, 0.0f), new GradientColorKey(Color.cyan, 1.0f) },
                                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
    );
            tr.colorGradient = gradient;
        }
        else if (DNAType == 7)
        {
            gradient.SetKeys(
                                new GradientColorKey[] { new GradientColorKey(Color.black, 0.0f), new GradientColorKey(Color.black, 1.0f) },
                                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
    );
            tr.colorGradient = gradient;
        }
    }

    IEnumerator Die()
    {
        canChangeColor = false;
        this.gameObject.layer = 15;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = 15;
        }
        //Destroy(myCas.GetComponent<Dragable>());
        dragger.MakeUndraggable();
        tr.enabled = false;
        yield return new WaitForSeconds(1);
        Color casColor;
        for (int i = 0; i < 200; i++)
        {
            casColor = GetComponent<SpriteRenderer>().color;
            sp.color = new Color(casColor.r, casColor.g, casColor.b, casColor.a - 0.005f);
            yield return new WaitForSeconds(0.01f);
        }
        sp.enabled = false;
        DNAType = 0;
        yield return new WaitForSeconds(1f);
        dragger.MakeDraggable();
        sp.enabled = true;
        sp.color = new Color(oriColor.r, oriColor.g, oriColor.b, 1);
        //dragger.enabled = true;
        int xPos = Random.Range(-3, 3);
        transform.position = new Vector3(xPos, -4f, 0f);
        DNAType = 0;
        canChangeColor = true;
        canKill = true;
        yield return null;
        this.gameObject.layer = 9;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = 9;
        }
    }

    public void Death()
    {
        StartCoroutine(Die());
    }
} 
