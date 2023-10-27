using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    public abstract class ItemSO : ScriptableObject
    {
        [Header("Describtion")]
        public string Name;
        public string Description;

        [Header("State")]
        public bool IsStackable;
        public int MaxStackSize = 1;
        public Rarity Rarity;
        

        [Header("Reference")]
        public Sprite Image;
        public List<ItemParameter> DefaultParameterList;

        public int ID => GetInstanceID();

        public bool SelectAction(GameObject character, List<ItemParameter> itemState, int amount, int selection)
        {
            switch (selection)
            {
                case 1:
                    IEquipable equipable = this as IEquipable;
                    equipable.PerformAction(character, amount, itemState);
                    break;
                case 2:
                    IActionable actionable = this as IActionable;
                    actionable.PerformAction2(character, amount, itemState);
                    break;
            }
            return false;
        }

        public string SelectAction(int selection)
        {
            switch (selection)
            {
                case 1:
                    return "Equip";
                case 2:
                    return "Consume";
            }
            return "";
        }
    }

    [Serializable]
    public struct ItemParameter : IEquatable<ItemParameter>
    {
        public ItemParameterSO itemParameter;
        public float value;

        public bool Equals(ItemParameter other)
        {
            return other.itemParameter == itemParameter;
        }
    }

    public enum Rarity
    {
        Common, Uncommon, Rare, Exotic, Mythic, Legendary 
    }

    public interface IDestoryableItem
    {

    }

    public interface IEquipable
    {
        bool PerformAction(GameObject character, int amount, List<ItemParameter> itemState);
    }

    public interface IActionable
    {
        bool PerformAction2(GameObject character, int amount, List<ItemParameter> itemState);
    }

    public interface IItemAction
    {
        bool SelectAction(GameObject character, List<ItemParameter> itemState, int amount, int selection);
        string SelectAction(int selection);
    }
}




