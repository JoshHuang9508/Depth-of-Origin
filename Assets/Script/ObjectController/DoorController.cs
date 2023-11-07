using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Setting")]
    public bool requiredKeys;
    public string keyName;
    public bool canReopen;

    [Header("Status")]
    public bool isOpen;

    Animator animator;
    BoxCollider2D BoxCollider2D;
    PlayerBehaviour player;
    Interactable interactable;

    void Start()
    {
        animator = GetComponent<Animator>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        interactable = GetComponent<Interactable>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    public void OpenDoor()
    {
        if (!isOpen && haveKey())
        {
            isOpen = true;
            interactable.enabled = canReopen ? true : false;
            BoxCollider2D.enabled = false;
            animator.SetTrigger("Open");
        }
    }

    public void CloseDoor()
    {
        if (isOpen && canReopen)
        {
            isOpen = false;
            interactable.enabled = canReopen ? true : false;
            BoxCollider2D.enabled = true;
            animator.SetTrigger("Close");
        }
    }

    private bool haveKey()
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
