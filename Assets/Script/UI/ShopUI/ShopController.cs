using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UDP;

public class ShopController : MonoBehaviour
{
    [SerializeField] private UIShop shopUI;
    [SerializeField] private UIShopGoodsPage shopgoodsPageUI;//shop page
    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private ShopSO shopData;
    public List<ShopItem> initialItems;

    public void Start()
    {
        //prepare UI
        shopUI.InitializeInventoryUI(inventoryData.Size);
        shopgoodsPageUI.InitializeShopGoodsUI(shopData.Size);

        //backpack action setup
        shopUI.OnDescriptionRequested += HandleDescriptionRequest;
        shopUI.OnSwapItems += HandleSwapItems;
        shopUI.OnStartDragging += HandleDragging;
        shopUI.OnItemActionRequested += HandleActionRequest;
        inventoryData.OnInventoryUpdated += UpdateInventoryUI;

        //shop goods action setup
        shopgoodsPageUI.OnDescriptionRequested += HandleDescriptionRequest;
        shopgoodsPageUI.OnItemActionRequested += HandleActionRequest;
        shopData.OnShopUpdated += UpdateShopUI;

        //initial
        shopData.initialize();
        foreach (ShopItem item in initialItems)
        {
            if (item.IsEmpty)
                continue;
            shopData.AddItem(item);
        }
    }

    private void UpdateShopUI(Dictionary<int, ShopItem> shopState)
    {
        shopgoodsPageUI.ResetAllItems();
        foreach (var item in shopState) 
        {
            shopgoodsPageUI.UpdateData(item.Key, item.Value.item.Image, item.Value.quantity);
        }
    }

    private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
    {
        shopUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            shopUI.UpdateData(item.Key, item.Value.item.Image, item.Value.quantity);
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
                case "Sell":
                    amountToUse = 1;
                    itemAction.SelectAction("Sell", amountToUse, gameObject, inventoryItem.itemState);
                    if (destoryableItem != null) inventoryData.RemoveItem(itemIndex, amountToUse);
                    break;
                case "Buy":
                    amountToUse = 1;
                    itemAction.SelectAction("Buy", amountToUse, gameObject, inventoryItem.itemState);
                    if (destoryableItem != null) inventoryData.AddItem(itemIndex, amountToUse); shopData.RemoveItem(itemIndex, amountToUse);
                    break;
            }

            if (inventoryData.GetItemAt(itemIndex).IsEmpty) shopUI.Reselection();
        }
        return;
    }

    private void HandleActionRequest(int itemIndex)
    {
        try
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (!inventoryItem.IsEmpty)
            {
                shopUI.ShowItemAction(itemIndex);

                if (inventoryItem.item is ISellable)
                {
                    shopUI.AddAction("Sell", () => PerformAction(itemIndex, "Sell"));
                }
            }
            return;
        }
        catch
        {
            ShopItem shopItem = shopData.GetItemAt(itemIndex);
            if (!shopItem.IsEmpty)
            {
                shopgoodsPageUI.ShowItemAction(itemIndex);

                if(shopItem.item is IBuyable)
                {
                    shopgoodsPageUI.AddAction("Buy", () => PerformAction(itemIndex, "Buy"));
                }
            }
            return;
        }
        
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
        try
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
        catch
        {
            ShopItem shopItem = shopData.GetItemAt(itemIndex);
            if (shopItem.IsEmpty)
            {
                shopgoodsPageUI.Reselection();
                return;
            }
            ItemSO item = shopItem.item;
            shopgoodsPageUI.UpdateDescription(itemIndex, item);
        }
        
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

