using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class EnemyBehavior : MonoBehaviour, Damage_Interface
{
    public EnemySO enemySO;

    [Header("Connect Object")]
    public GameObject healthText;
    public GameObject itemDropper;

    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D currentRb;
    Transform target;
    Damage_Interface damageableObject;

    float health;
    bool movementEnabler = true;
    bool damageEnabler = true;
    bool attackEnabler = true;

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

                //drop items
                var ItemDropper = Instantiate(itemDropper, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                ItemDropper.transform.parent = GameObject.FindWithTag("Item").transform;
                ItemDropper itemDropperController = ItemDropper.GetComponent<ItemDropper>();
                itemDropperController.DropItems(enemySO.lootings, enemySO.lootMinItems, enemySO.lootMaxItems, "Enemy");

                Destroy(gameObject);
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
        health = enemySO.health;
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
        if (Vector3.Distance(target.position, this.transform.position) <= enemySO.chaseField && Vector3.Distance(target.position, this.transform.position) >= enemySO.attackField && movementEnabler)
        {
            float movement_x = (this.transform.position.x - target.position.x <= 0) ? 1 : -1;
            float movement_y = (this.transform.position.y - target.position.y <= 0) ? 1 : -1;
            currentRb.velocity = new Vector3(movement_x * enemySO.moveSpeed, movement_y * enemySO.moveSpeed, 0.0f);

            //play animation
            animator.SetBool("ismove", true);
            animator.SetBool("ischase", true);
        }
        else if (Vector3.Distance(target.position, this.transform.position) > enemySO.chaseField && movementEnabler)
        {
            currentRb.velocity = new Vector2(0.0f, 0.0f);

            //play animation
            animator.SetBool("ismove", false);
            animator.SetBool("ischase", false);
        }
        else if (Vector3.Distance(target.position, this.transform.position) < enemySO.attackField && movementEnabler)
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
            //Debug.Log("enemy trying to attack");

            Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
            Vector2 direction = (Vector2)(target.gameObject.transform.position - parentPos).normalized;

            damageableObject.OnHit(enemySO.attackDamage, direction * enemySO.knockbackForce, enemySO.knockbackTime);
            StartCoroutine(attack_delay());
        }
    }

    public void OnHit(float damage, Vector2 knockbackForce, float knockbackTime)
    {
        if (damageEnabler)
        {
            Health -= damage;
            currentRb.velocity = knockbackForce;
            StartCoroutine(damage_delay());
            StartCoroutine(knockback_delay(knockbackTime));
        }
    }

    private IEnumerator knockback_delay(float knockbackTime)
    {
        animator.enabled = false;
        movementEnabler = false;
        yield return new WaitForSeconds(knockbackTime / 1);
        animator.enabled = true;
        movementEnabler = true;
    }

    private IEnumerator damage_delay()
    {
        damageEnabler = false;
        yield return new WaitForSeconds(0.2f);
        damageEnabler = true;
    }

    private IEnumerator attack_delay()
    {
        attackEnabler = false;
        yield return new WaitForSeconds(enemySO.attackSpeed);
        attackEnabler = true;
    }
}
