using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropItems(List<GameObject> lootings, int lootMinItems, int lootMaxItems)
    {
        int randonDropTimesCounter = Random.Range(lootMinItems, lootMaxItems);

        //Debug.Log(randonDropTimesCounter);

        //lootings = new List<GameObject>(Resources.LoadAll<GameObject>("Prefab/Lootings/ChestLevel1"));

        for (int i = 0; i <= randonDropTimesCounter - 1; i++)
        {
            float distance = Random.Range(-2.5f, 2.5f);
            int randonDrop = Random.Range(0, lootings.Count);
            var dropItem = Instantiate(
                lootings[randonDrop], 
                new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.position.z
                    ),
                new Quaternion(
                    0.0f, 
                    0.0f, 
                    0.0f, 
                    0.0f
                    )
                );
            dropItem.transform.parent = transform.parent;
            StartCoroutine(dropAnimation(dropItem, distance));
        }
    }

    private IEnumerator dropAnimation(GameObject dropItem, float distance)
    {
        float temp = distance / 2;
        if(distance >= 0)
        {
            for (float x = 0f; x <= distance; x += 0.1f)
            {
                float y = -1 * ((x - temp) * (x - temp)) + (temp * temp);
                dropItem.transform.position = new Vector3(
                    transform.position.x + x,
                    transform.position.y + y,
                    transform.position.z
                    );
                yield return new WaitForSeconds(0.01f);
            }
        }
        if(distance <= 0)
        {
            for (float x = 0f; x >= distance; x -= 0.1f)
            {
                float y = -1 * ((x - temp) * (x - temp)) + (temp * temp);
                dropItem.transform.position = new Vector3(
                    transform.position.x + x,
                    transform.position.y + y,
                    transform.position.z
                    );
                yield return new WaitForSeconds(0.01f);
            }
        }

        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
