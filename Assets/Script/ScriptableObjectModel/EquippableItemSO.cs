using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EquippableItemSO : ItemSO, IDestoryableItem, IItemAction
    {
        public string ActionName => "Equip";

        public bool PerformAction(GameObject player, List<ItemParameter> itemState = null)
        {
            AgentWeapon weapon = player.GetComponent<AgentWeapon>();
            if (weapon != null)
            {
                weapon.SetWeapon(this, itemState == null ? DefaultParameterList : itemState);
                return true;
            }
            return false;
        }
    }
}