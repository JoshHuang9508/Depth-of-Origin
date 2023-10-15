using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class ItemDropper : MonoBehaviour
{
    public GameObject itemModel;
    public GameObject coinModel;

    public void DropItems(List<Lootings> lootings, int lootMinItems, int lootMaxItems)
    {
        for (int i = 0; i < Random.Range(lootMinItems, lootMaxItems + 1); i++)
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

        if (lootings.Count == 0)
        {
            Destroy(gameObject);
            return;
        }

        foreach (Lootings _dropItem in lootings)
        {
            if(Random.Range(0, 100) <= _dropItem.chances)
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

        Destroy(gameObject);
    }

    public void DropWrackages(List<GameObject> wreckages) 
    {
        if (wreckages.Count == 0)
        {
            Destroy(gameObject);
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
public struct Lootings
{
    public ItemSO lootings;
    public float chances;
}
