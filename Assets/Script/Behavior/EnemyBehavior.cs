using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class EnemyBehavior : MonoBehaviour, Damageable
{
    public EnemySO enemy;

    [Header("Connect Object")]
    public GameObject damageText;
    public GameObject itemDropper;

    [Header("Read Only Value")]
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Rigidbody2D currentRb;
    public Transform target;
    public Damageable damageableObject;
    public Vector2 currentPos, targetPos, diraction;
    public float currentHealth;
    public bool movementEnabler = true;
    public bool damageEnabler = true;
    public bool attackEnabler = true;
    public bool DodgeEnabler = true;


    bool isCrit;
    public float Health
    {
        set
        {
            if (value < currentHealth)
            {
                //play hit animation

                //damage text
                RectTransform text_Transform = Instantiate(
                    damageText,
                    Camera.main.WorldToScreenPoint(gameObject.transform.position),
                    Quaternion.identity,
                    GameObject.Find("ScreenUI").transform
                    ).GetComponent<RectTransform>();

                TextMeshProUGUI text_MeshProUGUI = text_Transform.GetComponent<TextMeshProUGUI>();
                text_MeshProUGUI.text = Mathf.RoundToInt(currentHealth - value).ToString();
                text_MeshProUGUI.color = isCrit ? new Color(255, 255, 0, 255) : new Color(255, 255, 255, 255);
                text_MeshProUGUI.outlineColor = isCrit ? new Color(255, 0, 0, 255) : new Color(255, 255, 255, 0);
                text_MeshProUGUI.outlineWidth = isCrit ? 0.4f : 0f;
            }

            if (value >= currentHealth)
            {
                //damage text
                RectTransform text_Transform = Instantiate(
                    damageText,
                    Camera.main.WorldToScreenPoint(gameObject.transform.position),
                    Quaternion.identity,
                    GameObject.Find("ScreenUI").transform
                    ).GetComponent<RectTransform>();

                TextMeshProUGUI text_MeshProUGUI = text_Transform.GetComponent<TextMeshProUGUI>();
                text_MeshProUGUI.text = Mathf.RoundToInt(value - currentHealth).ToString();
                text_MeshProUGUI.color = new Color(0, 150, 0, 255);
            }

            currentHealth = value;

            if (currentHealth <= 0)
            {
                //play dead animation

                //drop items
                ItemDropper ItemDropper = Instantiate(
                    itemDropper,
                    new Vector3(transform.position.x, transform.position.y, transform.position.z),
                    Quaternion.identity,
                    GameObject.FindWithTag("Item").transform
                    ).GetComponent<ItemDropper>();
                ItemDropper.DropItems(enemy.lootings);
                ItemDropper.DropCoins(enemy.coins);
                ItemDropper.DropWrackages(enemy.wreckage);

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
        currentHealth = enemy.health;
        target = GameObject.FindWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        currentRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        damageableObject = target.GetComponentInParent<Damageable>();
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;
        targetPos = target.transform.position;
        diraction = (targetPos - currentPos).normalized;

        Moving();
    }

    void Moving()
    {
        switch (enemy.attackType)
        {
            case EnemySO.AttackType.Melee:
                if (Vector3.Distance(targetPos, currentPos) <= enemy.chaseField && Vector3.Distance(targetPos, currentPos) >= enemy.attackField && movementEnabler && attackEnabler)
                {
                    currentRb.MovePosition(currentPos + diraction * enemy.moveSpeed * Time.deltaTime);

                    //play animation
                    animator.SetBool("ismove", true);
                    animator.SetBool("ischase", true);
                }
                else if (Vector3.Distance(targetPos, currentPos) > enemy.chaseField && movementEnabler && attackEnabler)
                {
                    currentRb.velocity = Vector2.zero;

                    //play animation
                    animator.SetBool("ismove", false);
                    animator.SetBool("ischase", false);
                }
                else if (Vector3.Distance(targetPos, currentPos) < enemy.attackField && movementEnabler && attackEnabler)
                {
                    currentRb.velocity = Vector2.zero;
                    Attacking();

                    //play animation
                    animator.SetBool("ismove", false);
                    animator.SetBool("ischase", true);
                }
                break;

            case EnemySO.AttackType.Sniper:
                if (Vector3.Distance(targetPos, currentPos) <= enemy.chaseField && movementEnabler)
                {
                    currentRb.MovePosition(currentPos - diraction * enemy.moveSpeed * Time.deltaTime);
                    Attacking();

                    //play animation
                    animator.SetBool("ismove", true);
                    animator.SetBool("ischase", true); 
                }
                else if (Vector3.Distance(targetPos, currentPos) > enemy.chaseField && movementEnabler)
                {
                    currentRb.velocity = Vector2.zero;

                    //play animation
                    animator.SetBool("ismove", false);
                    animator.SetBool("ischase", true);
                }
                break;
        }
        

        spriteRenderer.flipX = (currentPos.x - targetPos.x) > 0.2 ? true : false;
    }

    void Attacking()
    {
        if (attackEnabler && movementEnabler)
        {
            switch (enemy.attackType)
            {
                case EnemySO.AttackType.Sniper:
                    float startAngle = Mathf.Atan2(diraction.y, diraction.x) * Mathf.Rad2Deg;

                    enemy.Attack_Ranged(startAngle, transform.position + new Vector3(0,0.5f,0));

                    StartCoroutine(delay((enabler) => {
                        attackEnabler = enabler;
                    }, enemy.attackSpeed));
                    break;

                case EnemySO.AttackType.Melee:
                    damageableObject.OnHit(enemy.attackDamage, false, diraction * enemy.knockbackForce, enemy.knockbackTime);

                    StartCoroutine(delay((enabler) => {
                        attackEnabler = enabler;
                    }, enemy.attackSpeed));
                    break;
            }
        }
    }


    public void OnHit(float damage, bool _isCrit, Vector2 knockbackForce, float knockbackTime)
    {
        if (damageEnabler)
        {
            isCrit = _isCrit;
            Health -= damage / (1 + (0.001f * enemy.defence));

            //knockback
            currentRb.velocity = knockbackForce / (1 + (0.001f * enemy.defence));

            //delay
            StartCoroutine(delay(enabler => {
                damageEnabler = enabler;
            }, 0.2f)) ;
            StartCoroutine(delay(enabler => {
                movementEnabler = enabler;
                animator.enabled = enabler;
            }, knockbackTime / (1 + (0.001f * enemy.defence))));
        }
    }

    private IEnumerator delay(System.Action<bool> callback, float delayTime)
    {
        callback(false);
        yield return new WaitForSeconds(delayTime);
        callback(true);
    }
}
