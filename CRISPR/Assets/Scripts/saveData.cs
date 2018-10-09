using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class saveData
{
    public static Game savedGame;
    public static void Save()
    {
        savedGame = Game.current;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath +
        "/savedGames.gd");
        bf.Serialize(file, saveData.savedGame);
        file.Close();
    }
    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath +
                "/savedGames.gd", FileMode.Open);
            Debug.Log(file);
            saveData.savedGame = (Game) bf.Deserialize(file);
            file.Close();
        }
        else
        {
            Game.current = new global::Game();
            saveData.savedGame = Game.current;
        }
    }
}
