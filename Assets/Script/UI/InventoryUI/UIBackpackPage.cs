using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UIBackpackPage : MonoBehaviour
{
    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private UIInventory inventoryUI;
    [SerializeField] private UIItemSlot itemSlot;
    [SerializeField] private RectTransform contentPanel;

    public List<UIItemSlot> listOfItemSlots = new List<UIItemSlot>();

    int currentDraggedItemIndex = -1;


    private void Start()
    {
        inventoryData = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>().inventoryData;
        inventoryUI = GetComponentInParent<UIInventory>();
    }

    public void InitializeBackpackSlot(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            UIItemSlot _itemSlot = Instantiate(itemSlot, Vector3.zero, Quaternion.identity);
            _itemSlot.transform.SetParent(contentPanel);
            _itemSlot.OnItemClicked += HandleItemSelection;
            _itemSlot.OnItemBeginDrag += HandleBeginDrag;
            _itemSlot.OnItemEndDrag += HandleEndDrag;
            _itemSlot.OnItemDroppedOn += HandleSwap;
            listOfItemSlots.Add(_itemSlot);
        }
    }

    public void HandleBeginDrag(UIItemSlot inventoryItemUI)
    {
        int index = listOfItemSlots.IndexOf(inventoryItemUI);
        InventoryItem inventoryItem = inventoryData.GetItemAt(index);

        if (index != -1 && !inventoryItem.IsEmpty)
        {
            HandleItemSelection(inventoryItemUI);
            currentDraggedItemIndex = index;
            inventoryUI.CreateDraggedItem(inventoryItem.item.Image, inventoryItem.quantity);
        }
    }

    public void HandleEndDrag(UIItemSlot inventoryItemUI)
    {
        inventoryUI.mouseFollower.Toggle(false);
        currentDraggedItemIndex = -1;
    }

    public void HandleSwap(UIItemSlot inventoryItemUI)
    {
        int index = listOfItemSlots.IndexOf(inventoryItemUI);

        if (index != -1)
        {
            inventoryData.SwapItems(currentDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }
    }

    public void HandleItemSelection(UIItemSlot inventoryItemUI)
    {
        int index = listOfItemSlots.IndexOf(inventoryItemUI);

        if (index != -1)
        {
            inventoryUI.SetDescription(index, "Backpack");
            inventoryUI.SetActionBotton(index, "Backpack");
        }
    }
}
