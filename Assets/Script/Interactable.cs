using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class Interactable : MonoBehaviour
{
    [Header("Setting")]
    public KeyCode interactKey;
    public UnityEvent interactAction, leaveRangeAction;

    [Header("Status")]
    public bool isInRange;

    [Header("Connect Object")]
    public GameObject interactDialog;
    public RectTransform temp;

    // Start is called before the first frame update
    void Start()
    {
        interactDialog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange)
        {
            
            if (Input.GetKeyDown(interactKey))
            {
                interactAction.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player in range");

            RectTransform text_Transform = Instantiate(interactDialog).GetComponent<RectTransform>();
            text_Transform.gameObject.SetActive(true);
            text_Transform.transform.position = new Vector3(960, 200, 0);
            text_Transform.SetParent(GameObject.FindFirstObjectByType<Canvas>().transform);

            TMP_Text text = text_Transform.GetComponentInChildren<TMP_Text>();
            text.text = $"Press {interactKey} to interact";

            temp = text_Transform;
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player leave range");

            Destroy(temp.gameObject);
            isInRange = false;
            leaveRangeAction.Invoke();
        }
    }
}
