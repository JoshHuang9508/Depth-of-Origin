using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    [Header("Setting")]
    public List<Vector2> positionList;

    [Header("Object Reference")]
    [SerializeField] private EnemyBehavior enemyBehavior;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject column;

    int behaviorType = 1;

    void Start()
    {
        column.GetComponent<BossColumnController>().shieldBreak += RemoveShield;

        StartCoroutine(SetTimer(callback => {
            enemyBehavior.behaviourEnabler = callback;
        }, 5f));

        BuildColumns();
    }

    private void Update()
    {
        if(enemyBehavior.currentHealth <= enemyBehavior.enemy.health * 0.5 && behaviorType == 1)
        {
            StartCoroutine(SetTimer(callback => {
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
                enemyBehavior.movementDisableTimer = enemyBehavior.movementDisableTimer < 5 ? 1000 : 0;
                enemyBehavior.currentRb.bodyType = RigidbodyType2D.Static;
                enemyBehavior.enemy.attackType = EnemySO.AttackType.Sniper;
                enemyBehavior.enemy.attackField = 100;
                enemyBehavior.enemy.chaseField = 0;
                enemyBehavior.enemy.attackSpeed = 0.4f;
                enemyBehavior.enemy.attackDamage = 1500;
                break;

            case 2:
                enemyBehavior.currentRb.bodyType = RigidbodyType2D.Dynamic;
                enemyBehavior.enemy.attackType = EnemySO.AttackType.Melee;
                enemyBehavior.enemy.attackField = 1.5f;
                enemyBehavior.enemy.chaseField = 100;
                enemyBehavior.enemy.attackSpeed = 1;
                enemyBehavior.enemy.attackDamage = 2000;
                break;
        }
    }

    public void RemoveShield()
    {
        StartCoroutine(SetTimer((callback) =>{
            shield.SetActive(callback && behaviorType == 1);
            if (callback && behaviorType == 1)  BuildColumns();
        }, 60));

        StartCoroutine(SetTimer(callback => {
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

    private IEnumerator SetTimer(System.Action<bool> callback, float time)
    {
        callback(false);
        yield return new WaitForSeconds(time);
        callback(true);
    }
}
