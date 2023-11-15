using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    public abstract class ItemSO : ScriptableObject, IItemAction
    {
        [Header("Describtion")]
        public string Name;
        [TextArea]public string Description;

        [Header("State")]
        public bool IsStackable;
        public bool isStorable = true;
        public int MaxStackSize = 1;
        public Rarity Rarity;
        public int sellPrice, buyPrice;
        

        [Header("Reference")]
        public Sprite Image;
        public List<ItemParameter> DefaultParameterList;


        public int ID => GetInstanceID();

        public bool SelectAction(string actionName, int amount, GameObject character, List<ItemParameter> itemState)
        {
            switch (actionName)
            {
                case "Equip":
                    IEquipable equipable = this as IEquipable;
                    equipable.EquipObject(amount, character, itemState);
                    break;
                case "Consume":
                    IConsumeable actionable = this as IConsumeable;
                    actionable.ConsumeObject(amount, character, itemState);
                    break;
                case "Sell":
                    ISellable sellable = this as ISellable;
                    sellable.SellObject(amount, character, itemState);
                    break;
                case "Buy":
                    IBuyable buyable = this as IBuyable;
                    buyable.BuyObject(amount, character, itemState);
                    break;
            }
            return false;
        }

        public bool SellObject(int amount, GameObject character, List<ItemParameter> itemstate)
        {
            PlayerBehaviour player = character.GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                player.currentCoinAmount += sellPrice;
            }
            return false;
        }

        public bool BuyObject(int amount, GameObject character, List<ItemParameter> itemstate)
        {
            PlayerBehaviour player = character.GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                if(player.currentCoinAmount < buyPrice)
                {
                    Debug.Log("You don't have enough money!");
                }
                else
                {
                    player.currentCoinAmount -= buyPrice;
                }
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

    public interface IEquipable
    {
        bool EquipObject(int amount, GameObject character, List<ItemParameter> itemState);
    }

    public interface IConsumeable
    {
        bool ConsumeObject(int amount, GameObject character, List<ItemParameter> itemState);
    }

    public interface IItemAction
    {
        bool SelectAction(string actionName, int amount, GameObject character, List<ItemParameter> itemState);
    }

    public interface ISellable
    {
        bool SellObject(int amount, GameObject character, List<ItemParameter> itemstate);
    }

    public interface IBuyable
    {
        bool BuyObject(int amount, GameObject character, List<ItemParameter> itemstate);
    }
}




