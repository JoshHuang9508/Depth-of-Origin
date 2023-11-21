using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;
using Inventory.UI;

public class Bazaar : MonoBehaviour
{
    [Header("Setting")]
    public List<InventoryItem> shopGoodsList = new();

    [Header("Connect Object")]
    public UIInventory shopUI;
    public InventorySO shopData;

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

        shopUI.SetInventoryContent(GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>().inventoryData, ActionType.BackpackShop);
        shopUI.SetInventoryContent(shopData, ActionType.ShopGoods);
        shopUI.gameObject.SetActive(!shopUI.gameObject.activeInHierarchy);
        Time.timeScale = shopUI.gameObject.activeInHierarchy ? 0 : 1;
    }

    public void CloseShop()
    {
        Time.timeScale = 1;
        shopUI.gameObject.SetActive(false);
    }
}
