using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public void DropItems(List<GameObject> lootings, int lootMinItems, int lootMaxItems)
    {
        int randonDropTimesCounter = Random.Range(lootMinItems, lootMaxItems);

        //Debug.Log(randonDropTimesCounter);

        for (int i = 0; i <= randonDropTimesCounter - 1; i++)
        {
            float distance_x = Random.Range(-3f, 3f);
            float distance_y = Random.Range(-3f, 3f);
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
    }

}
