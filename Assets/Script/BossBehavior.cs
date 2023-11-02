using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossBehavior : MonoBehaviour, Damage_Interface
{
    public EnemySO enemy;

    [Header("Connect Object")]
    public GameObject damageText;
    public GameObject itemDropper;
    public GameObject shield;

    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D currentRb;
    Transform target;
    Damage_Interface damageableObject;
    Vector2 currentPos, characterPos, diraction;

    public static int shieldBreakCount = 6;
    int behaviorType = 1;

    public float currentHealth;
    bool movementEnabler = true;
    bool damageEnabler = true;
    bool attackEnabler = true;

    bool isCrit = false;

   
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
                itemDropperController.Drop(enemy.lootings, enemy.lootMinItems, enemy.lootMaxItems, enemy.wreckage);

                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        currentHealth = enemy.health;
        target = GameObject.FindWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        currentRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        damageableObject = target.GetComponentInParent<Damage_Interface>();
    }

    private void Update()
    {
        characterPos = target.transform.position;
        currentPos = transform.position;
        diraction = (characterPos - currentPos).normalized;

        if(currentHealth <= enemy.health * 0.5)
        {
            behaviorType = 2;
        }

        Moving();
    }

    public void Moving()
    {
        switch (behaviorType)
        {
            case 1:
                currentRb.bodyType = RigidbodyType2D.Static;
                Attacking();
                break;
            case 2:
                currentRb.bodyType = RigidbodyType2D.Dynamic;
                if (Vector3.Distance(target.position, this.transform.position) <= enemy.chaseField && Vector3.Distance(target.position, this.transform.position) >= enemy.attackField && movementEnabler && attackEnabler)
                {
                    float movement_x = (this.transform.position.x - target.position.x <= 0) ? 1 : -1;
                    float movement_y = (this.transform.position.y - target.position.y <= 0) ? 1 : -1;
                    currentRb.velocity = new Vector3(movement_x * enemy.moveSpeed, movement_y * enemy.moveSpeed, 0.0f);

                    //play animation
                    animator.SetBool("ismove", true);
                    animator.SetBool("ischase", true);
                }
                else if (Vector3.Distance(target.position, this.transform.position) > enemy.chaseField && movementEnabler && attackEnabler)
                {
                    currentRb.velocity = new Vector2(0.0f, 0.0f);

                    //play animation
                    animator.SetBool("ismove", false);
                    animator.SetBool("ischase", false);
                }
                else if (Vector3.Distance(target.position, this.transform.position) < enemy.attackField && movementEnabler && attackEnabler)
                {
                    currentRb.velocity = new Vector2(0.0f, 0.0f);
                    Attacking();

                    //play animation
                    animator.SetBool("ismove", false);
                    animator.SetBool("ischase", true);
                }
                break;
        }

        spriteRenderer.flipX = (this.transform.position.x - target.position.x) > 0.2 ? true : false;
    }

    public void OnHit(float damage, bool _isCrit, Vector2 knockbackForce, float knockbackTime)
    {
        if (damageEnabler)
        {
            isCrit = _isCrit;
            Health -= damage;

            //knockback
            currentRb.velocity = knockbackForce / 10;


            //delay
            StartCoroutine(delay(enabler => {
                damageEnabler = enabler;
            }, 0.2f));
            StartCoroutine(delay(enabler => {
                movementEnabler = enabler;
                animator.enabled = enabler;
            }, knockbackTime / 10));
        }
    }

    public void Attacking()
    {
        if (attackEnabler)
        {
            switch (behaviorType)
            {
                case 1:
                    if (attackEnabler)
                    {
                        float startAngle = Mathf.Atan2(diraction.y, diraction.x) * Mathf.Rad2Deg;

                        for (int i = -180; i <= 180; i += 18)
                        {
                            GameObject projectileSummoned = Instantiate(enemy.projectile, new Vector3(
                            transform.position.x, transform.position.y, transform.position.z),
                            Quaternion.Euler(0, 0, startAngle - 90 + i),
                            GameObject.FindWithTag("Item").transform);
                            projectileSummoned.AddComponent<ProjectileMovement_Enemy>();
                            projectileSummoned.GetComponent<WeaponMovementRanged>().startAngle = Quaternion.Euler(0, 0, startAngle + i);
                            projectileSummoned.GetComponent<ProjectileMovement_Enemy>().enemy = enemy;
                        }

                        StartCoroutine(delay((enabler) => {
                            attackEnabler = enabler;
                        }, enemy.attackSpeed));
                    }
                    break;
                case 2:
                    if (attackEnabler)
                    {
                        damageableObject.OnHit(enemy.attackDamage, false, diraction * enemy.knockbackForce, enemy.knockbackTime);

                        StartCoroutine(delay((enabler) => {
                            attackEnabler = enabler;
                        }, enemy.attackSpeed));
                    }
                    break;
            }
        }
        
    }

    public void RemoveSomething()
    {
        shieldBreakCount--;
        if(shieldBreakCount <= 0)
        {
            StartCoroutine(delay(callback =>
            shield.SetActive(callback),
            180));
        }
    }

    private IEnumerator delay(System.Action<bool> callback, float delayTime)
    {
        callback(false);
        yield return new WaitForSeconds(delayTime);
        callback(true);
    }
}
