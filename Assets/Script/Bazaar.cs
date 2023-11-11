using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;
using Inventory.UI;

public class Bazaar : MonoBehaviour
{
    public UIInventory shopUI;
    public InventorySO shopData;

    public List<InventoryItem> shopGoodsList = new List<InventoryItem>();


    private void Update()
    {
        try
        {
            shopUI = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>().shopUI;
        }
        catch
        {

        }
    }

    public void OpenShop()
    {
        shopData.initialize();
        foreach (InventoryItem item in shopGoodsList)
        {
            if (item.IsEmpty)
                continue;
            shopData.AddItem(item);
        }

        shopUI.SetInventoryContent(GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>().inventoryData, InventoryType.BackpackShop);
        shopUI.SetInventoryContent(shopData, InventoryType.ShopGoods);
        shopUI.gameObject.SetActive(!shopUI.gameObject.activeInHierarchy);
    }

    public void CloseShop()
    {
        shopUI.gameObject.SetActive(false);
    }
}
