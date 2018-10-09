using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DNASlot))]
public class Spacer : MonoBehaviour {

    public GameObject DNA;

    SpriteRenderer sr;
    DNASlot slot;

    public int dnaType = 0;

    private int count = 0;
    private GameObject[] myDNAs = new GameObject[3];
    private Cas1Cas2 script;
    private SpacerHolder holder;
    private int heldDNA = 0;
    private SpacerDNA DNAScript;
    private bool caller = false;
    private bool step1 = true;
    private bool step2 = false;
    private bool step3 = false;

    private void OnEnable()
    {
        slot.OnDNASet += OnDnaSet;
    }

    private void OnDisable()
    {
        slot.OnDNASet -= OnDnaSet;
    } 

    private void Awake() {
        DNAScript = DNA.GetComponent<SpacerDNA>();
        sr = GetComponent<SpriteRenderer>();
        slot = GetComponent<DNASlot>();
        holder = GetComponentInParent<SpacerHolder>();
    }

    private void OnDnaSet(Types.DNA type) {
        //print(type);
        //sr.color = Types.GetDNAColor(type);
        //print(Types.GetDNAColor(type));
        //sr.color = Types.GetDNAColor(type);
    }

    //Tell the world i am calling the spacer holder so i am the only one with an active dna strand to pick up
    public void SetAsActiveCaller(bool active)
    {
        caller = active;
    }

    public bool AmActiveCaller()
    {
        return caller;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject other = collision.gameObject;
        if (other.CompareTag("cas1cas2")) {
            script = other.gameObject.GetComponentInParent<Cas1Cas2>();
            Debug.Log(script.ReturnDNA());
            if (script.IsMarked())
            {
                return;
            } else if (script.IsGrabbing())
            {
                if (holder.InVirusTypes(script.ReturnDNA()))
                {
                    return;
                }
                else
                {
                    GameObject.Find("SFXController").GetComponent<AudioButtonController>().Play("HighClick");
                    if (script.isItTutorial())
                    {
                        FindObjectOfType<GameController>().InitiateNextStep();
                    }
                    Types.DNA casType = Types.GetDNA(script.ReturnDNA());
                    heldDNA = script.ReturnDNA();
                    Debug.Log(casType);
                    slot.DNAType(casType);
                    if (dnaType != 0)
                    {
                        holder.RemoveVirusType(dnaType);
                    }
                    holder.AddVirusType(script.ReturnDNA());
                    dnaType = script.ReturnDNA();
                    Debug.Log(Types.GetColor(script.ReturnDNA()));
                    sr.color = Types.GetColor(script.ReturnDNA());
                    script.MarkForDeath();
                    script.Death();
                    
                }
            }
        }
    }

    public int ReturnDNAType()
    {
        return dnaType;
    }

    public void SpawnRNA()
    {
        if (holder.CanISpawnRNA())
        {
            if (dnaType != 0)
            {
                if (holder.isTutorialRN() && step1)
                {
                    FindObjectOfType<GameController>().InitiateNextStep();
                    step1 = false;
                    step2 = true;
                }
                GameObject.Find("SFXController").GetComponent<AudioButtonController>().Play("Swoosh");
                holder.SetCanSpawnRNA(false);
                SetAsActiveCaller(true);
                Vector2 force = new Vector2(0, 30);
                Vector3 location = new Vector3(transform.position.x, transform.position.y + 0.8f);
                GameObject RNA = Instantiate(DNA, location, Quaternion.Euler(0, 0, 0));
                SpacerDNA rnaScript = RNA.GetComponent<SpacerDNA>();
                rnaScript.SetSpacer(this);
                //rnaScript.SetPositionInArray(position);
                rnaScript.SetType(dnaType);
                rnaScript.SetPickedToTrue();
                holder.SpawnUninteractableDuplicates();
                RNA.GetComponent<Rigidbody2D>().AddForce(force);
                if (holder.isTutorialRN() && step2)
                {
                    rnaScript.AdvanceTutorial();
                    step2 = false;
                    step3 = true;
                }
                else if (holder.isTutorialRN() && step3)
                {
                    rnaScript.AdvanceTutorial();
                    rnaScript.HasAdvanced();
                }
                /*if (count == 0)
                  {
                    myDNAs[2] = RNA;
                  }
                  AddtoCount();
                  CheckCount(RNA);
                  */
            }
        }
    }

    //Spawns RNA that automatically dies and can't be picked up or interacted with 
    public void SpawnFakeRNA()
    {
        Vector2 force = new Vector2(0, 30);
        Vector3 location = new Vector3(transform.position.x, transform.position.y + 0.8f);
        GameObject RNA = Instantiate(DNA, location, Quaternion.Euler(0, 0, 0));
        RNA.GetComponent<BoxCollider2D>().enabled = false;
        SpacerDNA rnaScript = RNA.GetComponent<SpacerDNA>();
        rnaScript.SetType(dnaType);
        rnaScript.SetSpacer(this);
        RNA.GetComponent<Rigidbody2D>().AddForce(force);
        rnaScript.StartFade();
    }

    //Everything below this point is prototype code for keeping a limited number of DNA at any one time

    public void CheckCount( GameObject rna)
    {
        if (count > 3)
        {
            Destroy(myDNAs[2]);
        }
    }

    public void AddtoCount()
    {
        count += 1;
    }

    public void SubtractCount()
    {
        count -= 1;
    }

    public void SetSpacerFather(bool someBool)
    {
        holder.SetCanSpawnRNA(someBool);
    }
} 
