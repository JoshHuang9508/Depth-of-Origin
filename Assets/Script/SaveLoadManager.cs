using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml.Linq;

public class SaveLoadManager
{
    public static void SaveObjectState(List<StatsList> statsLists, string name)
    {
        string json = JsonUtility.ToJson(statsLists);
        File.WriteAllText($"{name}.json", json);
    }

    public static List<StatsList> LoadObjectState(string name)
    {
        if (File.Exists($"{name}.json"))
        {
            string json = File.ReadAllText("ObjectSaveData.json");
            return JsonUtility.FromJson<List<StatsList>>(json);
        }
        else
        {
            Debug.LogWarning("Save file not found.");
            return null;
        }
    }
}
