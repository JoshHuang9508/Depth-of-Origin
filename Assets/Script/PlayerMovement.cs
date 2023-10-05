using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerBasicLogic, Damage_Interface
{
    bool movementEnabler = true;

    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D currentRb;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currentRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }

    void Moving()
    {
        if (movementEnabler)
        {
            animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
            animator.SetFloat("Vertical", Input.GetAxis("Vertical"));

            Vector3 movement = new Vector3(Input.GetAxis("Horizontal") * walkSpeed, Input.GetAxis("Vertical") * walkSpeed, 0.0f);
            currentRb.velocity = new Vector2(movement.x, movement.y);

            spriteRenderer.flipX = Input.GetAxis("Horizontal") < 0 ? true : false;
        }
    }

    public void OnHit(float damage, Vector2 knockbackForce, float knockbackTime)
    {
        Health -= damage;

        StopCoroutine(knockback_delay(knockbackTime));
        currentRb.velocity = knockbackForce;
        StartCoroutine(knockback_delay(knockbackTime));
    }

    private IEnumerator knockback_delay(float knockbackTime)
    {
        animator.enabled = false;
        movementEnabler = false;
        yield return new WaitForSeconds(knockbackTime + 0.2f);
        animator.enabled = true;
        movementEnabler = true;
    }
}
