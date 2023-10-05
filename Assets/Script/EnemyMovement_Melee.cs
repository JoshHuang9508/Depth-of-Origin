using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement_Melee : EnemyBasicLogic, Damage_Interface
{
    float movement_x;
    float movement_y;
    bool movementEnabler = true;
    bool damageEnabler = true;
    bool attackEnabler = true;

    SpriteRenderer spriteRenderer;
    Animator animator;
    Transform target;
    Rigidbody2D currentRb;

    Damage_Interface damageableObject;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        animator =GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currentRb = GetComponent<Rigidbody2D>();
        damageableObject = target.GetComponentInParent<Damage_Interface>();
    }

    void Update()
    {
        Moving();
    }

    void Moving()
    {
        if (Vector3.Distance(target.position, this.transform.position) <= chaseField && Vector3.Distance(target.position, this.transform.position) >= attackField && movementEnabler)
        {
            movement_x = (this.transform.position.x - target.position.x <= 0) ? 1 : -1;
            movement_y = (this.transform.position.y - target.position.y <= 0) ? 1 : -1;
            currentRb.velocity = new Vector3(movement_x * moveSpeed, movement_y * moveSpeed, 0.0f);

            //play animation
            animator.SetBool("ismove", true);
            animator.SetBool("ischase", true);
        }
        else if (Vector3.Distance(target.position, this.transform.position) > chaseField && movementEnabler)
        {
            currentRb.velocity = new Vector2(0.0f, 0.0f);

            //play animation
            animator.SetBool("ismove", false);
            animator.SetBool("ischase", false);
        }
        else if (Vector3.Distance(target.position, this.transform.position) < attackField && movementEnabler)
        {
            currentRb.velocity = new Vector2(0.0f, 0.0f);
            Attacking();

            //play animation
            animator.SetBool("ismove", false);
            animator.SetBool("ischase", true);
        }

        if (this.transform.position.x - target.position.x > 0.2)
        {
            spriteRenderer.flipX = true;
        }
        else if (this.transform.position.x - target.position.x < -0.2)
        {
            spriteRenderer.flipX = false;
        }
    }

    void Attacking()
    {
        if (attackEnabler)
        {
            //Debug.Log("enemy trying to attack");

            Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
            Vector2 direction = (Vector2)(target.gameObject.transform.position - parentPos).normalized;

            damageableObject.OnHit(attackDamage, direction * knockbackForce, knockbackTime);
            StartCoroutine(attack_delay());
        }
    }

    public void OnHit(float damage, Vector2 knockbackForce, float knockbackTime)
    {
        if (damageEnabler)
        {
            Health -= damage;
            currentRb.velocity = knockbackForce;
            StartCoroutine(damage_delay(knockbackTime));
            StartCoroutine(knockback_delay(knockbackTime));
        }
    }

    private IEnumerator knockback_delay(float knockbackTime)
    {
        animator.enabled = false;
        movementEnabler = false;
        yield return new WaitForSeconds(knockbackTime);
        animator.enabled = true;
        movementEnabler = true;
    }

    private IEnumerator damage_delay(float knockbackTime)
    {
        damageEnabler = false;
        yield return new WaitForSeconds(knockbackTime);
        damageEnabler = true;
    }
    
    private IEnumerator attack_delay()
    {
        attackEnabler = false;
        yield return new WaitForSeconds(attackSpeed);
        attackEnabler = true;
    }
}
