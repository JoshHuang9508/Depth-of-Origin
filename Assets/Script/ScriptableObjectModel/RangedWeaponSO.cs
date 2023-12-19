using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "new ranged weapon", menuName = "Items/Weapon/Ranged Weapon")]
public class RangedWeaponSO : WeaponSO, IEquipable, IUnequipable
{
    [Header("Object Reference")]
    public GameObject projectileObject;

    [Header("Projectile Settings")]
    public ShootingType shootingType;
    public float flySpeed;
    public int splitAmount = 1;

    public enum ShootingType
    {
        Single, Split
    }

    public bool EquipObject(int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemState = null)
    {
        PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            player.SetEquipment(this, WeaponType.Ranged);
            if (inventoryData.GetItemAt(inventoryIndex).item is IDestoryableItem) inventoryData.RemoveItem(inventoryIndex, amount);
        }
        return false;
    }

    public bool UnequipObject(int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemstate)
    {
        PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            player.UnEquipment(this, WeaponType.Ranged);
            player.inventoryData.AddItem(inventoryData.GetItemAt(inventoryIndex));
        }
        return false;
    }
}
