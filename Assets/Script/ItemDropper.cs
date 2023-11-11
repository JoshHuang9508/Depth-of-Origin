using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class ItemDropper : MonoBehaviour
{
    public GameObject itemModel;
    public GameObject coinModel;

    private void Start()
    {
        StartCoroutine(delay(callback =>{
            if (callback) Destroy(gameObject);
        }, 0.2f));
    }

    public void DropCoins(int lootMinCoins, int lootMaxCoins)
    {
        for (int i = 0; i < Random.Range(lootMinCoins, lootMaxCoins + 1); i++)
        {
            var dropItem = Instantiate(
                coinModel,
                new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.position.z),
                Quaternion.identity,
                transform.parent
                );

            dropItem.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2, 2f) * 10, Random.Range(-2f, 2f) * 10);
        }
    }

    public void DropItems(List<Lootings> lootings = null, ItemSO lootng = null)
    {

        if (lootng != null)
        {
            Debug.Log(lootng.name);
            lootings = new List<Lootings>() { new Lootings(lootng, 100f)};
            Debug.Log(lootings[0].lootings);
        }

        if (lootings.Count == 0)
        {
            return;
        }

        foreach (Lootings _dropItem in lootings)
        {
            if(Random.Range(0, 100) < _dropItem.chances)
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
                dropItemPickable.inventoryItem = _dropItem.lootings;

                InitialFromItemDropper dropItemInitial = dropItem.GetComponent<InitialFromItemDropper>();
                dropItemInitial.InventoryItem = _dropItem.lootings;

                Rigidbody2D dropItemRb = dropItem.GetComponent<Rigidbody2D>();
                dropItemRb.velocity = new Vector2(Random.Range(-2, 2f) * 10, Random.Range(-2f, 2f) * 10);
            }
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

    public Lootings(ItemSO item, float chance)
    {
        lootings = item;
        chances = chance;
    }
}
