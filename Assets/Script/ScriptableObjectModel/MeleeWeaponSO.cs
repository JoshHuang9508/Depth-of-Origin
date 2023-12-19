using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;

[CreateAssetMenu(fileName = "new melee weapon", menuName = "Items/Weapon/Melee Weapon")]
public class MeleeWeaponSO : WeaponSO, IEquipable, IUnequipable
{
    [Header("Object Reference")]
    public GameObject weaponObject;

    [Header("Melee Weapon Setting")]
    public float attackSpeed = 1f;

    public bool EquipObject(int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemState = null)
    {
        PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            player.SetEquipment(this, WeaponType.Melee);
            if (inventoryData.GetItemAt(inventoryIndex).item is IDestoryableItem) inventoryData.RemoveItem(inventoryIndex, amount);
        }
        return false;
    }

    public bool UnequipObject(int amont, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemstate)
    {
        PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            player.UnEquipment(this, WeaponType.Melee);
            player.inventoryData.AddItem(inventoryData.GetItemAt(inventoryIndex));
        }
        return false;
    }
}
