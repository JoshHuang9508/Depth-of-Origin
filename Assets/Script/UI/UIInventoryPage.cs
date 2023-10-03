using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{

    [SerializeField] private UIInventoryItem inventoryItemUIprefab;

    [SerializeField] private RectTransform contentPanel;

    [SerializeField] private UIInventoryDescription inventoryItemUIDescription;

    [SerializeField] private MouseFollower mouseFollower;
    
    List<UIInventoryItem> listofUIinventoryItemUIs = new List<UIInventoryItem>();



    private int currentDraggedItemIndex = -1;

    public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;

    public event Action<int, int> OnSwapItems;

    private void Awake()
    {
        hide();
        mouseFollower.toggle(false);
        inventoryItemUIDescription.ResetDescription();
    }
    public void InitializeInventoryUI(int inventorysize)
    {
        for(int i = 0; i < inventorysize; i++)
        {
            UIInventoryItem uiItem = Instantiate(inventoryItemUIprefab , Vector3.zero , Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            listofUIinventoryItemUIs.Add(uiItem);
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemEndDrag += HandleItemEndDrag;
            uiItem.OnRightMouseBtnClicked += HandleShowItemActions;
            uiItem.OnItemDropped += Handleswap;
            
        }
    }

    private void Handleswap(UIInventoryItem inventoryItemUI)
    {
        int index = listofUIinventoryItemUIs.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }
        OnSwapItems?.Invoke(currentDraggedItemIndex, index);
    }

    private void ResetDraggedItem()
    {
        mouseFollower.toggle(false);
        currentDraggedItemIndex = -1;
    }

    public void UpdateData(int inventoryItemUIIndex,Sprite inventoryItemUIImage , int inventoryItemUIQuantity)
    {
        if(listofUIinventoryItemUIs.Count > inventoryItemUIQuantity)
        {
            listofUIinventoryItemUIs[inventoryItemUIIndex].SetData(inventoryItemUIImage, inventoryItemUIQuantity);
        }
    }

    private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
    {
        
    }

    private void HandleItemEndDrag(UIInventoryItem inventoryItemUI)
    {
        ResetDraggedItem();
    }

    private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
    {
        int index = listofUIinventoryItemUIs.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }
        currentDraggedItemIndex = index;
        HandleItemSelection(inventoryItemUI);
        OnStartDragging?.Invoke(index);
    }

    public void CreateDraggedItem(Sprite sprite, int quantity)
    {
        mouseFollower.toggle(true);
        mouseFollower.SetData(sprite, quantity);
    }

    private void HandleItemSelection(UIInventoryItem inventoryItemUI)
    {
        int index = listofUIinventoryItemUIs.IndexOf(inventoryItemUI);
        if(index == -1)
        {
            return;
        }
        OnDescriptionRequested?.Invoke(index);
    }

    public void show()
    {
        gameObject.SetActive(true);
        ResetSelection();
    }

    private void ResetSelection()
    {
        inventoryItemUIDescription.ResetDescription();
        DeselectAllItems();
    }

    private void DeselectAllItems()
    {
        foreach(UIInventoryItem item in listofUIinventoryItemUIs)
        {
            item.Deselect();
        }
    }

    public void hide()
    {
        gameObject.SetActive(false);
        ResetDraggedItem();
    }

   
}
