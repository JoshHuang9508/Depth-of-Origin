using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class EnemyBasicLogic : MonoBehaviour
{
    public string Name;
    public float health;
    public float attackSpeed;
    public float attackDamage;
    public float moveSpeed;
    public float chaseField;
    public float attackField;
    public float knockbackForce;
    public float knockbackTime;
    public float knockbackSpeed;

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

                Canvas canvas = GameObject.FindObjectOfType<Canvas>();
                text_Transform.SetParent(canvas.transform);
            }

            health = value;

            if (health <= 0)
            {
                Destroy(gameObject);
                //play dead animation
            }
        }
        get
        {
            return health;
        }
    }

    private void Start()
    {

    }
}