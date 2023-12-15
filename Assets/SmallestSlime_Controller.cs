using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallestSlime_Controller : MonoBehaviour
{
    [SerializeField] private GameObject spawnerPos;
    Boss2Controller controller;
    List<GameObject> entitylist = new();
    int slimeCounter = 0,notsmallestSlimeCounter = 0;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnDestroy()
    {
    }
}
