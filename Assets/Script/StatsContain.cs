using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsContain : MonoBehaviour
{
    [Header("Settng")]
    [SerializeField] private List<StatsList> statsLists;

    private void Start()
    {
        List<StatsList> loadedStatsLists = SaveLoadManager.LoadObjectState(this.name);

        if (loadedStatsLists != null)
        {


        }
    }

    private void OnDestroy()
    {
        SaveLoadManager.SaveObjectState(statsLists, this.name);
    }
}

[Serializable]
public class StatsList
{
    public string objectName;
    public ValueType valueType;
    public int intVal;
    public float floatVal;
    public double doubleVal;
    public string stringVal;
    public bool boolVal;

    public enum ValueType
    {
        Int, Float, Double, String, Bool
    }
}
