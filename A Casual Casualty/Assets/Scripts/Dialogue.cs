using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//create a new class called Dialogue
[System.Serializable]
public class Dialogue {

    public string name;
    [TextArea(3, 10)]
    public string[] sentences;
}
