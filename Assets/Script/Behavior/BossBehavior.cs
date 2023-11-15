using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BossBehavior : MonoBehaviour, Damageable
{
    [Header("Main Data")]
    public EnemySO enemy;

    [Header("Column Spawn Position")]
    public List<Vector2> positionList;

    [Header("Connect Object")]
    public GameObject damageText;
    public GameObject itemDropper;
    public GameObject shield;
    public GameObject column;

    [Header("Read Only Value")]
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Rigidbody2D currentRb;
    public Transform target;
    public Damageable damageableObject;
    public Vector2 currentPos, targetPos, diraction;
    int behaviorType = 1;
    public float currentHealth;
    public bool movementEnabler = true;
    public bool damageEnabler = true;
    public bool attackEnabler = true;
    public bool behaviorEnabler = true;

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
    }

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        currentRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        damageableObject = target.GetComponentInParent<Damageable>();

        currentRb.bodyType = RigidbodyType2D.Static;
        enemy.attackType = EnemySO.AttackType.Sniper;
        currentHealth = enemy.health;

        StartCoroutine(delay(callback => {
            behaviorEnabler = callback;
        }, 5f));

        BuildColumns();
    }

    private void Update()
    {
        targetPos = target.position;
        currentPos = transform.position;
        diraction = (targetPos - currentPos).normalized;

        if(currentHealth <= enemy.health * 0.5)
        {
            behaviorType = 2;
            behaviorEnabler = true;
            enemy.attackSpeed = 1;
            enemy.attackType = EnemySO.AttackType.Melee;
        }

        Moving();
    }

    public void Moving()
    {
        if (!behaviorEnabler)
        {
            return;
        }

        switch (behaviorType)
        {
            case 1:
                currentRb.bodyType = RigidbodyType2D.Static;
                Attacking();
                break;
            case 2:
                currentRb.bodyType = RigidbodyType2D.Dynamic;
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
        }

        spriteRenderer.flipX = (currentPos.x - targetPos.x) > 0.2 ? true : false;
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
            }, 0.2f));
            StartCoroutine(delay(enabler => {
                movementEnabler = enabler;
                animator.enabled = enabler;
            }, knockbackTime / (1 + (0.001f * enemy.defence))));
        }
    }

    public void Attacking()
    {
        if (attackEnabler && movementEnabler)
        {
            switch (enemy.attackType)
            {
                case EnemySO.AttackType.Sniper:
                    float startAngle = Mathf.Atan2(diraction.y, diraction.x) * Mathf.Rad2Deg;

                    enemy.Attack_Ranged(startAngle, transform.position);

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

    public void RemoveShield()
    {
        StartCoroutine(delay((callback) =>
        {
            shield.SetActive(callback && behaviorType == 1);
            if (callback && behaviorType == 1)  BuildColumns();
        }, 60));

        StartCoroutine(delay(callback => {
            behaviorEnabler = callback;
        }, 65f));
    }

    public void BuildColumns()
    {
        for(int i = 0; i < 6; i++)
        {
            BossColumnController columnSummoned = Instantiate(column, new Vector3(
                positionList[i].x, positionList[i].y, 0),
                Quaternion.identity,
                GameObject.FindWithTag("Object").transform
                ).GetComponent<BossColumnController>();
            columnSummoned.shieldBreak += RemoveShield;
            columnSummoned.Reset();
        }
    }

    private IEnumerator delay(System.Action<bool> callback, float delayTime)
    {
        callback(false);
        yield return new WaitForSeconds(delayTime);
        callback(true);
    }
}
