using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new equippableItem", menuName = "Items/Equippable Itme")]
public class EquippableItemSO : ItemSO, IEquipable, IDestoryableItem, ISellable, IBuyable, IUnequipable, IDroppable
{
    [Header("Setting")]
    public EquipmentType equipmentType;

    [Header("Effection")]
    public float E_maxHealth;
    public float E_strength;
    public float E_walkSpeed;
    public float E_defence;
    public float E_critRate;
    public float E_critDamage;

    public enum EquipmentType
    {
        armor, book, jewelry
    }


    public bool EquipObject(int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemState = null)
    {
        PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            player.SetEquipment(this, equipmentType);
            if (inventoryData.GetItemAt(inventoryIndex).item is IDestoryableItem) inventoryData.RemoveItem(inventoryIndex, amount);
        }
        return false;
    }

    public bool UnequipObject(int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemstate)
    {
        PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            player.UnEquipment(this, equipmentType);
            player.inventoryData.AddItem(inventoryData.GetItemAt(inventoryIndex));
        }
        return false;
    }
}