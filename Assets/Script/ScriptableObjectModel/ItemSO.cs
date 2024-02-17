using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    public abstract class ItemSO : ScriptableObject
    {
        [Header("Basic Data")]
        public string Name;
        [TextArea] public string Description;

        [Header("Setting")]
        public bool IsStackable;
        public bool isStorable = true;
        public int MaxStackSize = 1;
        public Rarity Rarity;
        public int sellPrice, buyPrice;

        [Header("Reference")]
        public Sprite Image;


        public int ID => GetInstanceID();



        public bool SellObject(int amount, InventorySO inventoryData, int inventoryIndex)
        {
            PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                player.currentCoinAmount += sellPrice;
                if(inventoryData.GetItemAt(inventoryIndex).item is IDestoryableItem) inventoryData.RemoveItem(inventoryIndex, amount);
            }
            return false;
        }

        public bool BuyObject(int amount, InventorySO inventoryData, int inventoryIndex)
        {
            PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                if(player.currentCoinAmount < buyPrice)
                {
                    Debug.Log("You don't have enough money!");
                }
                else
                {
                    player.currentCoinAmount -= buyPrice;
                    player.inventoryData.AddItem(inventoryData.GetItemAt(inventoryIndex));
                }
            }
            return false;
        }

        public bool DropItem(int amount, InventorySO inventoryData, int inventoryIndex)
        {
            PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                player.DropItems(inventoryData.GetItemAt(inventoryIndex).item, amount);
                inventoryData.RemoveItem(inventoryIndex, amount);
            }
            return false;
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
        bool EquipObject(int amount, InventorySO inventoryData, int inventoryIndex);
    }

    public interface IUnequipable
    {
        bool UnequipObject(int amount, InventorySO inventoryData, int inventoryIndex);
    }

    public interface IConsumeable
    {
        bool ConsumeObject(int amount, InventorySO inventoryData, int inventoryIndex);
    }

    public interface ISellable
    {
        bool SellObject(int amount, InventorySO inventoryData, int inventoryIndex);
    }

    public interface IBuyable
    {
        bool BuyObject(int amount, InventorySO inventoryData, int inventoryIndex);
    }

    public interface IDroppable
    {
        bool DropItem(int amount, InventorySO inventoryData, int inventoryIndex);
    }
}




