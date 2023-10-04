using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestController : MonoBehaviour
{
    public bool isOpen;

    public UnityEvent interAction;
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
            animator.SetBool("isOpen", isOpen);

            interAction.Invoke();
        }
        else if (isOpen)
        {
            //Debug.Log("Closed a chest");

            //isOpen = false;
            //animator.SetBool("isOpen", isOpen);
        }
    }

    
}
