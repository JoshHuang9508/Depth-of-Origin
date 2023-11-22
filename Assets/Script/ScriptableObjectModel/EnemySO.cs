using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using Inventory.Model;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "new enemy", menuName = "Enemy")]
public class EnemySO : ScriptableObject
{
    [Header("Basic Data")]
    public string Name;

    [Header("Setting")]
    public float health;
    public float moveSpeed;
    public float defence;
    public bool isBoss;
    public AttackType attackType;
    public float attackSpeed;
    public float attackDamage;
    public float chaseField;
    public float attackField;
    public float knockbackForce;
    public float knockbackTime;
    public ShootingType shootingType;
    public float projectileFlySpeed;
    public Difficulty difficulty = Difficulty.Easy;

    [Header("Looting")]
    public List<Coins> coins;
    public List<Lootings> lootings;
    public List<GameObject> wreckage;

    [Header("Object Reference")]
    public GameObject EnemyObject;
    public GameObject projectile;

    public enum Difficulty
    {
        Easy, Normal, Hard, Difficult, Extreme
    }

    public enum ShootingType
    {
        Single, Split, AllAngle
    }

    public enum AttackType
    {
        Melee, Sniper
    }


    public void Attack_Ranged(float startAngle, Vector3 startPosition)
    {
        switch (shootingType)
        {
            case ShootingType.Single:
                var ArrowSummoned = Instantiate(
                        projectile,
                        startPosition,
                        Quaternion.Euler(0, 0, startAngle - 90),
                        GameObject.FindWithTag("Item").transform);

                ArrowSummoned.AddComponent<ProjectileMovement_Enemy>();
                ArrowSummoned.GetComponent<WeaponMovementRanged>().startAngle = Quaternion.Euler(0, 0, startAngle);
                ArrowSummoned.GetComponent<ProjectileMovement_Enemy>().enemy = this;
                break;
                
            case ShootingType.Split:
                for (int i = -60; i <= 60; i += 30)
                {
                    var splitArrowSummoned = Instantiate(
                        projectile,
                        startPosition,
                        Quaternion.Euler(0, 0, startAngle + i - 90),
                        GameObject.FindWithTag("Item").transform);

                    splitArrowSummoned.AddComponent<ProjectileMovement_Enemy>();
                    splitArrowSummoned.GetComponent<WeaponMovementRanged>().startAngle = Quaternion.Euler(0, 0, startAngle + i);
                    splitArrowSummoned.GetComponent<ProjectileMovement_Enemy>().enemy = this;
                }
                break;

            case ShootingType.AllAngle:
                for (int i = -180; i <= 180; i += 18)
                {
                    var allAngleArrowSummoned = Instantiate(
                        projectile,
                        startPosition,
                        Quaternion.Euler(0, 0, startAngle - 90 + i),
                        GameObject.FindWithTag("Item").transform);

                    allAngleArrowSummoned.AddComponent<ProjectileMovement_Enemy>();
                    allAngleArrowSummoned.GetComponent<WeaponMovementRanged>().startAngle = Quaternion.Euler(0, 0, startAngle + i);
                    allAngleArrowSummoned.GetComponent<ProjectileMovement_Enemy>().enemy = this;
                }
                break;
        }
    }
}