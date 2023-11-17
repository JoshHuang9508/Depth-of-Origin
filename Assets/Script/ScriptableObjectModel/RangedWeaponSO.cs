using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "new ranged weapon", menuName = "Items/Weapon/Ranged Weapon")]
public class RangedWeaponSO : WeaponSO, IEquipable, IDestoryableItem, IItemAction, ISellable
{
    [Header("Projectile Object Settings")]
    public GameObject projectileObject;
    public float flySpeed;
    public int splitAmount = 1;
    public ProjectileType projectileType;

    public enum ProjectileType
    {
        Straight, Split
    }
}
