using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossBehavior : MonoBehaviour,Damage_Interface
{
    public EnemySO enemySO;

    [Header("Connect Object")]
    public GameObject damageText;
    public GameObject itemDropper;

    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D currentRb;
    Transform target;
    Damage_Interface damageableObject;

    float currentHealth;
    bool movementEnabler = true;
    bool damageEnabler = true;
    bool attackEnabler = true;
    int face=1;

    bool isCrit;

   
    public float Health
    {
        get { return currentHealth; }
        set 
        {
            if (value < currentHealth)
            {
                //play hit animation

                //damage text
                RectTransform text_Transform = Instantiate(damageText).GetComponent<RectTransform>();
                text_Transform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                text_Transform.SetParent(GameObject.FindFirstObjectByType<Canvas>().transform);

                TextMeshProUGUI text_MeshProUGUI = text_Transform.GetComponent<TextMeshProUGUI>();
                text_MeshProUGUI.text = (currentHealth - value).ToString();
                text_MeshProUGUI.color = isCrit ? new Color(255, 255, 0, 255) : new Color(255, 255, 255, 255);
                text_MeshProUGUI.outlineColor = isCrit ? new Color(255, 0, 0, 255) : new Color(255, 255, 255, 0);
                text_MeshProUGUI.outlineWidth = isCrit ? 0.4f : 0f;
            }

            if (value >= currentHealth)
            {
                //damage text
                RectTransform text_Transform = Instantiate(damageText).GetComponent<RectTransform>();
                text_Transform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                text_Transform.SetParent(GameObject.FindFirstObjectByType<Canvas>().transform);

                TextMeshProUGUI text_MeshProUGUI = text_Transform.GetComponent<TextMeshProUGUI>();
                text_MeshProUGUI.text = (value - currentHealth).ToString();
                text_MeshProUGUI.color = new Color(0, 150, 0, 255);
            }

            currentHealth = value;

            if (currentHealth <= 0)
            {
                //play dead animation

                //drop items
                var ItemDropper = Instantiate(itemDropper, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                ItemDropper.transform.parent = GameObject.FindWithTag("Item").transform;
                ItemDropper itemDropperController = ItemDropper.GetComponent<ItemDropper>();
                itemDropperController.Drop(enemySO.lootings, enemySO.lootMinItems, enemySO.lootMaxItems, enemySO.wreckage);

                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        currentHealth = enemySO.health;
        target = GameObject.FindWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        currentRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        damageableObject = target.GetComponentInParent<Damage_Interface>();
    }

    private void Update()
    {
        Moving();
    }

    public void Moving()
    {
        switch (face)
        {
            case 1:
                currentRb.bodyType = RigidbodyType2D.Static;
                break;
            case 2:
                currentRb.bodyType = RigidbodyType2D.Dynamic;
                if (Vector3.Distance(target.position, this.transform.position) <= enemySO.chaseField && Vector3.Distance(target.position, this.transform.position) >= enemySO.attackField && movementEnabler && attackEnabler)
                {
                    float movement_x = (this.transform.position.x - target.position.x <= 0) ? 1 : -1;
                    float movement_y = (this.transform.position.y - target.position.y <= 0) ? 1 : -1;
                    currentRb.velocity = new Vector3(movement_x * enemySO.moveSpeed, movement_y * enemySO.moveSpeed, 0.0f);

                    //play animation
                    animator.SetBool("ismove", true);
                    animator.SetBool("ischase", true);
                }
                break;
        }
    }

    public void OnHit(float damage, bool isCrit, Vector2 knockbackForce, float knockbackTime)
    {
        
    }
    public void Attacking()
    {
        if (attackEnabler)
        {
            switch (face)
            {
                case 1:

                    break;
                case 2:
                    break;
            }
        }
        
    }
    




}
