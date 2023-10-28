using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "new ranged weapon", menuName = "Items/Weapon/Ranged Weapon")]
public class RangedWeaponSO : WeaponSO, IEquipable, IDestoryableItem, IItemAction, ISellable
{
    [Header("projectile Object Settings")]
    public GameObject projectileObject;
    public float flySpeed;
}
