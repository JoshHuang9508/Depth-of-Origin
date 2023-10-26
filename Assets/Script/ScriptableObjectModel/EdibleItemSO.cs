using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new edibleItem", menuName = "Items/Edible Itme")]
public class EdibleItemSO : ItemSO,IItemAction,IDestoryableItem
{
    [Header("Effect settings")]
    public float E_heal;
    public float E_maxHealth;
    public float E_strength;
    public float E_walkSpeed;
    public float E_defence;
    public float E_critRate;
    public float E_critDamage;
    public float effectTime;

    public string ActionName => "Consume";

    public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
    {
        PlayerBehaviour player = character.GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            player.SetEquipment(this, effectTime);
        }
        return false;
    }

    
}

public interface IDestoryableItem
{

}

public interface IItemAction
{
    public string ActionName { get; }
    bool PerformAction(GameObject character, List<ItemParameter> itemState);

}