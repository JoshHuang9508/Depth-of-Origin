using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventory : MonoBehaviour
    {
        [Header("Inventory Data")]
        [SerializeField] private InventorySO inventoryData;

        [Header("Pages")]
        [SerializeField] public UIDescriptionPage descriptionPage;
        [SerializeField] public UIBackpackPage backpackPage;
        [SerializeField] public MouseFollower mouseFollower;
        [SerializeField] public ItemActionPanel actionPanel;


        private void OnEnable()
        {
            UpdateBackpack(inventoryData.GetCurrentInventoryState());
            ClearDescription();
        }

        private void OnDisable()
        {
            actionPanel.Toggle(false);
            mouseFollower.Toggle(false);
        }

        private void Awake()
        {
            inventoryData = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>().inventoryData;

            mouseFollower.Toggle(false);
            descriptionPage.ResetDescription();
            gameObject.SetActive(false);

            inventoryData.OnInventoryUpdated += UpdateBackpack;

            backpackPage.InitializeBackpackSlot(inventoryData.Size);
        }





        private void UpdateBackpack(Dictionary<int, InventoryItem> inventoryState)
        {
            ResetAllItems();
            foreach (var item in inventoryState)
            {
                if (backpackPage.listOfItemSlots.Count > item.Key)
                {
                    backpackPage.listOfItemSlots[item.Key].SetData(item.Value.item.Image, item.Value.quantity);
                }
            }
        }

        public void UpdateDescription(int itemIndex, ItemSO item)
        {
            descriptionPage.SetDescription(item);
            Deselect();
            backpackPage.listOfItemSlots[itemIndex].Select();
        }





        public void SetDescription(int itemIndex, string type)
        {
            switch (type)
            {
                case "Backpack":
                    InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
                    if (inventoryItem.IsEmpty)
                    {
                        ClearDescription();
                        return;
                    }
                    UpdateDescription(itemIndex, inventoryItem.item);
                    break;
            }
        }

        public void SetActionBotton(int itemIndex, string type)
        {
            switch (type)
            {
                case "Backpack":
                    InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
                    if (!inventoryItem.IsEmpty)
                    {
                        actionPanel.Toggle(true);

                        if (inventoryItem.item is IEquipable)
                        {
                            actionPanel.AddButton("Equip", () => PerformAction(itemIndex, "Equip"));
                        }
                        if (inventoryItem.item is IConsumeable)
                        {
                            actionPanel.AddButton("Consume", () => PerformAction(itemIndex, "Consume"));
                        }
                        if (inventoryItem.item is IDestoryableItem)
                        {
                            actionPanel.AddButton("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
                        }
                    }
                    break;
            }
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

                if (inventoryData.GetItemAt(itemIndex).IsEmpty) ClearDescription();
            }
            return;
        }

        public void ClearDescription()
        {
            descriptionPage.ResetDescription();
            Deselect();
        }

        public void Deselect()
        {
            foreach (UIItemSlot item in backpackPage.listOfItemSlots)
            {
                item.Deselect();
            }
            actionPanel.Toggle(false);
        }

        public void ResetAllItems()
        {
            foreach(var item in backpackPage.listOfItemSlots)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            ClearDescription();
        }
    }
}