using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BossSummonerContorller : MonoBehaviour
{
    [Header("Settings")]
    public int neededActionTimes;
    public EnemySO boss;
    public Vector3 summonPos;

    [Header("Status")]
    public static int currentActionTimes;

    private void Start()
    {
        currentActionTimes = 0;
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
        currentActionTimes++;
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
