using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBasicLogic : MonoBehaviour
{
    public float walkSpeed;
    public float health;

    public GameObject healthText;

    public float Health
    {
        set
        {
            if (value < health)
            {
                //play hit animation
                RectTransform text_Transform = Instantiate(healthText).GetComponent<RectTransform>();
                text_Transform.GetComponent<TextMeshProUGUI>().text = (health - value).ToString();
                text_Transform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

                Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
                text_Transform.SetParent(canvas.transform);
            }

            if (value > health)
            {
                RectTransform text_Transform = Instantiate(healthText).GetComponent<RectTransform>();
                text_Transform.GetComponent<TextMeshProUGUI>().text = (value - health).ToString();
                text_Transform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

                Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
                text_Transform.SetParent(canvas.transform);
            }

            health = value;

            if (health <= 0)
            {
                Debug.Log("Player Dead");
                //play dead animation
            }
        }
        get
        {
            return health;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
