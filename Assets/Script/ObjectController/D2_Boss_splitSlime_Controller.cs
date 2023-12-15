using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D2_Boss_splitSlime_Controller : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] private EnemyBehavior enemyBehavior;
    [SerializeField] private GameObject controller;
    [SerializeField] private EnemySO smallestSlime;
    private void Start()
    {
        enemyBehavior.currentRb.bodyType = RigidbodyType2D.Dynamic;
        enemyBehavior.enemy.attackType = EnemySO.AttackType.Melee;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < 2; i++)
        {
            var bossSummoned = Instantiate(
                smallestSlime.EnemyObject,
                transform.position,
                Quaternion.identity,
                GameObject.FindWithTag("Entity").transform);
            bossSummoned.GetComponent<EnemyBehavior>().enemy = smallestSlime;
        }
    }
}
