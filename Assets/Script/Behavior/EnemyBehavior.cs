using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEngine.Rendering.DebugUI;

public class EnemyBehavior : MonoBehaviour, Damageable
{
    public EnemySO enemy;

    [Header("Connect Object")]
    public GameObject damageText;
    public GameObject itemDropper;

    [Header("Current Data")]
    public Transform target;
    public Vector2 currentPos, targetPos, diraction;
    public float currentHealth;

    [Header("Status")]
    public bool movementEnabler = true;
    public float movementDisableTimer = 0;
    public bool attackEnabler = true;
    public float attackDisableTimer = 0;
    public bool damageEnabler = true;
    public float damageDisableTimer = 0;
    public bool dodgeEnabler = true;
    public float dodgeDisableTimer = 0;
    public bool behaviourEnabler = true;

    public float Health
    {
        get
        {
            return currentHealth;
        }
        set
        {
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

    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D currentRb;
    Damageable damageableObject;



    void Start()
    {
        currentHealth = enemy.health;
        target = GameObject.FindWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        currentRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        damageableObject = target.GetComponentInParent<Damageable>();

        if (enemy.isBoss) gameObject.tag = "Boss";
    }

    void Update()
    {
        if (!behaviourEnabler) return;

        currentPos = transform.position;
        targetPos = target.transform.position;
        diraction = (targetPos - currentPos).normalized;

        Moving();
        Attacking();
        UpdateTimer();
    }

    private void Moving()
    {
        spriteRenderer.flipX = (currentPos.x - targetPos.x) > 0.2;

        switch (enemy.attackType)
        {
            case EnemySO.AttackType.Melee:

                if (!movementEnabler) return;

                if (Vector3.Distance(targetPos, currentPos) <= enemy.chaseField && Vector3.Distance(targetPos, currentPos) >= enemy.attackField)
                {
                    currentRb.MovePosition(currentPos + enemy.moveSpeed * Time.deltaTime * diraction);

                    animator.SetBool("ismove", true);
                    animator.SetBool("ischase", true);
                }
                else if (Vector3.Distance(targetPos, currentPos) > enemy.chaseField)
                {
                    animator.SetBool("ismove", false);
                    animator.SetBool("ischase", false);
                }
                else if (Vector3.Distance(targetPos, currentPos) < enemy.attackField)
                {
                    animator.SetBool("ismove", false);
                    animator.SetBool("ischase", true);
                }
                break;

            case EnemySO.AttackType.Sniper:

                if (!movementEnabler) return;

                if (Vector3.Distance(targetPos, currentPos) < enemy.chaseField)
                {
                    currentRb.MovePosition(currentPos - enemy.moveSpeed * Time.deltaTime * diraction);

                    animator.SetBool("ismove", true);
                    animator.SetBool("ischase", true);
                }
                else if (Vector3.Distance(targetPos, currentPos) > enemy.chaseField && Vector3.Distance(targetPos, currentPos) < enemy.attackField)
                {
                    currentRb.velocity = Vector2.zero;

                    animator.SetBool("ismove", false);
                    animator.SetBool("ischase", true);
                }
                else if (Vector3.Distance(targetPos, currentPos) > enemy.attackField)
                {
                    currentRb.velocity = Vector2.zero;

                    animator.SetBool("ismove", false);
                    animator.SetBool("ischase", false);
                }
                break;
        }
    }

    private void Attacking()
    {
        if (!attackEnabler) return;

        switch (enemy.attackType)
        {
            case EnemySO.AttackType.Melee:

                if (Vector3.Distance(targetPos, currentPos) < enemy.attackField)
                {
                    damageableObject.OnHit(enemy.attackDamage, false, diraction * enemy.knockbackForce, enemy.knockbackTime);

                    attackDisableTimer += enemy.attackSpeed;
                    movementDisableTimer += enemy.attackSpeed;
                }
                break;

            case EnemySO.AttackType.Sniper:

                if (Vector3.Distance(targetPos, currentPos) < enemy.attackField)
                {
                    enemy.Attack_Ranged(Mathf.Atan2(diraction.y, diraction.x) * Mathf.Rad2Deg, transform.position + new Vector3(0, 0.5f, 0));

                    attackDisableTimer += enemy.attackSpeed;
                }
                break;
        }
    }


    public void OnHit(float damage, bool isCrit, Vector2 knockbackForce, float knockbackTime)
    {
        if (UpdateTimer() && damageEnabler)
        {
            Health -= damage / (1 + (0.001f * enemy.defence));
            InstantiateDamageText(damage / (1 + (0.001f * enemy.defence)), isCrit ? "DamageCrit" : "Damage");

            //knockback
            currentRb.velocity = knockbackForce / (1 + (0.001f * enemy.defence));

            //delay
            damageDisableTimer += 0.2f;
            movementDisableTimer += knockbackTime / (1 + (0.001f * enemy.defence));
            attackDisableTimer += knockbackTime / (1 + (0.001f * enemy.defence));
        }
    }

    private bool UpdateTimer()
    {
        movementDisableTimer = Mathf.Max(0, movementDisableTimer - Time.deltaTime);
        attackDisableTimer = Mathf.Max(0, attackDisableTimer - Time.deltaTime);
        damageDisableTimer = Mathf.Max(0, damageDisableTimer - Time.deltaTime);
        dodgeDisableTimer = Mathf.Max(0, dodgeDisableTimer - Time.deltaTime);

        movementEnabler = movementDisableTimer <= 0;
        attackEnabler = attackDisableTimer <= 0;
        damageEnabler = damageDisableTimer <= 0;
        dodgeEnabler = dodgeDisableTimer <= 0;

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