using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BossBehavior : MonoBehaviour
{
    EnemyBehavior enemyBehavior;
    Rigidbody2D currentRb;
    EnemySO enemy;

    [Header("Setting")]
    public List<Vector2> positionList;

    [Header("Connect Object")]
    public GameObject shield;
    public GameObject column;

    int behaviorType = 1;

    void Start()
    {
        enemyBehavior = GetComponent<EnemyBehavior>();
        currentRb = GetComponent<Rigidbody2D>();
        enemy = enemyBehavior.enemy;

        column.GetComponent<BossColumnController>().shieldBreak += RemoveShield;

        enemyBehavior.movementEnabler = false;

        StartCoroutine(delay(callback => {
            enemyBehavior.behaviourEnabler = callback;
        }, 5f));

        BuildColumns();
    }

    private void FixedUpdate()
    {
        if(enemyBehavior.currentHealth <= enemyBehavior.enemy.health * 0.5 && behaviorType == 1)
        {
            behaviorType = 2;
            StartCoroutine(delay(callback =>
            {
                shield.SetActive(!callback);
                if (callback)
                {
                    enemyBehavior.movementEnabler = true;
                    enemyBehavior.attackEnabler = true;
                }
            }, 3f));
        }

        switch (behaviorType)
        {
            case 1:
                currentRb.bodyType = RigidbodyType2D.Static;
                enemy.attackType = EnemySO.AttackType.Sniper;
                enemy.attackField = 100;
                enemy.chaseField = 0;
                enemy.attackSpeed = 0.4f;
                enemy.attackDamage = 1500;
                break;

            case 2:
                currentRb.bodyType = RigidbodyType2D.Dynamic;
                enemy.attackType = EnemySO.AttackType.Melee;
                enemy.attackField = 1.5f;
                enemy.chaseField = 100;
                enemy.attackSpeed = 1;
                enemy.attackDamage = 2000;
                break;

        }
    }

    public void RemoveShield()
    {
        StartCoroutine(delay((callback) =>
        {
            shield.SetActive(callback && behaviorType == 1);
            if (callback && behaviorType == 1)  BuildColumns();
        }, 60));

        StartCoroutine(delay(callback => {
            enemyBehavior.movementEnabler = callback;
            enemyBehavior.attackEnabler = callback;
        }, 65f));
    }

    public void BuildColumns()
    {
        for(int i = 0; i < 6; i++)
        {
            BossColumnController columnSummoned = Instantiate(column, new Vector3(
                positionList[i].x, positionList[i].y, 0),
                Quaternion.identity,
                GameObject.FindWithTag("Object").transform
                ).GetComponent<BossColumnController>();
            columnSummoned.shieldBreak += RemoveShield;
            columnSummoned.Reset();
        }
    }

    private IEnumerator delay(System.Action<bool> callback, float delayTime)
    {
        callback(false);
        yield return new WaitForSeconds(delayTime);
        callback(true);
    }
}
