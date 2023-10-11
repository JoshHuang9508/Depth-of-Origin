using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public GameObject skull;

    public void DropItems(List<GameObject> lootings, List<GameObject> wreckages, int lootMinItems, int lootMaxItems, string dropType)
    {
        for (int i = 0; i <= Random.Range(lootMinItems, lootMaxItems) - 1; i++)
        {
            var dropItem = Instantiate(
                lootings[Random.Range(0, lootings.Count)],
                new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.position.z
                    ),
                Quaternion.identity
                );
            dropItem.transform.parent = transform.parent;
            Rigidbody2D dropItemRb = dropItem.GetComponent<Rigidbody2D>();
            dropItemRb.velocity = new Vector2(Random.Range(-2, 2f) * 10, Random.Range(-2f, 2f) * 10);
        }

        if(dropType == "Enemy" || dropType == "BreakableObject")
        {
            foreach (GameObject wreckage in wreckages)
            {
                var Wreckage = Instantiate(wreckage, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                Wreckage.transform.parent = transform.parent;
                Rigidbody2D WreckageRb = Wreckage.GetComponent<Rigidbody2D>();
                WreckageRb.velocity = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
            }
        }

        /*if(dropType == "Enemy")
        {
            var Skull = Instantiate(skull, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            Skull.transform.parent = transform.parent;
            Rigidbody2D skullRb = Skull.GetComponent<Rigidbody2D>();
            skullRb.velocity = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
        }*/

        Destroy(gameObject);
    }

}
