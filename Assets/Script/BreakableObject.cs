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
    public List<Coins> coins;
    public List<Lootings> lootings;
    public List<GameObject> wreckage;

    [Header("Connect Object")]
    public GameObject damageText;
    public GameObject itemDropper;

    [Header("Status")]
    public bool damageEnabler = true;
    public float damageDisableTimer = 0;

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;

            if (health <= 0)
            {
                //drop items
                ItemDropper ItemDropper = Instantiate(
                    itemDropper,
                    new Vector3(transform.position.x, transform.position.y, transform.position.z),
                    Quaternion.identity,
                    GameObject.FindWithTag("Item").transform
                    ).GetComponent<ItemDropper>();
                ItemDropper.DropItems(lootings);
                ItemDropper.DropCoins(coins);
                ItemDropper.DropWrackages(wreckage);
                Destroy(gameObject);
            }
        }
        
    }

    private void Update()
    {
        UpdateTimer();
    }

    public void OnHit(float damage, bool isCrit, Vector2 knockbackForce, float knockbackTime)
    {
        if (UpdateTimer() && damageEnabler)
        {
            Health -= damage;
            InstantiateDamageText(damage, isCrit ? "DamageCrit" : "Damage");

            damageDisableTimer += 0.2f;
        }
    }

    public bool UpdateTimer()
    {
        //update timer
        damageDisableTimer = Mathf.Max(0, damageDisableTimer - Time.deltaTime);

        damageEnabler = damageDisableTimer <= 0;

        return true;
    }

    private void InstantiateDamageText(float value, string type)
    {
        var damageTextInstantiated = Instantiate(
            damageText,
            Camera.main.WorldToScreenPoint(gameObject.transform.position),
            Quaternion.identity,
            GameObject.Find("ScreenUI").transform
            ).GetComponent<DamageText>();

        damageTextInstantiated.SetContent(value, type);
    }
}
