using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Pickable : MonoBehaviour
{
    [Header("Setting")]
    public ItemSO inventoryItem;
    public int quantity = 1;
    public float pickupDistance;

    [Header("Status")]
    public bool pickEnabler = false;
    public bool isInventoryFull;
    public bool storable;

    [Header("Connect Object")]
    public InventorySO inventoryData;

    Rigidbody2D currentRb;
    GameObject target;
    

    void Start()
    {
        currentRb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player");

        StartCoroutine(pickup_delay());
    }

    void Update()
    {
        storable = inventoryItem.isStorable;

        isInventoryFull = inventoryData.IsInventoryFull() && inventoryData.IsCertainItemFull(inventoryItem.ID);

        if ((storable ? !isInventoryFull : true) && pickEnabler && Vector2.Distance(target.transform.position, this.transform.position) < pickupDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 50 * Time.deltaTime);

            if(Vector2.Distance(this.transform.position, target.transform.position) <= 0.8)
            {
                if(inventoryItem is CoinSO)
                {
                    target.GetComponent<PlayerBehaviour>().currentCoinAmount += 1;
                }
                else if(inventoryItem is KeySO)
                {
                    target.GetComponent<PlayerBehaviour>().keyList.Add(new PlayerBehaviour.KeyList((KeySO)inventoryItem, 1));
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
