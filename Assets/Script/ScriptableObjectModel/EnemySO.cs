using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "new enemy", menuName = "EnemySO")]
public class EnemySO : ScriptableObject
{
    public GameObject EnemyObject;

    [Header("Basic Data")]
    public string Name;
    public float health;
    public float moveSpeed;
    public Difficulty difficulty;

    [Header("Attacking")]
    public AttackType attackType;
    public float attackSpeed;
    public float attackDamage;
    public float chaseField;
    public float attackField;
    public float knockbackForce;
    public float knockbackTime;
    public float knockbackSpeed;

    [Header("Looting")]
    public int lootMinItems;
    public int lootMaxItems;
    public List<GameObject> lootings;
}

public enum AttackType
{
    Melee, Sniper
}

public enum Difficulty
{
    Easy, Normal, Hard, Difficult, Extreme
}
