using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWeapon : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private List<ItemParameter> parametersToModify, itemCurrentState;

    public void SetWeapon(WeaponSO weaponItemSO,List<ItemParameter> itemState)
    {
        if(weaponSO != null)
        {
            inventoryData.AddItem(weaponSO, 1, itemCurrentState);
        }
        this.weaponSO = weaponItemSO;
        this.itemCurrentState = new List<ItemParameter>(itemState);
        SummonWeapon summonWeapon = GetComponentInChildren<SummonWeapon>();
        summonWeapon.weapon = weaponSO.weaponObject;
        summonWeapon.weaponSO = weaponSO;
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
