using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{

    public bool isOpen;

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
            isOpen = true;
            Debug.Log("Opened a chest");
            animator.SetBool("isOpen", isOpen);
        }
        else if (isOpen)
        {
            isOpen = false;
            Debug.Log("Closed a chest");
            animator.SetBool("isOpen", isOpen);
        }
    }
}
