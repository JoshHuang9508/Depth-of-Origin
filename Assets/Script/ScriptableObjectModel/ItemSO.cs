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

        [Header("Reference")]
        public Sprite Image;
        public GameObject DropItem;
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
}


