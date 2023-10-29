using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new equippableItem", menuName = "Items/Equippable Itme")]
public class EquippableItemSO : ItemSO, IEquipable, IDestoryableItem, IItemAction, ISellable, IBuyable
{
    [Header("Basic Data")]
    public EquipmentType equipmentType;
    public enum EquipmentType
    {
        armor, book, jewelry
    }

    public int price;

    [Header("Effect settings")]
    public float E_maxHealth;
    public float E_strength;
    public float E_walkSpeed;
    public float E_defence;
    public float E_critRate;
    public float E_critDamage;

    public bool EquipObject(int amount, GameObject character, List<ItemParameter> itemState = null)
    {
        PlayerBehaviour player = character.GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            player.SetEquipment(this, equipmentType);
        }
        return false;
    }

    public bool SellObject(int amount, GameObject character, List<ItemParameter> itemstate)
    {
        PlayerBehaviour player = character.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            player.currentCoinAmount += price;
        }
        return false;
    }

    public bool BuyObject(int amount, GameObject character, List<ItemParameter> itemstate)
    {
        PlayerBehaviour player = character.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            player.currentCoinAmount -= price;
        }
        return false;
    }
}