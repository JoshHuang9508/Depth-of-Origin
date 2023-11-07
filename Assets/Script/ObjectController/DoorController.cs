using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Setting")]
    public bool requiredKeys;
    public string keyName;

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
            BoxCollider2D.enabled = false;
            animator.SetTrigger("Open");
        }
    }
}
