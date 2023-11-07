using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Status")]
    public bool isOpen;

    Animator animator;
    BoxCollider2D BoxCollider2D;

    void Start()
    {
        animator = GetComponent<Animator>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            BoxCollider2D.enabled = false;
            animator.SetTrigger("Open");
        }
        else if (isOpen)
        {
            isOpen = false;
            BoxCollider2D.enabled = true;
            animator.SetTrigger("Close");
        }
    }
}
