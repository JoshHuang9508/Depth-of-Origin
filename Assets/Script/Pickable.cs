using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Pickable : MonoBehaviour
{
    [field: SerializeField] public ItemSO inventoryItem { get; set; }
    [field: SerializeField] private InventorySO inventoryData;

    [SerializeField] public int Quantity { get; set; } = 1;

    bool pickEnabler = false;
    bool inRange = false;
    Rigidbody2D currentRb;
    Collider2D target;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(pickup_delay());
        currentRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && pickEnabler)
        {
            int movement_x = (this.transform.position.x - target.transform.position.x <= 0) ? 1 : -1;
            int movement_y = (this.transform.position.y - target.transform.position.y <= 0) ? 1 : -1;
            currentRb.velocity = new Vector3(movement_x * 12, movement_y * 12, 0.0f);
            float distance = Vector2.Distance(this.transform.position, target.transform.position);

            if(distance <= 0.2)
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
                    inventoryData.AddItem(inventoryItem, Quantity);
                }
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
            target = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
        }
    }


    private IEnumerator pickup_delay()
    {
        yield return new WaitForSeconds(1.5f);
        pickEnabler = true;
    }
}
