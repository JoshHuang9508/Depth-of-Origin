using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Pickable : MonoBehaviour
{
    [Header("Item Object")]
    public ItemSO inventoryItem;

    [Header("Setting")]
    public int quantity = 1;
    public float pickupDistance;

    [Header("Status")]
    public bool pickEnabler = false;
    public bool isInventoryFull;
    public bool storable;

    [Header("Connect Object")]
    public InventorySO inventoryData;

    PlayerBehaviour target;
    

    void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        StartCoroutine(pickup_delay());
    }

    void Update()
    {
        storable = inventoryItem.isStorable;

        isInventoryFull = inventoryData.IsInventoryFull() && inventoryData.IsCertainItemFull(inventoryItem.ID);

        if (target.behaviourEnabler && (storable ? !isInventoryFull : true) && pickEnabler && Vector2.Distance(target.transform.position, this.transform.position) < pickupDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 50 * Time.deltaTime);

            if(Vector2.Distance(this.transform.position, target.transform.position) <= 0.8)
            {
                if(inventoryItem is CoinSO)
                {
                    target.currentCoinAmount += ((CoinSO)inventoryItem).coinAmount;
                }
                else if(inventoryItem is KeySO)
                {
                    target.keyList.Add(new PlayerBehaviour.KeyList {key = (KeySO)inventoryItem, quantity = 1});
                }
                else
                {
                    inventoryData.AddItem(inventoryItem, quantity);
                }
                Destroy(gameObject);
            }
        }
    }


    private IEnumerator pickup_delay()
    {
        yield return new WaitForSeconds(1.5f);
        pickEnabler = true;
    }
}
