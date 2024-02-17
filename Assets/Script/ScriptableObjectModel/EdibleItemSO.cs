using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new edibleItem", menuName = "Items/Edible Itme")]
public class EdibleItemSO : ItemSO, IConsumeable, IEquipable, IDestoryableItem, ISellable, IBuyable, IUnequipable, IDroppable
{
    [Header("Effection")]
    public float E_heal;
    public float E_maxHealth;
    public float E_strength;
    public float E_walkSpeed;
    public float E_defence;
    public float E_critRate;
    public float E_critDamage;
    public float effectTime;


    public bool EquipObject(int amount, InventorySO inventoryData, int inventoryIndex)
    {
        PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>(); ;

        if (player != null)
        {
            player.SetEquipment(this, amount);
            if (inventoryData.GetItemAt(inventoryIndex).item is IDestoryableItem) inventoryData.RemoveItem(inventoryIndex, amount);
        }
        return false;
    }

    public bool ConsumeObject(int amount, InventorySO inventoryData, int inventoryIndex)
    {
        PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>(); ;

        if (player != null)
        {
            player.SetEffection(this, effectTime);
            if (inventoryData.GetItemAt(inventoryIndex).item is IDestoryableItem) inventoryData.RemoveItem(inventoryIndex, amount);
        }
        return false;
    }

    public bool UnequipObject(int amount, InventorySO inventoryData, int inventoryIndex)
    {
        PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>(); ;

        if (player != null)
        {
            player.UnEquipment(this, amount);
            player.inventoryData.AddItem(inventoryData.GetItemAt(inventoryIndex));
        }
        return false;
    }
}