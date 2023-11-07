using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Inventory.Model;

public class BreakableObject : MonoBehaviour, Damageable
{
    [Header("Basic Data")]
    public float health;

    [Header("Looting")]
    public int lootMinCoins;
    public int lootMaxCoins;
    public List<Lootings> lootings;
    public List<GameObject> wreckage;

    [Header("Connect Object")]
    public GameObject damageText;
    public GameObject itemDropper;

    bool damageEnabler = true;

    bool isCrit;
    public float Health
    {
        set
        {
            if (value < health)
            {
                //play hit animation

                //damage text
                RectTransform text_Transform = Instantiate(damageText).GetComponent<RectTransform>();
                text_Transform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                text_Transform.SetParent(GameObject.FindFirstObjectByType<Canvas>().transform);

                TextMeshProUGUI text_MeshProUGUI = text_Transform.GetComponent<TextMeshProUGUI>();
                text_MeshProUGUI.text = Mathf.RoundToInt(health - value).ToString();
                text_MeshProUGUI.color = isCrit ? new Color(255, 255, 0, 255) : new Color(255, 255, 255, 255);
                text_MeshProUGUI.outlineColor = isCrit ? new Color(255, 0, 0, 255) : new Color(255, 255, 255, 0);
                text_MeshProUGUI.outlineWidth = isCrit ? 0.4f : 0f;
            }

            if (value >= health)
            {
                //damage text
                RectTransform text_Transform = Instantiate(damageText).GetComponent<RectTransform>();
                text_Transform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                text_Transform.SetParent(GameObject.FindFirstObjectByType<Canvas>().transform);

                TextMeshProUGUI text_MeshProUGUI = text_Transform.GetComponent<TextMeshProUGUI>();
                text_MeshProUGUI.text = Mathf.RoundToInt(value - health).ToString();
                text_MeshProUGUI.color = new Color(0, 150, 0, 255);
            }

            health = value;

            if (health <= 0)
            {
                //drop items
                var ItemDropper = Instantiate(
                    itemDropper,
                    new Vector3(
                        transform.position.x,
                        transform.position.y + 0.5f,
                        transform.position.z),
                    Quaternion.identity,
                    GameObject.FindWithTag("Item").transform
                    );

                ItemDropper.GetComponent<ItemDropper>().DropItems(lootings);
                ItemDropper.GetComponent<ItemDropper>().DropCoins(lootMinCoins, lootMaxCoins);
                ItemDropper.GetComponent<ItemDropper>().DropWrackages(wreckage);
                Destroy(gameObject);
            }
        }
        get
        {
            return health;
        }
    }

    public void OnHit(float damage, bool _isCrit, Vector2 knockbackForce, float knockbackTime)
    {
        if (damageEnabler)
        {
            isCrit = _isCrit;
            Health -= damage;

            //delay
            StartCoroutine(delay(enabler => {
                damageEnabler = enabler;
            }, 0.2f));
        }
    }

    private IEnumerator delay(System.Action<bool> callback, float delayTime)
    {
        callback(false);
        yield return new WaitForSeconds(delayTime);
        callback(true);
    }
}
