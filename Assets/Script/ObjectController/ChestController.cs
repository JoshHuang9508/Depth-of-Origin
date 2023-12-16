using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class ChestController : MonoBehaviour
{
    [Header("Looting")]
    public List<Coins> coins;
    public List<Lootings> lootings;

    [Header("Audio")]
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip openSound;

    [Header("Object Reference")]
    [SerializeField] private Animator animator;
    [SerializeField] public Interactable interactable;
    [SerializeField] private GameObject itemDropper;

    [Header("Stats")]
    public bool isOpen;


    void Start()
    {
        audioPlayer = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();
    }

    public void OpenChest()
    {
        if (!isOpen)
        {
            isOpen = true;
            interactable.enabled = false;

            animator.SetTrigger("Open");

            audioPlayer.PlayOneShot(openSound);

            var ItemDropper = Instantiate(
                itemDropper,
                transform.position,
                new Quaternion(0.0f, 0.0f, 0.0f, 0.0f),
                GameObject.FindWithTag("Item").transform
                ).GetComponent<ItemDropper>();

            ItemDropper.DropCoins(coins);
            ItemDropper.DropItems(lootings);
        }
    }
}
