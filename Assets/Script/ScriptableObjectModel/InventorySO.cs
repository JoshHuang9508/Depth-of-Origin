using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Model 
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        [SerializeField] public List<InventoryItem> inventoryItems;
        [field: SerializeField] public int Size { get; private set; } = 10;
        



        public void initialize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.item, item.quantity);
        }

        public int AddItem(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
            if(item.IsStackable == false)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while(quantity > 0 && IsInventoryFull() == false)
                    {
                        quantity -= AddItemToFristFreeSlot(item, 1, itemState);
                    }
                }

                OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
                return quantity;
            }

            else
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    if (inventoryItems[i].IsEmpty) continue;
                    if (inventoryItems[i].item.ID == item.ID)
                    {
                        int amountPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;

                        if (quantity > amountPossibleToTake)
                        {
                            inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize);
                            quantity -= amountPossibleToTake;
                        }
                        else
                        {
                            inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);

                            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
                            return quantity;
                        }
                    }
                }

                while (quantity > 0 && IsInventoryFull() == false)
                {
                    int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                    quantity -= newQuantity;
                    AddItemToFristFreeSlot(item, newQuantity);
                }

                OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
                return quantity;
            }
        }

        private int AddItemToFristFreeSlot(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
            InventoryItem newItem = new InventoryItem
            {
                item = item,
                quantity = quantity,
                itemState = new List<ItemParameter>(itemState == null ? item.DefaultParameterList : itemState)
            };
            for(int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }
            return 0;
        }

        public bool IsInventoryFull()
            => inventoryItems.Where(item => item.IsEmpty).Any() == false;

        public bool IsCertainItemFull(int itemID)
        {
            foreach (InventoryItem item in inventoryItems)
            {
                if (item.item.ID == itemID)
                {
                    if(item.quantity < item.item.MaxStackSize)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    continue;
                }
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }

        public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            InventoryItem item1 = inventoryItems[itemIndex_1];
            inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
            inventoryItems[itemIndex_2] = item1;

            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }

        public void RemoveItem(int itemIndex, int amount)
        {
            if(inventoryItems.Count > itemIndex)
            {
                if (inventoryItems[itemIndex].IsEmpty) return;

                int temp = (amount == -1 ) ? 0 : inventoryItems[itemIndex].quantity - amount;

                inventoryItems[itemIndex] = temp <= 0 ? InventoryItem.GetEmptyItem() : inventoryItems[itemIndex].ChangeQuantity(temp);

                OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
            }
        }
        public void AddItem(int itemIndex, int amount)
        {
            if (inventoryItems.Count > itemIndex)
            {
                if (inventoryItems[itemIndex].IsEmpty) return;

                int temp = inventoryItems[itemIndex].quantity + amount;

                inventoryItems[itemIndex] = temp <= 0 ? InventoryItem.GetEmptyItem() : inventoryItems[itemIndex].ChangeQuantity(temp);

                OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
            }
        }

    }

    [Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public ItemSO item;
        public List<ItemParameter> itemState;

        public bool IsEmpty => item == null;

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                item = this.item,
                quantity = newQuantity,
                itemState = new List<ItemParameter>(this.itemState)
            };
        }

        public static InventoryItem GetEmptyItem() => new InventoryItem
        {
            item = null,
            quantity = 0,
            itemState = new List<ItemParameter>()
        };
    }
}

