using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private bool requiredKeys;
    [SerializeField] private string keyName;
    [SerializeField] private bool canReopen;

    [Header("Object Reference")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerBehaviour player;
    [SerializeField] private BoxCollider2D BoxCollider2D;
    [SerializeField] private Interactable interactable;

    [Header("Stats")]
    public bool isOpen;


    void Start()
    {
        animator = GetComponent<Animator>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        interactable = GetComponent<Interactable>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    public void OpenDoor()
    {
        if (!isOpen && HaveKey())
        {
            isOpen = true;
            interactable.enabled = false;
            BoxCollider2D.enabled = false;
            animator.SetTrigger("Open");
        }
    }

    public void CloseDoor()
    {
        if (isOpen && canReopen)
        {
            isOpen = false;
            interactable.enabled = true;
            BoxCollider2D.enabled = true;
            animator.SetTrigger("Close");
        }
    }

    private bool HaveKey()
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
