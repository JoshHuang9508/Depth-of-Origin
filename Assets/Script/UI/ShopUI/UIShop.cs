using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShop : MonoBehaviour
{
    [SerializeField] private UIItemSlot itemPrefabs;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private UIDescriptionPage itemDescription;
    [SerializeField] private MouseFollower mouseFollower;
    [SerializeField] private ItemActionPanel actionPanel;

    private int currentDraggedItemIndex = -1;

    public event Action<int, string> OnDescriptionRequested, OnItemActionRequested;
    public event Action<int> OnStartDragging;
    public event Action<int, int> OnSwapItems;

    List<UIItemSlot> listOfUIItems = new List<UIItemSlot>();


    private void OnEnable()
    {
        Reselection();
    }

    private void OnDisable()
    {
        actionPanel.Toggle(false);
        ResetDraggedItem();
    }

    private void Awake()
    {
        mouseFollower.Toggle(false);
        itemDescription.ResetDescription();
        gameObject.SetActive(false);
    }

    public void InitializeInventoryUI(int inventorysize)
    {
        for (int i = 0; i < inventorysize; i++)
        {
            UIItemSlot uiItem = Instantiate(itemPrefabs, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemDroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;

            listOfUIItems.Add(uiItem);
        }
    }

    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (listOfUIItems.Count > itemIndex)
        {
            listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    public void UpdateDescription(int itemIndex, ItemSO item)
    {
        itemDescription.SetDescription(item);
        DeselectAllItems();
        listOfUIItems[itemIndex].Select();
    }

    private void HandleEndDrag(UIItemSlot inventoryItemUI)
    {
        ResetDraggedItem();
    }

    private void HandleSwap(UIItemSlot inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }
        OnSwapItems?.Invoke(currentDraggedItemIndex, index);
        HandleItemSelection(inventoryItemUI);
    }

    private void HandleBeginDrag(UIItemSlot inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
            return;
        currentDraggedItemIndex = index;
        HandleItemSelection(inventoryItemUI);
        OnStartDragging?.Invoke(index);
        OnItemActionRequested?.Invoke(index, "Backpack");
    }

    private void HandleItemSelection(UIItemSlot inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
            return;
        OnDescriptionRequested?.Invoke(index, "Backpack");
        OnItemActionRequested?.Invoke(index, "Backpack");
    }

    public void CreateDraggedItem(Sprite sprite, int quantity)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, quantity);
    }

    private void ResetDraggedItem()
    {
        mouseFollower.Toggle(false);
        currentDraggedItemIndex = -1;
    }

    public void Reselection()
    {
        itemDescription.ResetDescription();
        DeselectAllItems();
    }

    private void DeselectAllItems()
    {
        foreach (UIItemSlot item in listOfUIItems)
        {
            item.Deselect();
        }
        actionPanel.Toggle(false);
    }

    public void ShowItemAction()
    {
        actionPanel.Toggle(true);
    }

    public void AddAction(string actionName, Action performAction)
    {
        actionPanel.AddButton(actionName, performAction);
    }

    public void ResetAllItems()
    {
        foreach (var item in listOfUIItems)
        {
            item.ResetData();
            item.Deselect();
        }
    }
}
