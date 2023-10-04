using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage inventoryUI;

    [SerializeField] private Inventory_scriptable InventoryData;

    public int inventorySize = 10;
    public void Start()
    {
        inventoryUI.InitializeInventoryUI(inventorySize);
        //InventoryData.initialize();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.show();
                foreach(var item in InventoryData.GetCurrentInventoryState())
                {
                    inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                }
            }
            else
            {
                inventoryUI.hide();
            }
        }
    }
}
