using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement_Melee : EnemyBasicLogic, Damage_Interface
{
    float movement_x;
    float movement_y;
    float time_in_attackField = 0;
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

    // Update is called once per frame
    void Update()
    {
        moving();
    }

    void moving()
    {
        if (Vector3.Distance(target.position, this.transform.position) <= chaseField && Vector3.Distance(target.position, this.transform.position) >= attackField && movementEnabler)
        {
            movement_x = (this.transform.position.x - target.position.x <= 0) ?  1  :  -1 ;
            movement_y = (this.transform.position.y - target.position.y <= 0) ?  1  :  -1 ;
            Vector3 movement = new Vector3(movement_x * moveSpeed, movement_y * moveSpeed, 0.0f);
            currentRb.velocity = new Vector2(movement.x,  movement.y);

            animator.SetBool("ismove",true);
            animator.SetBool("ischase", true);
        }
        else if (Vector3.Distance(target.position, this.transform.position) > chaseField && movementEnabler)
        {
            currentRb.velocity = new Vector2(0.0f, 0.0f);

            animator.SetBool("ismove", false);
            animator.SetBool("ischase", false);
        }
        else if (Vector3.Distance(target.position, this.transform.position) < attackField  && movementEnabler)
        {
            attacking();

            animator.SetBool("ismove",false);
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

    void attacking()
    {
        time_in_attackField += Time.deltaTime;

        if (time_in_attackField >= attackSpeed * 10 * Time.deltaTime)
        {
            Debug.Log("enemy trying to attack");

            Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
            Vector2 direction = (Vector2)(target.gameObject.transform.position - parentPos).normalized;

            damageableObject.OnHit(attackDamage, direction * knockbackForce * 1000, knockbackTime);

            time_in_attackField = 0;
        }
    }

    public void OnHit(float damage, Vector2 knockbackForce, float knockbackTime)
    {
        if (damageEnabler)
        {
            Health -= damage;
            StartCoroutine(damage_delay());
        }

        StopCoroutine(knockback_delay(knockbackTime));
        currentRb.AddForce(knockbackForce);
        StartCoroutine(knockback_delay(knockbackTime));
    }

    private IEnumerator knockback_delay(float knockbackTime)
    {
        movementEnabler = false;
        yield return new WaitForSeconds(knockbackTime);
        movementEnabler = true;
    }

    private IEnumerator damage_delay()
    {
        damageEnabler = false;
        yield return new WaitForSeconds(0.2f);
        damageEnabler = true;
    }

    
}
