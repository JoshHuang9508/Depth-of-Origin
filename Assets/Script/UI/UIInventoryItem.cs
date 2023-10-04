using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UIInventoryItem : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IEndDragHandler,IDropHandler,IDragHandler
{
    [SerializeField] private Image inventoryItemUIImage;
    [SerializeField] private Image borderImage;
    [SerializeField] private TMP_Text quantityText;

    public event Action<UIInventoryItem> OnItemClicked,OnItemDropped,OnItemBeginDrag,OnItemEndDrag,OnRightMouseBtnClicked;
    private bool empty = true;

    private void Awake()
    {
        ResetData();
        Deselect();
    }

    public void ResetData()
    {
        this.inventoryItemUIImage.gameObject.SetActive(false);
        this.empty = true;
    }
    public void Deselect()
    {
        this.borderImage.enabled = false;
    }

    public void SetData(Sprite sprite,int quantity)
    {
        this.inventoryItemUIImage.gameObject.SetActive(true);
        this.inventoryItemUIImage.sprite = sprite;
        this.quantityText.text = quantity + "";
        this.empty = false;
    }

    public void Select()
    {
        borderImage.enabled = true;
    }
    

    public void OnPointerClick(PointerEventData pointerData)
    {
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClicked?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (empty)
        {
            return;
        }
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDropped?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}
