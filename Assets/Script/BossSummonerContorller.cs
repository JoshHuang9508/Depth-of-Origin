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
    public bool isActived = false;

    Interactable interactable;

    private void Start()
    {
        interactable = GetComponent<Interactable>();

        currentActionTimes = 0;
    }

    float time = 0;
    private void Update()
    {
        if (currentActionTimes >= neededActionTimes)
        {
            time += Time.deltaTime;
            if(time >= 0.5f)
            {
                Destroy(gameObject);
            }
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
