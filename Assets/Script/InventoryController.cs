using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage inventoryUI;

    public int inventorySize = 8;
    public void Start()
    {
        inventoryUI.InitializeInventoryUI(inventorySize);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.show();
            }
            else
            {
                inventoryUI.hide();
            }
        }
    }
}
