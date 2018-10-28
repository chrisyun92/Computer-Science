using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSaver : MonoBehaviour {

    private int level = 0;


	public void ChangeLevel(int levelToStart)
    {
        level = levelToStart;
    }

    public int LevelNumber()
    {
        return level;
    }
}
