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

        public void PerformAction(int itemIndex, int actionSelection)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return;

            IDestoryableItem destoryableItem = inventoryItem.item as IDestoryableItem;
            if (destoryableItem != null) inventoryData.RemoveItem(itemIndex, 1);

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                switch (actionSelection)
                {
                    case 1:
                        itemAction.SelectAction(gameObject, inventoryItem.itemState, (inventoryItem.item.IsStackable) ? inventoryItem.quantity : 1, 1);
                        break;
                    case 2:
                        itemAction.SelectAction(gameObject, inventoryItem.itemState, (inventoryItem.item.IsStackable) ? inventoryItem.quantity : 1, 2);
                        break;
                }
                
                if (inventoryData.GetItemAt(itemIndex).IsEmpty) inventoryUI.Reselection();
            }
        }

        private void HandleActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                return;
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                inventoryUI.ShowItemAction(itemIndex);

                if(inventoryItem.item is IEquipable)
                {
                    inventoryUI.AddAction(itemAction.SelectAction(1), () => PerformAction(itemIndex, 1));
                }
                if(inventoryItem.item is IActionable)
                {
                    inventoryUI.AddAction(itemAction.SelectAction(2), () => PerformAction(itemIndex, 2));
                }
                
            }
            IDestoryableItem destoryableItem = inventoryItem.item as IDestoryableItem;
            if (destoryableItem != null)
            {
                inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
            }
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