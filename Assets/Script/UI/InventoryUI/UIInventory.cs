using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventory : MonoBehaviour
    {
        [Header("Pages")]
        [SerializeField] private List<GameObject> contentPages;
        [SerializeField] private List<UIDescriptionPage> descriptionPages = new();
        [SerializeField] private List<UIItemSlotsPage> backpackPages = new();
        [SerializeField] private List<UIEquipmentPage> equipmentPages = new();

        [Header("Object Reference")]
        [SerializeField] private MouseFollower mouseFollower;
        [SerializeField] private GameObject itemDropper;


        private void OnDisable()
        {
            ClearDescription(ActionType.All);
            mouseFollower.Toggle(false);
        }

        private void Awake()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                contentPages.Add(transform.GetChild(i).gameObject);
            }

            foreach(GameObject contentPage in contentPages)
            {
                if (contentPage.GetComponent<UIItemSlotsPage>()) backpackPages.Add(contentPage.GetComponent<UIItemSlotsPage>());
                if (contentPage.GetComponent<UIDescriptionPage>()) descriptionPages.Add(contentPage.GetComponent<UIDescriptionPage>());
            }

            foreach(UIDescriptionPage descriptionPage in descriptionPages)
            {
                descriptionPage.ResetDescription();
            }
            gameObject.SetActive(false);
        }





        public void SetInventoryContent(InventorySO inventoryData, ActionType inventoryType)
        {
            foreach(UIItemSlotsPage backpackPage in backpackPages)
            {
                if(backpackPage.actionType == inventoryType || backpackPage.actionType == ActionType.All)
                {
                    backpackPage.inventoryData = inventoryData;
                    backpackPage.UpdateBackpack(inventoryData.GetCurrentInventoryState());
                }
            }
            
        }

        public void SetDescription(ItemSO item, ActionType inventoryType)
        {
            foreach(UIDescriptionPage descriptionPage in descriptionPages)
            {
                if(descriptionPage.actionType == inventoryType || descriptionPage.actionType == ActionType.All)
                {
                    descriptionPage.SetDescription(item);
                }
            }
        }

        public void ClearDescription(ActionType inventoryType)
        {
            foreach (UIDescriptionPage descriptionPage in descriptionPages)
            {
                if (descriptionPage.actionType == inventoryType || descriptionPage.actionType == ActionType.All)
                {
                    descriptionPage.ResetDescription();
                    descriptionPage.actionPanel.Toggle(false);
                }
            }
        }

        public void SetActionBotton(InventorySO inventoryData, int itemIndex, ActionType inventoryType)
        {
            foreach (UIDescriptionPage descriptionPage in descriptionPages)
            {
                if (descriptionPage.actionType == inventoryType || descriptionPage.actionType == ActionType.All)
                {
                    InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
                    descriptionPage.actionPanel.Toggle(true);

                    if (!inventoryItem.IsEmpty)
                    {
                        switch (inventoryType)
                        {
                            case ActionType.BackpackInventory:
                                if (inventoryItem.item is IEquipable) descriptionPage.actionPanel.AddButton("Equip", () => PerformAction(inventoryData, itemIndex, "Equip", inventoryType));
                                if (inventoryItem.item is IConsumeable) descriptionPage.actionPanel.AddButton("Consume", () => PerformAction(inventoryData, itemIndex, "Consume", inventoryType));
                                if (inventoryItem.item is IDroppable) descriptionPage.actionPanel.AddButton("Drop", () => PerformAction(inventoryData, itemIndex, "Drop", inventoryType));
                                break;
                            case ActionType.BackpackShop:
                                if (inventoryItem.item is ISellable) descriptionPage.actionPanel.AddButton("Sell", () => PerformAction(inventoryData, itemIndex, "Sell", inventoryType));
                                break;
                            case ActionType.ShopGoods:
                                if (inventoryItem.item is IBuyable) descriptionPage.actionPanel.AddButton("Buy", () => PerformAction(inventoryData, itemIndex, "Buy", inventoryType));
                                break;
                            case ActionType.BackpackEquipment:
                                if (inventoryItem.item is IUnequipable) descriptionPage.actionPanel.AddButton("Unequip", () => PerformAction(inventoryData, itemIndex, "Unequip", inventoryType));
                                break;
                        }
                    }
                }
            }
        }

        public void PerformAction(InventorySO inventoryData, int itemIndex, string actionName, ActionType inventoryType)
        {
            foreach (UIDescriptionPage descriptionPage in descriptionPages)
            {
                if (descriptionPage.actionType == inventoryType || descriptionPage.actionType == ActionType.All)
                {
                    InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
                    InventorySO playerInventoryData = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>().inventoryData;
                    int amountToUse = 0;

                    if (!inventoryItem.IsEmpty)
                    {
                        switch (actionName)
                        {
                            case "Equip":
                                amountToUse = (inventoryItem.item.IsStackable) ? inventoryItem.quantity : 1;
                                IEquipable ObjectToEquip = inventoryItem.item as IEquipable;
                                ObjectToEquip.EquipObject(amountToUse, playerInventoryData, itemIndex);
                                break;
                            case "Unequip":
                                amountToUse = 1;
                                IUnequipable ObjectToUnequip = inventoryItem.item as IUnequipable;
                                ObjectToUnequip.UnequipObject(amountToUse, inventoryData, itemIndex);
                                break;
                            case "Consume":
                                amountToUse = 1;
                                IConsumeable ObjectToConsume = inventoryItem.item as IConsumeable;
                                ObjectToConsume.ConsumeObject(amountToUse, inventoryData, itemIndex);
                                break;
                            case "Drop":
                                amountToUse = (inventoryItem.item.IsStackable) ? inventoryItem.quantity : 1;
                                IDroppable ObjectToDrop = inventoryItem.item as IDroppable;
                                ObjectToDrop.DropItem(amountToUse, inventoryData, itemIndex);
                                ClearDescription(inventoryType);
                                break;
                            case "Sell":
                                amountToUse = 1;
                                ISellable ObjectToSell = inventoryItem.item as ISellable;
                                ObjectToSell.SellObject(amountToUse, inventoryData, itemIndex);
                                break;
                            case "Buy":
                                amountToUse = 1;
                                IBuyable ObjectToBuy = inventoryItem.item as IBuyable;
                                ObjectToBuy.BuyObject(amountToUse, inventoryData, itemIndex);
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

    public enum ActionType
    {
        BackpackInventory, BackpackEquipment, BackpackShop, ShopGoods, All
    }
}