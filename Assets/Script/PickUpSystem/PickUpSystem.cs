using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableSystem : MonoBehaviour
{
    [SerializeField] private InventorySO inventoryData;

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        Pickable item = collision.GetComponent<Pickable>();
        if(item != null)
        {
            inventoryData.AddItem(item.InventoryItem, item.Quantity);
        }
    }*/

    public void GetItems(ItemSO itemSO, int Quantity)
    {
        inventoryData.AddItem(itemSO, Quantity);
    }
}
