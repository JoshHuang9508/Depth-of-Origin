using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    bool isOpen;
    [Header("Looting")]
    public int lootMinItems;
    public int lootMaxItems;
    public List<GameObject> lootings;
    public List<GameObject> wreckage;

    [Header("Connect Object")]
    public GameObject itemDropper;

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
            animator.SetTrigger("Open");

            var ItemDropper = Instantiate(itemDropper, new Vector3(transform.position.x, transform.position.y, transform.position.z), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
            ItemDropper.transform.parent = GameObject.FindWithTag("Item").transform;
            ItemDropper itemDropperController = ItemDropper.GetComponent<ItemDropper>();
            itemDropperController.DropItems(lootings, wreckage, lootMinItems, lootMaxItems, "Chest");
        }
        /*else if (isOpen)
        {
            //Debug.Log("Closed a chest");

            isOpen = false;
            animator.SetTrigger("Close");
        }*/
    }

    
}
