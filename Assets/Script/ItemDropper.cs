using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class ItemDropper : MonoBehaviour
{
    [Header("Connect Object")]
    public GameObject itemModel;

    private void Start()
    {
        StartCoroutine(delay(callback =>{
            if (callback) Destroy(gameObject);
        }, 0.2f));
    }

    public void DropCoins(List<Coins> coins = null)
    {
        if (coins.Count == 0)
        {
            return;
        }

        foreach (Coins coin in coins)
        {
            for(int i = 0; i < coin.amount; i++)
            {
                var dropCoin = Instantiate(
                    itemModel,
                    new Vector3(
                        transform.position.x,
                        transform.position.y,
                        transform.position.z),
                    Quaternion.identity,
                    transform.parent
                    );

                Pickable dropItemPickable = dropCoin.GetComponent<Pickable>();
                dropItemPickable.inventoryItem = coin.coins;
                dropItemPickable.pickupDistance = 100;

                InitialFromItemDropper dropItemInitial = dropCoin.GetComponent<InitialFromItemDropper>();
                dropItemInitial.InventoryItem = coin.coins;

                Rigidbody2D dropItemRb = dropCoin.GetComponent<Rigidbody2D>();
                dropItemRb.velocity = new Vector2(Random.Range(-2, 2f) * 10, Random.Range(-2f, 2f) * 10);
            }
        }
    }

    public void DropItems(List<Lootings> lootings)
    {
        if (lootings.Count == 0)
        {
            return;
        }

        foreach (Lootings looting in lootings)
        {
            if(Random.Range(0, 100) < looting.chances)
            {
                for(int i = 0; i < looting.quantity; i++)
                {
                    var dropItem = Instantiate(
                        itemModel,
                        new Vector3(
                            transform.position.x,
                            transform.position.y,
                            transform.position.z),
                        Quaternion.identity,
                        transform.parent
                        );

                    Pickable dropItemPickable = dropItem.GetComponent<Pickable>();
                    dropItemPickable.inventoryItem = looting.lootings;

                    InitialFromItemDropper dropItemInitial = dropItem.GetComponent<InitialFromItemDropper>();
                    dropItemInitial.InventoryItem = looting.lootings;

                    Rigidbody2D dropItemRb = dropItem.GetComponent<Rigidbody2D>();
                    dropItemRb.velocity = new Vector2(Random.Range(-2, 2f) * 10, Random.Range(-2f, 2f) * 10);
                }
            }
        }
    }

    public void DropItems(ItemSO looting, int quantity = 1)
    {
        for (int i = 0; i < quantity; i++)
        {
            var dropItem = Instantiate(
                itemModel,
                new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.position.z),
                Quaternion.identity,
                transform.parent
                );

            Pickable dropItemPickable = dropItem.GetComponent<Pickable>();
            dropItemPickable.inventoryItem = looting;

            InitialFromItemDropper dropItemInitial = dropItem.GetComponent<InitialFromItemDropper>();
            dropItemInitial.InventoryItem = looting;

            Rigidbody2D dropItemRb = dropItem.GetComponent<Rigidbody2D>();
            dropItemRb.velocity = new Vector2(Random.Range(-2, 2f) * 10, Random.Range(-2f, 2f) * 10);
        }
    }

    public void DropWrackages(List<GameObject> wreckages) 
    {
        if (wreckages.Count == 0)
        {
            return;
        }

        foreach (GameObject wreckage in wreckages)
        {
            var Wreckage = Instantiate(
                wreckage, 
                new Vector3(
                    transform.position.x, 
                    transform.position.y, 
                    transform.position.z), 
                Quaternion.identity,
                transform.parent
                );

            Rigidbody2D WreckageRb = Wreckage.GetComponent<Rigidbody2D>();
            WreckageRb.velocity = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
        }
    }

    private IEnumerator delay(System.Action<bool> callback, float delayTime)
    {
        callback(false);
        yield return new WaitForSeconds(delayTime);
        callback(true);
    }
}

[System.Serializable]
public class Lootings
{
    public ItemSO lootings;
    public float chances;
    public int quantity;

    public Lootings(ItemSO item, float chance, int num = 1)
    {
        lootings = item;
        chances = chance;
        quantity = num;
    }
}

[System.Serializable]
public class Coins
{
    public CoinSO coins;
    public int amount;

    public Coins(CoinSO coin, int num)
    {
        coins = coin;
        amount = num;
    }
}
