using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class ItemDropper : MonoBehaviour
{
    public GameObject item_model;

    public void DropItems(List<ItemSO> lootings, int lootMinItems, int lootMaxItems)
    {
        if (lootings.Count == 0)
        {
            Destroy(gameObject);
            return;
        }
            

        for (int i = 0; i <= Random.Range(lootMinItems, lootMaxItems) - 1; i++)
        {
            int randomIndex = Random.Range(0, lootings.Count);

            var dropItem = Instantiate(
                item_model,
                new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.position.z),
                Quaternion.identity
                );
            dropItem.transform.parent = transform.parent;
            Pickable dropItem_pickable = dropItem.GetComponent<Pickable>();
            dropItem_pickable.InventoryItem = lootings[randomIndex];

            InitialFromItemDropper dropItem_initial = dropItem.GetComponent<InitialFromItemDropper>();
            dropItem_initial.InventoryItem = lootings[randomIndex];

            Rigidbody2D dropItemRb = dropItem.GetComponent<Rigidbody2D>();
            dropItemRb.velocity = new Vector2(Random.Range(-2, 2f) * 10, Random.Range(-2f, 2f) * 10);
        }
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
                Quaternion.identity);
            Wreckage.transform.parent = transform.parent;
            Rigidbody2D WreckageRb = Wreckage.GetComponent<Rigidbody2D>();
            WreckageRb.velocity = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
        }
    }

}
