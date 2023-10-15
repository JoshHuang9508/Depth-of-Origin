using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Inventory.Model;

public class BreakableObject : MonoBehaviour, Damage_Interface
{
    [Header("Basic Data")]
    public float health;

    [Header("Looting")]
    public int lootMinItems;
    public int lootMaxItems;
    public List<Lootings> lootings;
    public List<GameObject> wreckage;

    [Header("Connect Object")]
    public GameObject damageText;
    public GameObject itemDropper;

    bool damageEnabler = true;

    public float Health
    {
        set
        {
            if (value < health)
            {
                //play hit animation

                //show damage text
                RectTransform text_Transform = Instantiate(damageText).GetComponent<RectTransform>();
                text_Transform.GetComponent<TextMeshProUGUI>().text = (health - value).ToString();
                text_Transform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

                Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
                text_Transform.SetParent(canvas.transform);
            }

            health = value;

            if (health <= 0)
            {
                //play dead animation

                //drop items
                var ItemDropper = Instantiate(itemDropper, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
                ItemDropper.transform.parent = GameObject.FindWithTag("Item").transform;
                ItemDropper itemDropperController = ItemDropper.GetComponent<ItemDropper>();
                itemDropperController.DropItems(lootings, lootMinItems, lootMaxItems);
                itemDropperController.DropWrackages(wreckage);

                Destroy(gameObject);
            }
        }
        get
        {
            return health;
        }
    }

    public void OnHit(float damage, Vector2 knockbackForce, float knockbackTime)
    {
        if (damageEnabler)
        {
            Health -= damage;
            StartCoroutine(damage_delay());
        }
    }

    private IEnumerator damage_delay()
    {
        damageEnabler = false;
        yield return new WaitForSeconds(0.2f);
        damageEnabler = true;
    }
}
