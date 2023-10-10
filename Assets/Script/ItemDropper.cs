using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public GameObject skull;

    public void DropItems(List<GameObject> lootings, int lootMinItems, int lootMaxItems, string dropType)
    {
        int randonDropTimesCounter = Random.Range(lootMinItems, lootMaxItems);

        //Debug.Log(randonDropTimesCounter);

        for (int i = 0; i <= randonDropTimesCounter - 1; i++)
        {
            float distance_x = Random.Range(-2, 2f);
            float distance_y = Random.Range(-2f, 2f);
            int randonDrop = Random.Range(0, lootings.Count);
            var dropItem = Instantiate(
                lootings[randonDrop],
                new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.position.z
                    ),
                Quaternion.identity
                );
            dropItem.transform.parent = transform.parent;
            Rigidbody2D dropItemRb = dropItem.GetComponent<Rigidbody2D>();
            dropItemRb.velocity = new Vector2(distance_x * 10, distance_y * 10);
        }

        if(dropType == "Enemy")
        {
            var Skull = Instantiate(skull, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            Skull.transform.parent = transform.parent;
            Rigidbody2D skullRb = Skull.GetComponent<Rigidbody2D>();
            skullRb.velocity = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
        }

        Destroy(gameObject);
    }

}
