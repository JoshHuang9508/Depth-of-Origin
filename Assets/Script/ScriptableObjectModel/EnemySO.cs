using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using Inventory.Model;

[CreateAssetMenu(fileName = "new enemy", menuName = "Enemy")]
public class EnemySO : ScriptableObject
{
    public GameObject EnemyObject;

    [Header("Basic Data")]
    public string Name;
    public float health;
    public float moveSpeed;
    public Difficulty difficulty = Difficulty.Easy;

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
    public List<ItemSO> lootings;
    public List<GameObject> wreckage;
}

public enum AttackType
{
    Melee, Sniper
}

public enum Difficulty
{
    Easy, Normal, Hard, Difficult, Extreme
}
