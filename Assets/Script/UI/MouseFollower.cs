using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private UIInventoryItem inventoryItemUI;

    public void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        inventoryItemUI = GetComponentInChildren<UIInventoryItem>();
    }
    public void SetData(Sprite sprite, int quantity)
    {
        inventoryItemUI.SetData(sprite, quantity);
    }

    private void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform,Input.mousePosition,canvas.worldCamera,out position);
        transform.position = canvas.transform.TransformPoint(position);

    }

    public void toggle(bool val)
    {
        Debug.Log($"Item toggled {val}");
        gameObject.SetActive(val);

    }

}
