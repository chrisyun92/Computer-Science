using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Types {
    public enum Virus {
        Red,
        Blue,
        Green,
        Yellow,
        Pink,
        Teal,
        Black,
    };

    public enum DNA {
        Red,
        Blue,
        Green,
        Yellow,
        Pink,
        Teal,
        Black,
    }

    public static Color GetVirusColor(Virus type) {
        switch(type) {
            case Virus.Red:
                return new Color(1, 0, 0);
            case Virus.Green:
                return new Color(0, 1, 0);
            case Virus.Blue:
                return new Color(0, 0, 1);
            case Virus.Yellow:
                return new Color(234, 255, 0);
            case Virus.Pink:
                return new Color(255, 61, 243);
            case Virus.Teal:
                return new Color(0, 145, 255);
            case Virus.Black:
                return new Color(0, 0, 0);
            default:
                return new Color(0, 0, 0);
        }
    }

    public static Color GetDNAColor(DNA type) {
        switch (type) {
            case DNA.Red:
                return new Color(1, 0, 0);
            case DNA.Green:
                return new Color(0, 1, 0);
            case DNA.Blue:
                return new Color(0, 0, 1);
            case DNA.Yellow:
                return new Color(234, 255, 0);
            case DNA.Pink:
                return new Color(255, 0, 241);
            case DNA.Teal:
                return new Color(0, 145, 255);
            case DNA.Black:
                return new Color(0, 0, 0);
            default:
                return new Color(0, 0, 0);
        }
    }

    public static Color GetColor(int type)
    {
        switch (type)
        {
            case 1:
                return new Color(1, 0, 0);
            case 3:
                return new Color(0, 1, 0);
            case 2:
                return new Color(0, 0, 1);
            case 5:
                return new Color(234, 255, 0);
            case 4:
                return new Color(255, 0, 241);
            case 6:
                return new Color(0, 145, 255);
            case 7:
                return new Color(0, 0, 0);
            default:
                return new Color(0, 0, 0);
        }
    }

    public static DNA GetDNA(int type)
    {
        switch (type)
        {
            case 1:
                return DNA.Red;
            case 3:
                return DNA.Green;
            case 2:
                return DNA.Blue;
            case 5:
                return DNA.Yellow;
            case 4:
                return DNA.Pink;
            case 6:
                return DNA.Teal;
            case 7:
                return DNA.Black;
            default:
                return DNA.Red;
        }
    }

    public static int GetVirusSpawnTime(Virus type) {
        return 1;
    }
} 
