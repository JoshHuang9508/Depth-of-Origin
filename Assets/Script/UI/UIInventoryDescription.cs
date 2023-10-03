using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventoryDescription : MonoBehaviour
{
    [SerializeField] private Image inventoryItemUIImage;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;

    private void Awake()
    {
        ResetDescription();
    }

    public void ResetDescription()
    {
        this.inventoryItemUIImage.gameObject.SetActive(false);
        this.title.text = "";
        this.description.text = "";
    }

    public void SetDescription(Sprite sprite,string inventoryItemUIname,string inventoryItemUIDescription)
    {
        this.inventoryItemUIImage.gameObject.SetActive(true);
        this.inventoryItemUIImage.sprite = sprite;
        this.title.text = inventoryItemUIname;
        this.description.text = inventoryItemUIDescription;
    }
}
