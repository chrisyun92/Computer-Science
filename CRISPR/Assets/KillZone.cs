using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {

    private Cas1Cas2 myCas12;
    private Cas9 myCas9;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("cas1cas2"))
        {
            Debug.Log("cas12 in killzone");
            myCas12 = col.gameObject.GetComponentInParent<Cas1Cas2>();
            myCas12.MarkForDeath();
            myCas12.callSpecialDie();
        } else if (col.gameObject.CompareTag("cas9"))
        {
            myCas9 = col.gameObject.GetComponent<Cas9>();
            myCas9.Death();
        }
    }
}
