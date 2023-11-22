using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BossBehavior : MonoBehaviour
{
    [Header("Setting")]
    public List<Vector2> positionList;

    [Header("Object Reference")]
    [SerializeField] private EnemySO enemy;
    [SerializeField] private EnemyBehavior enemyBehavior;
    [SerializeField] private Rigidbody2D currentRb;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject column;

    int behaviorType = 1;

    void Start()
    {
        enemyBehavior = GetComponent<EnemyBehavior>();
        currentRb = GetComponent<Rigidbody2D>();
        enemy = enemyBehavior.enemy;

        column.GetComponent<BossColumnController>().shieldBreak += RemoveShield;

        StartCoroutine(delay(callback => {
            enemyBehavior.behaviourEnabler = callback;
        }, 5f));

        BuildColumns();
    }

    private void Update()
    {
        if(enemyBehavior.currentHealth <= enemyBehavior.enemy.health * 0.5 && behaviorType == 1)
        {
            StartCoroutine(delay(callback =>
            {
                shield.SetActive(!callback);
                if (callback)
                {
                    enemyBehavior.movementDisableTimer = 0;
                    enemyBehavior.behaviourEnabler = true;
                    behaviorType = 2;
                }
            }, 3f));
        }

        switch (behaviorType)
        {
            case 1:
                enemyBehavior.movementDisableTimer += enemyBehavior.movementDisableTimer < 5 ? 1000 : 0;
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
            enemyBehavior.behaviourEnabler = callback;
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
