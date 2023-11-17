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
        [Header("Pages")]
        [SerializeField] public List<GameObject> contentPages;
        [SerializeField] public List<UIDescriptionPage> descriptionPages = new List<UIDescriptionPage>();
        [SerializeField] public List<UIBackpackPage> backpackPages = new List<UIBackpackPage>();

        [Header("Connect Object")]
        [SerializeField] public MouseFollower mouseFollower;
        [SerializeField] public GameObject itemDropper;


        private void OnDisable()
        {
            ClearDescription(InventoryType.All);
            mouseFollower.Toggle(false);
        }

        private void Awake()
        {
            foreach(GameObject contentPage in contentPages)
            {
                if (contentPage.GetComponent<UIBackpackPage>()) backpackPages.Add(contentPage.GetComponent<UIBackpackPage>());
                if (contentPage.GetComponent<UIDescriptionPage>()) descriptionPages.Add(contentPage.GetComponent<UIDescriptionPage>());
            }

            foreach(UIDescriptionPage descriptionPage in descriptionPages)
            {
                descriptionPage.ResetDescription();
            }
            gameObject.SetActive(false);
        }





        public void SetInventoryContent(InventorySO inventoryData, InventoryType inventoryType)
        {
            foreach(UIBackpackPage backpackPage in backpackPages)
            {
                if(backpackPage.inventoryType == inventoryType || backpackPage.inventoryType == InventoryType.All)
                {
                    backpackPage.inventoryData = inventoryData;
                    backpackPage.UpdateBackpack(inventoryData.GetCurrentInventoryState());
                }
            }
            
        }

        public void SetDescription(ItemSO item, InventoryType inventoryType)
        {
            foreach(UIDescriptionPage descriptionPage in descriptionPages)
            {
                if(descriptionPage.inventoryType == inventoryType || descriptionPage.inventoryType == InventoryType.All)
                {
                    descriptionPage.SetDescription(item);
                }
            }
        }

        public void ClearDescription(InventoryType inventoryType)
        {
            foreach (UIDescriptionPage descriptionPage in descriptionPages)
            {
                if (descriptionPage.inventoryType == inventoryType || descriptionPage.inventoryType == InventoryType.All)
                {
                    descriptionPage.ResetDescription();
                    descriptionPage.actionPanel.Toggle(false);
                }
            }
        }

        public void SetActionBotton(InventorySO inventoryData, int itemIndex, InventoryType inventoryType)
        {
            foreach (UIDescriptionPage descriptionPage in descriptionPages)
            {
                if (descriptionPage.inventoryType == inventoryType || descriptionPage.inventoryType == InventoryType.All)
                {
                    InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
                    descriptionPage.actionPanel.Toggle(true);

                    if (!inventoryItem.IsEmpty)
                    {
                        switch (inventoryType)
                        {
                            case InventoryType.BackpackInventory:
                                if (inventoryItem.item is IEquipable) descriptionPage.actionPanel.AddButton("Equip", () => PerformAction(inventoryData, itemIndex, "Equip", inventoryType));
                                if (inventoryItem.item is IConsumeable) descriptionPage.actionPanel.AddButton("Consume", () => PerformAction(inventoryData, itemIndex, "Consume", inventoryType));
                                if (inventoryItem.item is IDestoryableItem) descriptionPage.actionPanel.AddButton("Drop", () => PerformAction(inventoryData, itemIndex, "Drop", inventoryType));
                                break;
                            case InventoryType.BackpackShop:
                                if (inventoryItem.item is ISellable) descriptionPage.actionPanel.AddButton("Sell", () => PerformAction(inventoryData, itemIndex, "Sell", inventoryType));
                                break;
                            case InventoryType.ShopGoods:
                                if (inventoryItem.item is IBuyable) descriptionPage.actionPanel.AddButton("Buy", () => PerformAction(inventoryData, itemIndex, "Buy", inventoryType));
                                break;
                        }
                    }
                }
            }
        }

        public void PerformAction(InventorySO inventoryData, int itemIndex, string actionName, InventoryType inventoryType)
        {
            foreach (UIDescriptionPage descriptionPage in descriptionPages)
            {
                if (descriptionPage.inventoryType == inventoryType || descriptionPage.inventoryType == InventoryType.All)
                {
                    InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
                    IDestoryableItem destoryableItem = inventoryItem.item as IDestoryableItem;
                    IItemAction itemAction = inventoryItem.item as IItemAction;
                    GameObject player = GameObject.FindWithTag("Player");
                    int amountToUse = 0;

                    if (itemAction != null && !inventoryItem.IsEmpty)
                    {
                        switch (actionName)
                        {
                            case "Equip":
                                amountToUse = (inventoryItem.item.IsStackable) ? inventoryItem.quantity : 1;
                                itemAction.SelectAction("Equip", amountToUse, player, inventoryItem.itemState);
                                if (destoryableItem != null) inventoryData.RemoveItem(itemIndex, amountToUse);
                                break;
                            case "Consume":
                                amountToUse = 1;
                                itemAction.SelectAction("Consume", amountToUse, player, inventoryItem.itemState);
                                if (destoryableItem != null) inventoryData.RemoveItem(itemIndex, amountToUse);
                                break;
                            case "Drop":
                                amountToUse = (inventoryItem.item.IsStackable) ? inventoryItem.quantity : 1;
                                ItemDropper ItemDropper = Instantiate(
                                    itemDropper,
                                    new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z),
                                    new Quaternion(0.0f, 0.0f, 0.0f, 0.0f),
                                    GameObject.FindWithTag("Item").transform
                                    ).GetComponent<ItemDropper>();
                                ItemDropper.DropItems(inventoryItem.item, amountToUse);
                                inventoryData.RemoveItem(itemIndex, amountToUse);
                                ClearDescription(inventoryType);
                                break;
                            case "Sell":
                                amountToUse = 1;
                                itemAction.SelectAction("Sell", amountToUse, player, inventoryItem.itemState);
                                if (destoryableItem != null) inventoryData.RemoveItem(itemIndex, amountToUse);
                                break;
                            case "Buy":
                                amountToUse = 1;
                                itemAction.SelectAction("Buy", amountToUse, player, inventoryItem.itemState);
                                if (destoryableItem != null) player.GetComponent<PlayerBehaviour>().inventoryData.AddItem(inventoryItem.item, amountToUse);
                                break;
                        }

                        if (inventoryData.GetItemAt(itemIndex).IsEmpty) ClearDescription(inventoryType);
                    }
                }
            }
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        public void DeletDraggedItem()
        {
            mouseFollower.Toggle(false);
        }
    }

    public enum InventoryType
    {
        BackpackInventory, BackpackShop, ShopGoods, All
    }
}