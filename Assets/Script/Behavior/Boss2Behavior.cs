using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Boss2Behavior : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] private EnemyBehavior enemyBehavior;
    [SerializeField] private GameObject poison;
    [SerializeField] private EnemySO smallerSlime;

    [Header("Dynamic Data")]
    [SerializeField] private GameObject target;
    bool poisonEnabler = true , directionEnabler = true ;
    int behaviorType = 1;

    

    private void Start()
    {
        enemyBehavior.currentRb.bodyType = RigidbodyType2D.Dynamic;
        enemyBehavior.enemy.attackType = EnemySO.AttackType.Melee;
        enemyBehavior.enemy.attackField = 2f;
        enemyBehavior.enemy.chaseField = 100;
        enemyBehavior.enemy.attackSpeed = 0.5f;
        enemyBehavior.enemy.attackDamage = 2000;
        target = GameObject.FindWithTag("Player");
        
    }
    private void Update()
    {
        float distant = 0f;
        distant = Vector3.Distance(gameObject.transform.position, target.transform.position);
        
        switch(behaviorType)
        {
            case 1:
                if (distant <= enemyBehavior.enemy.chaseField && distant > 9f)
                {
                    enemyBehavior.currentRb.bodyType = RigidbodyType2D.Dynamic;
                    enemyBehavior.enemy.attackType = EnemySO.AttackType.Sniper;
                    enemyBehavior.enemy.attackField = 100f;
                    enemyBehavior.enemy.chaseField = 100;
                    enemyBehavior.enemy.attackSpeed = 3f;
                    enemyBehavior.enemy.attackDamage = 2000;
                    enemyBehavior.enemy.knockbackForce = 30f;
                    directionEnabler = true;
                    enemyBehavior.attackEnabler = true;
                    if (directionEnabler && enemyBehavior.enemy.moveSpeed >= 0)
                    {
                        enemyBehavior.enemy.moveSpeed *= -1; 
                        directionEnabler = false;
                    }
                    enemyBehavior.enemy.attackType = EnemySO.AttackType.Sniper;
                }
                else if(distant <= 9f)
                {
                    enemyBehavior.currentRb.bodyType = RigidbodyType2D.Dynamic;
                    enemyBehavior.enemy.attackType = EnemySO.AttackType.Melee;
                    enemyBehavior.enemy.attackField = 2f;
                    enemyBehavior.enemy.chaseField = 100;
                    enemyBehavior.enemy.attackSpeed = 1;
                    enemyBehavior.enemy.attackDamage = 2000;
                    enemyBehavior.enemy.knockbackForce = 100f;
                    enemyBehavior.attackEnabler = false;
                    directionEnabler = true;
                    if (directionEnabler && enemyBehavior.enemy.moveSpeed < 0)
                    {
                        enemyBehavior.enemy.moveSpeed *= -1;
                        directionEnabler = false;
                    }
                    enemyBehavior.enemy.attackType = EnemySO.AttackType.Melee;
                    if (poisonEnabler)
                    {
                        GameObject _poison = Instantiate(poison, gameObject.transform.position, Quaternion.identity, this.transform);
                        StartCoroutine(SetTimer(callback => {
                            poisonEnabler = callback;
                        }, 10f));
                        _poison.GetComponent<Projectile_SlimeBalls_Behavior>().alivetime = 10f;
                    }
                }
                break;
            case 2:
                Destroy(gameObject);
                break;
        }

        if(enemyBehavior.currentHealth <= enemyBehavior.enemy.health * 0.01f && behaviorType == 1)
        {
            behaviorType = 2;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < 2; i++)
        {
            var bossSummoned = Instantiate(
                smallerSlime.EnemyObject,
                transform.position,
                Quaternion.identity,
                GameObject.FindWithTag("Entity").transform);
            bossSummoned.GetComponent<EnemyBehavior>().enemy = smallerSlime;
        }
    }
    private IEnumerator SetTimer(System.Action<bool> callback, float time)
    {
        callback(false);
        yield return new WaitForSeconds(time);
        callback(true);
    }

}
