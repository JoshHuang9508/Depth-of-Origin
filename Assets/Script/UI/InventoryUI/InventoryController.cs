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
        [SerializeField] private UIInventory inventoryUI;
        [SerializeField] private InventorySO inventoryData;

        public void Start()
        {
            //prepare UI
            inventoryUI.InitializeInventoryUI(inventoryData.Size);

            //action setup
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleActionRequest;
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.Image, item.Value.quantity);
            }
        }

        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.Reselection();
        }

        public void PerformAction(int itemIndex, string actionName)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            IDestoryableItem destoryableItem = inventoryItem.item as IDestoryableItem;
            IItemAction itemAction = inventoryItem.item as IItemAction;
            int amountToUse = 0;

            if (itemAction != null && !inventoryItem.IsEmpty)
            {
                switch (actionName)
                {
                    case "Equip":
                        amountToUse = (inventoryItem.item.IsStackable) ? inventoryItem.quantity : 1;
                        itemAction.SelectAction("Equip", amountToUse, gameObject, inventoryItem.itemState);
                        if (destoryableItem != null) inventoryData.RemoveItem(itemIndex, amountToUse);
                        break;
                    case "Consume":
                        amountToUse = 1;
                        itemAction.SelectAction("Consume", amountToUse, gameObject, inventoryItem.itemState);
                        if (destoryableItem != null) inventoryData.RemoveItem(itemIndex, amountToUse);
                        break;
                }
                
                if (inventoryData.GetItemAt(itemIndex).IsEmpty) inventoryUI.Reselection();
            }
            return;
        }

        private void HandleActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            IDestoryableItem destoryableItem = inventoryItem.item as IDestoryableItem;
            IItemAction itemAction = inventoryItem.item as IItemAction;

            if (itemAction != null && !inventoryItem.IsEmpty)
            {
                inventoryUI.ShowItemAction(itemIndex);

                if(inventoryItem.item is IEquipable)
                {
                    inventoryUI.AddAction("Equip", () => PerformAction(itemIndex, "Equip"));
                }
                if(inventoryItem.item is IConsumeable)
                {
                    inventoryUI.AddAction("Consume", () => PerformAction(itemIndex, "Consume"));
                }
                if (destoryableItem != null)
                {
                    inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
                }
            }
            return;
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem =inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return;

            inventoryUI.CreateDraggedItem(inventoryItem.item.Image, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
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
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UpdateData(item.Key, item.Value.item.Image, item.Value.quantity);
                    }
                }
                else inventoryUI.hide();
            }
        }
    }
}