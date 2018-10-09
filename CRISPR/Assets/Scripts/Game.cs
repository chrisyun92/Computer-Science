using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game
{
    public static Game current;
    public bool openedBefore;

    public Game()
    {
        try
        {
            openedBefore = GameObject.Find("Opening Controller").GetComponent<OpeningController>().OpenedBefore();
        }
        catch (System.Exception e)
        {
            openedBefore = false;
        }
    }
}