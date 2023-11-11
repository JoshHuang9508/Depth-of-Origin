using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class Interactable : MonoBehaviour
{
    [Header("Setting")]
    public bool interactable;
    public KeyCode interactKey;
    public UnityEvent interactAction, enterRangeAction, leaveRangeAction;

    [Header("Status")]
    public bool isInRange;

    [Header("Connect Object")]
    public GameObject interactDialog;
    public RectTransform temp;

    void Start()
    {
        interactDialog.SetActive(false);
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
            //Debug.Log("Player in range");
            isInRange = true;
            enterRangeAction.Invoke();

            if (this.enabled && interactable)
            {
                RectTransform text_Transform = Instantiate(
                    interactDialog,
                    new Vector3(960, 200, 0),
                    Quaternion.identity,
                    GameObject.Find("ScreenUI").transform
                    ).GetComponent<RectTransform>();
                text_Transform.gameObject.SetActive(true);

                TMP_Text text = text_Transform.GetComponentInChildren<TMP_Text>();
                text.text = $"Press {interactKey} to interact";

                temp = text_Transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player leave range");
            isInRange = false;
            leaveRangeAction.Invoke();

            try
            {
                Destroy(temp.gameObject);
            }
            catch { }
        }
    }

    private void OnDisable()
    {
        try
        {
            Destroy(temp.gameObject);
        }
        catch { }
    }

    private void OnEnable()
    {

    }
}
