using Inventory.Model;
using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UDP;

public class ShopController : MonoBehaviour
{
    [SerializeField] private UIShop shopUI;
    [SerializeField] private InventorySO inventoryData;

    public void Start()
    {
        //prepare UI
        shopUI.InitializeInventoryUI(inventoryData.Size);

        //action setup
        shopUI.OnDescriptionRequested += HandleDescriptionRequest;
        shopUI.OnSwapItems += HandleSwapItems;
        shopUI.OnStartDragging += HandleDragging;
        shopUI.OnItemActionRequested += HandleActionRequest;
        inventoryData.OnInventoryUpdated += UpdateInventoryUI;
    }

    private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
    {
        shopUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            shopUI.UpdateData(item.Key, item.Value.item.Image, item.Value.quantity);
        }
    }

    private void DropItem(int itemIndex, int quantity)
    {
        inventoryData.RemoveItem(itemIndex, quantity);
        shopUI.Reselection();
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
                case "Sell":
                    amountToUse = 1;
                    itemAction.SelectAction("Sell", amountToUse, gameObject, inventoryItem.itemState);
                    if (destoryableItem != null) inventoryData.RemoveItem(itemIndex, amountToUse);
                    break;
                case "Consume":
                    amountToUse = 1;
                    itemAction.SelectAction("Consume", amountToUse, gameObject, inventoryItem.itemState);
                    if (destoryableItem != null) inventoryData.RemoveItem(itemIndex, amountToUse);
                    break;
            }

            if (inventoryData.GetItemAt(itemIndex).IsEmpty) shopUI.Reselection();
        }
        return;
    }

    private void HandleActionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);

        if (!inventoryItem.IsEmpty)
        {
            shopUI.ShowItemAction(itemIndex);

            if (inventoryItem.item is ISellable)
            {
                shopUI.AddAction("Sell", () => PerformAction(itemIndex, "Sell"));
            }
            if (inventoryItem.item is IConsumeable)
            {
                shopUI.AddAction("Consume", () => PerformAction(itemIndex, "Consume"));
            }
        }
        return;
    }

    private void HandleDragging(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty) return;

        shopUI.CreateDraggedItem(inventoryItem.item.Image, inventoryItem.quantity);
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
            shopUI.Reselection();
            return;
        }

        ItemSO item = inventoryItem.item;
        shopUI.UpdateDescription(itemIndex, item);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (shopUI.isActiveAndEnabled == false)
            {
                shopUI.show();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    shopUI.UpdateData(item.Key, item.Value.item.Image, item.Value.quantity);
                }
            }
            else shopUI.hide();
        }
    }
}
