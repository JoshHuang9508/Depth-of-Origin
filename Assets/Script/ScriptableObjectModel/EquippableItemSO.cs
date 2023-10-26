using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new equippableItem", menuName = "Items/Equippable Itme")]
public class EquippableItemSO : ItemSO, IItemAction, IDestoryableItem
{
    [Header("Effect settings")]
    public float E_maxHealth;
    public float E_strength;
    public float E_walkSpeed;
    public float E_defence;
    public float E_critRate;
    public float E_critDamage;
    public EquipmentType equipmentType;
    public enum EquipmentType
    {
        armor, book, jewelry
    }

    public string ActionName => "Equip";

    public bool PerformAction(GameObject _player, List<ItemParameter> itemState = null)
    {
        PlayerBehaviour player = _player.GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            player.SetEquipment(this, equipmentType);
            return true;
        }
        return false;
    }
}