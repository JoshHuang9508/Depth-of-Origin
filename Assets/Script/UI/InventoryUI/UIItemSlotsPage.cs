using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UIItemSlotsPage : MonoBehaviour
{
    [Header("Settings")]
    public ActionType actionType;
    public bool isDragable;

    [Header("Connect Object")]
    public InventorySO inventoryData;
    [SerializeField] private UIInventory inventoryUI;
    [SerializeField] private UIItemSlot itemSlot;
    [SerializeField] private RectTransform contentPanel;

    public List<UIItemSlot> listOfItemSlots = new();

    int currentDraggedItemIndex = -1;


    private void Start()
    {
        inventoryUI = GetComponentInParent<UIInventory>();

        InitializeSlot(inventoryData.Size);
        UpdateBackpack(inventoryData.GetCurrentInventoryState());
    }

    public void InitializeSlot(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            UIItemSlot _itemSlot = Instantiate(itemSlot, Vector3.zero, Quaternion.identity, contentPanel);
            _itemSlot.OnItemClicked += HandleItemSelection;
            _itemSlot.OnItemBeginDrag += HandleBeginDrag;
            _itemSlot.OnItemEndDrag += HandleEndDrag;
            _itemSlot.OnItemDroppedOn += HandleSwap;
            listOfItemSlots.Add(_itemSlot);
        }

        inventoryData.OnInventoryUpdated += UpdateBackpack;
    }





    public void UpdateBackpack(Dictionary<int, InventoryItem> inventoryState)
    {
        ResetAllItems();

        foreach (var item in inventoryState)
        {
            if (listOfItemSlots.Count > item.Key)
            {
                listOfItemSlots[item.Key].SetData(item.Value.item.Image, item.Value.quantity);
            }
        }
    }

    public void HandleBeginDrag(UIItemSlot inventoryItemUI)
    {
        int index = listOfItemSlots.IndexOf(inventoryItemUI);
        InventoryItem inventoryItem = inventoryData.GetItemAt(index);

        if (index != -1 && !inventoryItem.IsEmpty && isDragable)
        {
            HandleItemSelection(inventoryItemUI);
            currentDraggedItemIndex = index;
            inventoryUI.CreateDraggedItem(inventoryItem.item.Image, inventoryItem.quantity);
        }
    }

    public void HandleEndDrag(UIItemSlot inventoryItemUI)
    {
        inventoryUI.DeletDraggedItem();
        currentDraggedItemIndex = -1;
    }

    public void HandleSwap(UIItemSlot inventoryItemUI)
    {
        int index = listOfItemSlots.IndexOf(inventoryItemUI);

        if (index != -1 && isDragable)
        {
            inventoryData.SwapItems(currentDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }
    }

    public void HandleItemSelection(UIItemSlot inventoryItemUI)
    {
        int index = listOfItemSlots.IndexOf(inventoryItemUI);
        InventoryItem inventoryItem = inventoryData.GetItemAt(index);

        if (index != -1 && !inventoryItem.IsEmpty)
        {
            Deselect();
            listOfItemSlots[index].Select();

            inventoryUI.SetDescription(inventoryItem.item, actionType);
            inventoryUI.SetActionBotton(inventoryData, index, actionType);
        }
        else
        {
            Deselect();
        }
    }





    public void Deselect()
    {
        foreach (UIItemSlot item in listOfItemSlots)
        {
            item.Deselect();
        }
        inventoryUI.ClearDescription(actionType);
    }

    public void ResetAllItems()
    {
        foreach (UIItemSlot item in listOfItemSlots)
        {
            item.ResetData();
        }
    }
}
