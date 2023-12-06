using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BossSummonerContorller : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int neededActionTimes;
    [SerializeField] private Vector3 summonPos;

    [Header("Object Reference")]
    [SerializeField] private EnemySO boss;
    [SerializeField] private Interactable interactable;

    [Header("Dynamic Data")]
    [SerializeField] private static int currentActionTimes;

    [Header("Stats")]
    public bool isActived = false;


    private void Start()
    {
        interactable = GetComponent<Interactable>();

        currentActionTimes = 0;
        Console.WriteLine("GAY!!!");
    }

    private void Update()
    {
        if (currentActionTimes >= neededActionTimes)
        {
            Destroy(gameObject);
        }
    }

    public void OnInteraction()
    {
        if (isActived) return;

        currentActionTimes++;
        isActived = true;
        interactable.enabled = false;

        if (currentActionTimes >= neededActionTimes)
        {
            var bossSummoned = Instantiate(
                boss.EnemyObject,
                summonPos,
                Quaternion.identity,
                GameObject.FindWithTag("Entity").transform);
            bossSummoned.GetComponent<EnemyBehavior>().enemy = boss;
        }
    }
}
