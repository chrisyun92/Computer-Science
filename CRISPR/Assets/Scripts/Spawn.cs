using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public GameObject destination;
    public GameObject[] viruses = new GameObject[1];
    //public float timeToSpawn;
    //private float elapsedTime = 0;
    public float rotationAngle;
    public int XForce;
    public int YForce;
    [SerializeField]
    private bool isTutorial = false;

    private Transform cellScene;

    public void SetTransformforVirus(Transform trans)
    {
        cellScene = trans;
    }

    public void SpawnVirus(int type)
    {
        GameObject item = Instantiate(viruses[0], transform.position, transform.rotation * Quaternion.Euler(0, 0, rotationAngle));
        item.transform.parent = cellScene;

        // Move our position a step closer to the target.
        item.GetComponent<Flow>().to = destination.transform;
        item.GetComponent<Virus>().SetForce(XForce, YForce);
        item.GetComponent<Virus>().SetDNAType(type);
        if (isTutorial)
        {
            item.GetComponent<Virus>().SetIsTutorial();
        }
        //item.GetComponent<Virus>().SetTransformParent(cellScene);
    }

    void Update()
    {
    }

    public void RandomSpawn()
    {
        rotationAngle = Random.Range(0, 359);
        GameObject item = Instantiate(viruses[0], transform.position, transform.rotation * Quaternion.Euler(0, 0, rotationAngle));
        item.GetComponent<Flow>().to = null;
    }
}
