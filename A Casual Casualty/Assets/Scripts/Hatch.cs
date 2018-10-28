using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour {

    Vector2 rayOrigin;
    public GameObject table;
    private RaycastHit2D[] objects;
    private Vector2 right;
    public bool isTorpedoInHatch;
    public bool isHatchClosed;
    public bool isTubeFilled;

    // Use this for initialization
    void Start()
    {
        rayOrigin = gameObject.GetComponent<Rigidbody2D>().position;
        isTorpedoInHatch = false;
        isHatchClosed = true;
        isTubeFilled = true;
}

    public void DetectTable()
    {
        right = Vector2.right;
        objects = Physics2D.CircleCastAll(rayOrigin, 0.8f, right, 0.8f);
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].transform.tag == "Table" && GameObject.Find("Table").GetComponent<Table>().isLoaded)
            {
                table.GetComponentInChildren<Table>().LoadSprite();
                GameObject.Find("Table").GetComponent<Table>().isLoaded = false;

                isTorpedoInHatch = true;

                // Kill players in the tube?
                // GameObject.Find("Tube").GetComponent<Tube>().KillZone(false);
            }
        }
    }
}
