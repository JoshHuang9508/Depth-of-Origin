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
    public int lootMinItems;
    public int lootMaxItems;

    public List<GameObject> lootings;
    public GameObject healthText;
    public GameObject itemDropper;
    public GameObject skull;

    public float Health
    {
        set
        {
            if (value < health)
            {
                //play hit animation

                //health text
                RectTransform text_Transform = Instantiate(healthText).GetComponent<RectTransform>();
                text_Transform.GetComponent<TextMeshProUGUI>().text = (health - value).ToString();
                text_Transform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

                Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
                text_Transform.SetParent(canvas.transform);
            }

            health = value;

            if (health <= 0)
            {
                //play dead animation
                var Skull = Instantiate(skull, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                Skull.transform.parent = transform.parent;
                Rigidbody2D skullRb = Skull.GetComponent<Rigidbody2D>();
                skullRb.velocity = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);

                //drop items
                var ItemDropper = Instantiate(itemDropper, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                ItemDropper.transform.parent = GameObject.FindWithTag("Item").transform;
                ItemDropper itemDropperController = ItemDropper.GetComponent<ItemDropper>();
                itemDropperController.DropItems(lootings, lootMinItems, lootMaxItems);

                Destroy(gameObject);
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