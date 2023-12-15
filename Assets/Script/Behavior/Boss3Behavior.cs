using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Behavior : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] private EnemyBehavior enemyBehavior;

    [SerializeField] private float disableTimer = 10;
    [SerializeField] private int behaviorType = 1;

    


    private void Start()
    {
        enemyBehavior.OnAttack += Attacking;

        StartCoroutine(SetTimer(callback => {
            enemyBehavior.behaviourEnabler = callback;
        }, 5f));
    }

    private void Update()
    {
        if (enemyBehavior.currentHealth <= enemyBehavior.enemy.health * 0.5 && behaviorType == 1)
        {
            enemyBehavior.movementDisableTimer = 3;
            enemyBehavior.attackDisableTimer = 13;
            behaviorType = 2;
        }

        if (!enemyBehavior.behaviourEnabler) return;

        if (enemyBehavior.attackDisableTimer <= 3)
        {
            //attack warning
        }

        switch (behaviorType)
        {
            case 1:

                if (enemyBehavior.haveShield)
                {
                    disableTimer = 10;
                }
                else if (!enemyBehavior.haveShield)
                {
                    enemyBehavior.attackDisableTimer = 3;
                    enemyBehavior.movementDisableTimer = 3;

                    disableTimer -= Time.deltaTime;

                    enemyBehavior.ShieldHealth = enemyBehavior.enemy.shieldHealth * (1 - Mathf.Max(disableTimer / 10, 0));

                    if (disableTimer <= 0) enemyBehavior.SetShield();
                }
                break;

            case 2:

                //switch projectile
                enemyBehavior.ShieldHealth = 0;

                break;
        }
    }

    private void Attacking()
    {
        enemyBehavior.ShieldHealth -= enemyBehavior.enemy.shieldHealth * 0.25f;
    }

    private IEnumerator SetTimer(System.Action<bool> callback, float time)
    {
        callback(false);
        yield return new WaitForSeconds(time);
        callback(true);
    }
}
