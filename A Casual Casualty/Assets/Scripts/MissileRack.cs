using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileRack : MonoBehaviour {

    Vector2 rayOrigin;
    public GameObject table;
    private RaycastHit2D[] objects;
    private Vector2 up;

    // Use this for initialization
    void Start () {
        rayOrigin = gameObject.GetComponent<Rigidbody2D>().position;
    }

    public void DetectTable () {
        up = Vector2.up;
        objects = Physics2D.CircleCastAll(rayOrigin, 0.6f, up, 0.6f);
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].transform.tag == "Table" && !GameObject.
                Find("Table").GetComponent<Table>().isLoaded)
            {
                table.GetComponentInChildren<Table>().LoadSprite();
                GameObject.Find("Table").GetComponent<Table>().isLoaded = true;
            }
        }
    }
}
