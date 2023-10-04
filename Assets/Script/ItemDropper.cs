using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public int lootMinItems;
    public int lootMaxItems;

    public List<GameObject> lootings;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropItems()
    {
        int randonDropTimesCounter = Random.Range(lootMinItems, lootMaxItems);

        //Debug.Log(randonDropTimesCounter);

        //lootings = new List<GameObject>(Resources.LoadAll<GameObject>("Prefab/Lootings/ChestLevel1"));

        for (int i = 0; i <= randonDropTimesCounter - 1; i++)
        {
            float position = Random.Range(-3f, 3f);
            int randonDrop = Random.Range(0, lootings.Count);

            //Debug.Log(lootings[randonDrop]);

            var DropItem = Instantiate(lootings[randonDrop], new Vector3(transform.position.x, transform.position.y, transform.position.z), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
            DropItem.transform.parent = this.transform;
            StartCoroutine(dropAnimation(DropItem, position));
        }
    }

    private IEnumerator dropAnimation(GameObject DropItem, float position)
    {
        float temp = position / 2;
        if(position > 0)
        {
            for (float x = 0f; x <= position; x += 0.1f)
            {
                float y = -1 * ((x - temp) * (x - temp)) + (temp * temp);
                DropItem.transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
                yield return new WaitForSeconds(0.01f);
            }
        }
        if(position < 0)
        {
            for (float x = 0f; x >= position; x -= 0.1f)
            {
                float y = -1 * ((x - temp) * (x - temp)) + (temp * temp);
                DropItem.transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
