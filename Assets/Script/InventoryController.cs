using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private UIInventoryPage inventoryUI; 

        [SerializeField] private InventorySO InventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        bool initialEnabler = true;

        public void Start()
        {
            PrepareUI();
            if(initialEnabler) PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            InventoryData.initialize();
            InventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                InventoryData.AddItem(item);
            }
            initialEnabler = false;
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.Image, item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(InventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleActionRequest;
        }

        private void HandleActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = InventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                return;
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                inventoryUI.ShowItemAction(itemIndex);
                inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }
            IDestoryableItem destoryableItem = inventoryItem.item as IDestoryableItem;
            if (destoryableItem != null)
            {
                inventoryUI.AddAction("Drop", () => DropItem(itemIndex,inventoryItem.quantity));
            }
        }

        private void DropItem(int itemIndex, int quantity)
        {
            InventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.Reselection();
        }

        public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = InventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                return;
            }
            IDestoryableItem destoryableItem = inventoryItem.item as IDestoryableItem;
            if (destoryableItem != null)
            {
                InventoryData.RemoveItem(itemIndex, 1);
            }
            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject, inventoryItem.itemState);
                if (InventoryData.GetItemAt(itemIndex).IsEmpty)
                    inventoryUI.Reselection();
            }
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = InventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.Image, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            InventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = InventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.Reselection();
                return;
            }

            ItemSO item = inventoryItem.item;
            inventoryUI.UpdateDescription(itemIndex, item);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    inventoryUI.show();
                    foreach (var item in InventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UpdateData(item.Key, item.Value.item.Image, item.Value.quantity);
                    }
                }
                else
                {
                    inventoryUI.hide();
                }
            }
        }
    }
}