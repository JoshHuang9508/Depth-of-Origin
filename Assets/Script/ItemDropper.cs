using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class ItemDropper : MonoBehaviour
{
    public GameObject itemModel;
    public GameObject coinModel;

    public void Drop(List<Lootings> lootings, int lootMinCoins, int lootMaxCoins, List<GameObject> wreckages)
    {
        DropItems(lootings, null);
        DropCoins(lootMinCoins, lootMaxCoins);
        DropWrackages(wreckages);
        Destroy(gameObject);
    }
    public void Drop(List<Lootings> lootings, int lootMinCoins, int lootMaxCoins)
    {
        DropItems(lootings, null);
        DropCoins(lootMinCoins, lootMaxCoins);
        Destroy(gameObject);
    }
    public void Drop(ItemSO item)
    {
        DropItems(null, item);
        Destroy(gameObject);
    }

    private void DropCoins(int lootMinCoins, int lootMaxCoins)
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

    private void DropItems(List<Lootings> lootings, ItemSO lootng)
    {

        if (lootng != null)
        {
            Debug.Log(lootng.name);
            lootings = new List<Lootings>() { new Lootings(lootng, 100f)};
            Debug.Log(lootings[0].lootings);
        }

        Debug.Log(lootings.Count == 0);

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
                dropItemPickable.InventoryItem = _dropItem.lootings;

                InitialFromItemDropper dropItemInitial = dropItem.GetComponent<InitialFromItemDropper>();
                dropItemInitial.InventoryItem = _dropItem.lootings;

                Rigidbody2D dropItemRb = dropItem.GetComponent<Rigidbody2D>();
                dropItemRb.velocity = new Vector2(Random.Range(-2, 2f) * 10, Random.Range(-2f, 2f) * 10);
            }
        }
    }

    private void DropWrackages(List<GameObject> wreckages) 
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
