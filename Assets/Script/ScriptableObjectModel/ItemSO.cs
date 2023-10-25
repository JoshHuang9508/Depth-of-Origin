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
}


