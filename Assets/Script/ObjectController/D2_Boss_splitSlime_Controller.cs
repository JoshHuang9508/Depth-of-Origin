using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D2_Boss_splitSlime_Controller : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] private EnemyBehavior enemyBehavior;
    [SerializeField] private GameObject controller, smallerSlime; 
    private void Start()
    {
        enemyBehavior.currentRb.bodyType = RigidbodyType2D.Dynamic;
        enemyBehavior.enemy.attackType = EnemySO.AttackType.Melee;
        enemyBehavior.enemy.attackField = 1.5f;
        enemyBehavior.enemy.chaseField = 100;
        enemyBehavior.enemy.attackSpeed = 1;
        enemyBehavior.enemy.attackDamage = 2000;
    }

    private void OnDestroy()
    {
        controller.GetComponent<Boss2Controller>().deathCounter += 1;
    }
}
