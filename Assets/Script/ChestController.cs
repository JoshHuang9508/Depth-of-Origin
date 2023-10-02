using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public int lootMinItems;
    public int lootMaxItems;
    public bool isOpen;
    public List<GameObject> lootings;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenChest()
    {
        if (!isOpen)
        {
            //Debug.Log("Opened a chest");

            isOpen = true;
            animator.SetBool("isOpen", isOpen);

            DropItems();
        }
        else if (isOpen)
        {
            //Debug.Log("Closed a chest");

            isOpen = false;
            animator.SetBool("isOpen", isOpen);
        }
    }

    public void DropItems()
    {
        int randonDropTimesCounter = Random.Range(lootMinItems, lootMaxItems);

        //Debug.Log(randonDropTimesCounter);

        lootings = new List<GameObject>(Resources.LoadAll<GameObject>("Prefab/Lootings/ChestLevel1"));

        for (int i = 0; i <= randonDropTimesCounter - 1; i++){

            int randonDrop = Random.Range(0, lootings.Count);

            //Debug.Log(lootings[randonDrop]);

            var DropItem = Instantiate(lootings[randonDrop], new Vector3(transform.position.x, transform.position.y, transform.position.z), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
            DropItem.transform.parent = this.transform;
        }
    }
}
