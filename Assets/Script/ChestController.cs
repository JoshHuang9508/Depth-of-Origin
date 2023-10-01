using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{

    public bool isOpen;

    public Animator animation;

    // Start is called before the first frame update
    void Start()
    {
        
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
            animation.SetBool("isOpen", isOpen);
        }
        else if (isOpen)
        {
            isOpen = false;
            Debug.Log("Closed a chest");
            animation.SetBool("isOpen", isOpen);
        }
    }
}
