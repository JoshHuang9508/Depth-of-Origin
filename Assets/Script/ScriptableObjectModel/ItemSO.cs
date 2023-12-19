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
        public List<ItemParameter> DefaultParameterList;


        public int ID => GetInstanceID();

        //public bool SelectAction(string actionName, int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemState)
        //{
        //    switch (actionName)
        //    {
        //        case "Equip":
        //            IEquipable equipable = this as IEquipable;
        //            equipable.EquipObject(amount, inventoryData, inventoryIndex, itemState);
        //            break;
        //        case "Consume":
        //            IConsumeable actionable = this as IConsumeable;
        //            actionable.ConsumeObject(amount, inventoryData, inventoryIndex, itemState);
        //            break;
        //        case "Sell":
        //            ISellable sellable = this as ISellable;
        //            sellable.SellObject(amount, inventoryData, inventoryIndex, itemState);
        //            break;
        //        case "Buy":
        //            IBuyable buyable = this as IBuyable;
        //            buyable.BuyObject(amount, inventoryData, inventoryIndex, itemState);
        //            break;
        //        case "Unequip":
        //            IUnequipable unequipable = this as IUnequipable;
        //            unequipable.UnequipObject(amount, inventoryData, inventoryIndex, itemState);
        //            break;
        //    }
        //    return false;
        //}

        public bool SellObject(int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemstate)
        {
            PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                player.currentCoinAmount += sellPrice;
                if(inventoryData.GetItemAt(inventoryIndex).item is IDestoryableItem) inventoryData.RemoveItem(inventoryIndex, amount);
            }
            return false;
        }

        public bool BuyObject(int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemstate)
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

        public bool DropItem(int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemstate)
        {
            PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                ItemDropper ItemDropper = Instantiate(
                    player.itemDropper,
                    new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z),
                    new Quaternion(0.0f, 0.0f, 0.0f, 0.0f),
                    GameObject.FindWithTag("Item").transform
                    ).GetComponent<ItemDropper>();
                ItemDropper.DropItems(inventoryData.GetItemAt(inventoryIndex).item, amount);
                inventoryData.RemoveItem(inventoryIndex, amount);
            }
            return false;
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

    //public interface IItemAction
    //{
    //    bool SelectAction(string actionName, int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemState);
    //}

    public interface IEquipable
    {
        bool EquipObject(int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemState);
    }

    public interface IUnequipable
    {
        bool UnequipObject(int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemstate);
    }

    public interface IConsumeable
    {
        bool ConsumeObject(int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemState);
    }

    public interface ISellable
    {
        bool SellObject(int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemstate);
    }

    public interface IBuyable
    {
        bool BuyObject(int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemstate);
    }

    public interface IDroppable
    {
        bool DropItem(int amount, InventorySO inventoryData, int inventoryIndex, List<ItemParameter> itemstate);
    }
}




