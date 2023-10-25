using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class EnemyBehavior : MonoBehaviour, Damage_Interface
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

    public float Health
    {
        set
        {
            if (value < currentHealth)
            {
                //play hit animation
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
        get
        {
            return currentHealth;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = enemySO.health;
        target = GameObject.FindWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        currentRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        damageableObject = target.GetComponentInParent<Damage_Interface>();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }

    void Moving()
    {
        if (Vector3.Distance(target.position, this.transform.position) <= enemySO.chaseField && Vector3.Distance(target.position, this.transform.position) >= enemySO.attackField && movementEnabler && attackEnabler)
        {
            float movement_x = (this.transform.position.x - target.position.x <= 0) ? 1 : -1;
            float movement_y = (this.transform.position.y - target.position.y <= 0) ? 1 : -1;
            currentRb.velocity = new Vector3(movement_x * enemySO.moveSpeed, movement_y * enemySO.moveSpeed, 0.0f);

            //play animation
            animator.SetBool("ismove", true);
            animator.SetBool("ischase", true);
        }
        else if (Vector3.Distance(target.position, this.transform.position) > enemySO.chaseField && movementEnabler && attackEnabler)
        {
            currentRb.velocity = new Vector2(0.0f, 0.0f);

            //play animation
            animator.SetBool("ismove", false);
            animator.SetBool("ischase", false);
        }
        else if (Vector3.Distance(target.position, this.transform.position) < enemySO.attackField && movementEnabler && attackEnabler)
        {
            currentRb.velocity = new Vector2(0.0f, 0.0f);
            Attacking();

            //play animation
            animator.SetBool("ismove", false);
            animator.SetBool("ischase", true);
        }

        spriteRenderer.flipX = (this.transform.position.x - target.position.x) > 0.2 ? true : false;
    }

    void Attacking()
    {
        if (attackEnabler)
        {
            Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
            Vector2 direction = (Vector2)(target.gameObject.transform.position - parentPos).normalized;

            damageableObject.OnHit(enemySO.attackDamage, false, direction * enemySO.knockbackForce, enemySO.knockbackTime);

            StartCoroutine(delay((enabler) => {
                attackEnabler = enabler;
            }, enemySO.attackSpeed));
        }
    }

    public void OnHit(float damage, bool isCrit, Vector2 knockbackForce, float knockbackTime)
    {
        if (damageEnabler)
        {
            Health -= damage;

            //damage text
            RectTransform text_Transform = Instantiate(damageText).GetComponent<RectTransform>();
            text_Transform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            text_Transform.SetParent(GameObject.FindFirstObjectByType<Canvas>().transform);

            TextMeshProUGUI text_MeshProUGUI = text_Transform.GetComponent<TextMeshProUGUI>();
            text_MeshProUGUI.text = damage.ToString();
            text_MeshProUGUI.color = isCrit ? new Color(255, 255, 0, 255) : new Color(255, 255, 255, 255);
            text_MeshProUGUI.outlineColor = isCrit ? new Color(255, 0, 0, 255) : new Color(255, 255, 255, 0);
            text_MeshProUGUI.outlineWidth = isCrit ? 0.4f : 0f;


            //knockback
            currentRb.velocity = knockbackForce;


            //delay
            StartCoroutine(delay(enabler => {
                damageEnabler = enabler;
            }, 0.2f)) ;
            StartCoroutine(delay(enabler => {
                movementEnabler = enabler;
                animator.enabled = enabler;
            }, knockbackTime / 1));
        }
    }

    private IEnumerator delay(System.Action<bool> callback, float delayTime)
    {
        callback(false);
        yield return new WaitForSeconds(delayTime);
        callback(true);
    }
}
