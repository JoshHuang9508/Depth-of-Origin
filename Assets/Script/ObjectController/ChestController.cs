using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class ChestController : MonoBehaviour
{
    [Header("Setting")]
    public bool requiredKeys;
    public string keyName;

    [Header("Status")]
    public bool isOpen;

    [Header("Looting")]
    public int lootMinCoins;
    public int lootMaxCoins;
    public List<Lootings> lootings;

    [Header("Connect Object")]
    public GameObject itemDropper;

    Animator animator;
    PlayerBehaviour player;
    Interactable interactable;

    void Start()
    {
        animator = GetComponent<Animator>();
        interactable = GetComponent<Interactable>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    public void OpenChest()
    {
        bool haveKey = false;

        if (requiredKeys)
        {
            foreach (var key in player.keyList)
            {
                if (key.key.Name == keyName) haveKey = true;
            }
        }
        else haveKey = true;

        if (!isOpen && haveKey)
        {
            isOpen = true;
            interactable.enabled = false;
            animator.SetTrigger("Open");

            var ItemDropper = Instantiate(itemDropper, new Vector3(transform.position.x, transform.position.y, transform.position.z), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
            ItemDropper.transform.parent = GameObject.FindWithTag("Item").transform;
            ItemDropper itemDropperController = ItemDropper.GetComponent<ItemDropper>();
            itemDropperController.DropCoins(lootMinCoins, lootMaxCoins);
            itemDropperController.DropItems(lootings);
        }
    }
}