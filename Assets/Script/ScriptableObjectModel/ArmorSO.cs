using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="new Armor",menuName ="Items/Armor")]
public class ArmorSO : ItemSO, IItemAction, IDestoryableItem
{
    public GameObject armorObject;
    public string ActionName => "equip";

    public bool PerformAction(GameObject player, List<ItemParameter> itemState = null)
    {
        PlayerBehaviour armor = player.GetComponent<PlayerBehaviour>();
        if (armor != null)
        {
            armor.armorSO = this;
            return true;
        }
        return false;
    }
}
