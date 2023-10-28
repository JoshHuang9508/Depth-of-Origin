using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShopGoodsPage : MonoBehaviour
{
    [SerializeField] private UIInventoryItem itemPrefabs;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private UIDescriptionPage itemDescription;
    [SerializeField] private ItemActionPanel actionPanel;

    public event Action<int> OnDescriptionRequested, OnItemActionRequested;
    List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

    public void InitializeShopGoodsUI(int goodssize)
    {
        for (int i = 0; i < goodssize; i++)
        {
            UIInventoryItem uiItem = Instantiate(itemPrefabs, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            uiItem.OnItemClicked += HandleItemSelection;
            listOfUIItems.Add(uiItem);
        }
    }

    private void HandleItemSelection(UIInventoryItem inventoryItemUI)
    {
        int index = listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
            return;
        OnDescriptionRequested?.Invoke(index);
        OnItemActionRequested?.Invoke(index);
    }

    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (listOfUIItems.Count > itemIndex)
        {
            listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    public void Reselection()
    {
        itemDescription.ResetDescription();
        DeselectAllItems();
    }

    private void DeselectAllItems()
    {
        foreach (UIInventoryItem item in listOfUIItems)
        {
            item.Deselect();
        }
        actionPanel.Toggle(false);
    }

    public void UpdateDescription(int itemIndex, ItemSO item)
    {
        itemDescription.SetDescription(item);
        DeselectAllItems();
        listOfUIItems[itemIndex].Select();
    }

    public void AddAction(string actionName, Action performAction)
    {
        actionPanel.AddButton(actionName, performAction);
    }

    public void ShowItemAction(int itemIndex)
    {
        actionPanel.Toggle(true);
        //actionPanel.transform.position = listOfUIItems[itemIndex].transform.position;
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
