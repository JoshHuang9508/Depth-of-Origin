using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class ChestController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private bool requiredKeys;
    [SerializeField] private string keyName;

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

    [Header("Player Data")]
    [SerializeField] private PlayerBehaviour player;

    [Header("Stats")]
    public bool isOpen;


    void Start()
    {
        audioPlayer = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    public void OpenChest()
    {
        if (!isOpen && HaveKey())
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

    public bool HaveKey()
    {
        bool haveKey = false;
        int indexOfKeyList = -1;

        if (requiredKeys)
        {
            foreach (var key in player.keyList)
            {
                if (key.key.Name == keyName)
                {
                    haveKey = true;
                    indexOfKeyList = player.keyList.IndexOf(key);
                }
            }
            if (haveKey)
            {
                player.keyList[indexOfKeyList].quantity--;
            }
        }
        else haveKey = true;

        return haveKey;
    }
}
