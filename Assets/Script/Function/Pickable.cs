using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Pickable : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private int quantity = 1;
    public float pickupDistance;

    [Header("Object Reference")]
    public ItemSO inventoryItem;
    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private PlayerBehaviour player;

    [Header("Stats")]
    public bool pickEnabler = false;
    public bool isInventoryFull;
    public bool storable;
    

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        StartCoroutine(pickup_delay());
    }

    void Update()
    {
        storable = inventoryItem.isStorable;

        isInventoryFull = inventoryData.IsInventoryFull() && inventoryData.IsCertainItemFull(inventoryItem.ID);

        if (player.behaviourEnabler && (storable ? !isInventoryFull : true) && pickEnabler && Vector2.Distance(player.transform.position, this.transform.position) < pickupDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 30 * Time.deltaTime);

            if(Vector2.Distance(this.transform.position, player.transform.position) <= 0.2)
            {
                if(inventoryItem is CoinSO)
                {
                    player.currentCoinAmount += ((CoinSO)inventoryItem).coinAmount;
                }
                else if(inventoryItem is KeySO)
                {
                    player.keyList.Add(new PlayerBehaviour.KeyList {key = (KeySO)inventoryItem, quantity = 1});
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
