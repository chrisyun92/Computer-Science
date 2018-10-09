using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacerHolder : MonoBehaviour {

    public GameObject myCas;

    private Cas1Cas2 script;
    private int[] virusTypes = new int[8];
    private int counter = 0;
    [SerializeField]
    private Transform[] spacers = new Transform[5];
    [SerializeField]
    private bool isTutorial;
    private bool canSpawnMoreRNA = true;

    // Use this for initialization
    void Start () {
        script = myCas.GetComponent<Cas1Cas2>();
        int numTrans = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddVirusType(int newVirus)
    {
        virusTypes[newVirus] = newVirus;
        script.AddTypeTransported(newVirus);
    }

    public bool InVirusTypes(int virusType)
    {
        foreach (int virus in virusTypes)
        {
            if (virusType == virus)
            {
                return true;
            } 
        }
        return false;
    }

    public void SetCanSpawnRNA(bool tf)
    {
        canSpawnMoreRNA = tf;
    }

    public void RemoveVirusType(int virus)
    {
        virusTypes[virus] = 0;
        script.RemoveTypeTransported(virus);
    }

    public bool CanISpawnRNA()
    {
        return canSpawnMoreRNA;
    }

    public void SpawnUninteractableDuplicates()
    {
        foreach (Transform spacer in spacers)
        {
            if (spacer != null && spacer.gameObject.activeSelf) 
            {
                Spacer spacerScript = spacer.gameObject.GetComponent<Spacer>();
                if (!spacerScript.AmActiveCaller())
                {
                    spacerScript.SpawnFakeRNA();
                } else
                {
                    spacerScript.SetAsActiveCaller(false);
                }
            }
        }
    }

    public bool isTutorialRN()
    {
        return isTutorial;
    }

    public void SwitchTutorialBool()
    {
        isTutorial = !isTutorial;
    }
}
