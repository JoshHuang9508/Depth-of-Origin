using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWeapon : MonoBehaviour
{
    [SerializeField] private EquippableItemSO weapon;
    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private List<ItemParameter> parametersToModify, itemCurrentState;

    public void SetWeapon(EquippableItemSO weaponItemSO,List<ItemParameter> itemState)
    {
        if(weapon != null)
        {
            inventoryData.AddItem(weapon, 1, itemCurrentState);
        }
        this.weapon = weaponItemSO;
        this.itemCurrentState = new List<ItemParameter>(itemState);
        SummonWeapon summonWeapon = GetComponentInChildren<SummonWeapon>();
        summonWeapon.weapons = weapon.item;
        ModifyParameters();
    }
    public void ModifyParameters()
    {
        foreach(var parameters in parametersToModify)
        {
            if (itemCurrentState.Contains(parameters))
            {
                int index = itemCurrentState.IndexOf(parameters);
                float newValue = itemCurrentState[index].value + parameters.value;
                itemCurrentState[index] = new ItemParameter
                {
                    itemParameter = parameters.itemParameter,
                    value = newValue,
                };
            }
        }
    }
}
