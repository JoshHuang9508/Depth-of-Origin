using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{

    [SerializeField] private UIInventoryItem itemprefab;

    [SerializeField] private RectTransform contentPanel;

    [SerializeField] private UIInventoryDescription itemDescription;

    [SerializeField] private MouseFollower mouseFollower;
    
    List<UIInventoryItem> listofUIitems = new List<UIInventoryItem>();

    public Sprite image,image2;
    public int quantity;
    public string title, description;

    private int currentDraggedItemIndex = -1;

    private void Awake()
    {
        hide();
        mouseFollower.toggle(false);
        itemDescription.ResetDescription();
    }
    public void InitializeInventoryUI(int inventorysize)
    {
        for(int i = 0; i < inventorysize; i++)
        {
            UIInventoryItem uiItem = Instantiate(itemprefab , Vector3.zero , Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            listofUIitems.Add(uiItem);
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemEndDrag += HandleItemEndDrag;
            uiItem.OnRightMouseBtnClicked += HandleShowItemActions;
            uiItem.OnItemDropped += Handleswap;
            
        }
    }

    private void Handleswap(UIInventoryItem item)
    {
        int index = listofUIitems.IndexOf(item);
        if (index == -1)
        {
            mouseFollower.toggle(false);
            currentDraggedItemIndex = -1;
            return;
        }
        listofUIitems[currentDraggedItemIndex].SetData(index == 0 ? image : image2, quantity);
        listofUIitems[index].SetData(currentDraggedItemIndex == 0 ? image : image2, quantity);
        currentDraggedItemIndex = -1;
    }

    private void HandleShowItemActions(UIInventoryItem item)
    {
        
    }

    private void HandleItemEndDrag(UIInventoryItem item)
    {
        mouseFollower.toggle(false);
    }

    private void HandleBeginDrag(UIInventoryItem item)
    {
        int index = listofUIitems.IndexOf(item);
        if (index == -1)
        {
            return;
        }
        currentDraggedItemIndex = index;
        mouseFollower.toggle(true);
        mouseFollower.SetData(index == 0 ? image:image2,quantity);
    }

    private void HandleItemSelection(UIInventoryItem item)
    {
        itemDescription.SetDescription(image, title, description);
        listofUIitems[0].Select();
    }

    public void show()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription();
        listofUIitems[0].SetData(image, quantity);
        listofUIitems[1].SetData(image2, quantity);
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }

   
}
