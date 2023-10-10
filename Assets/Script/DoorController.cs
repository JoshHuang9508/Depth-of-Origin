using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    bool isOpen;

    Animator animator;
    BoxCollider2D BoxCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        if (!isOpen)
        {
            //Debug.Log("Opened a chest");

            isOpen = true;
            BoxCollider2D.enabled = false;
            animator.SetTrigger("Open");
        }
        else if (isOpen)
        {
            //Debug.Log("Closed a chest");

            isOpen = false;
            BoxCollider2D.enabled = true;
            animator.SetTrigger("Close");
        }
    }
}
