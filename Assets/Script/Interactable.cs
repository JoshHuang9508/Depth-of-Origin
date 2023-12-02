using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class Interactable : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private bool interactable;
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private UnityEvent interactAction, enterRangeAction, leaveRangeAction;

    [Header("Object Reference")]
    [SerializeField] private GameObject interactDialogObject;

    [Header("Dynamic Data")]
    [SerializeField] private TMP_Text interactDialogText;
    [SerializeField] private GameObject interactDialog;

    [Header("Stats")]
    [SerializeField] private bool isInRange;
    

    void Start()
    {
        if(interactable)
        {
            interactDialog = Instantiate(
            interactDialogObject,
            transform.position + new Vector3(0, 1.5f, 0),
            Quaternion.identity,
            transform
            );

            interactDialog.SetActive(false);
            interactDialogText = interactDialog.GetComponentInChildren<TMP_Text>();
        }
    }

    void Update()
    {
        if (isInRange && this.enabled && interactable)
        {
            if (Input.GetKeyDown(interactKey))
            {
                try
                {
                    interactAction.Invoke();
                }
                catch
                {
                    Debug.LogWarning("Unexpecting problem, please check if something went wrong!!");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            enterRangeAction.Invoke();

            if (enabled && interactable)
            {
                interactDialog.SetActive(true);
                interactDialogText.text = $"Press {interactKey} to interact";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            leaveRangeAction.Invoke();

            if (interactable) interactDialog.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (interactable) interactDialog.SetActive(false);
    }
}
