using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerBasicLogic, Damage_Interface
{
    bool movementEnabler = true;
    bool sprintEnabler = false;
    int walkSpeedMutiplyer = 1;

    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D currentRb;

    public KeyCode sprintKey;

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

        if (Input.GetKeyDown(sprintKey)) Sprint();
    }
     
    void Moving()
    {
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        spriteRenderer.flipX = Input.GetAxis("Horizontal") < 0 ? true : false;

        if (movementEnabler && Input.anyKey) {

            Vector3 movement = new Vector3(
                Input.GetAxis("Horizontal") * walkSpeed * walkSpeedMutiplyer, 
                Input.GetAxis("Vertical") * walkSpeed * walkSpeedMutiplyer, 
                0.0f
            );
            currentRb.velocity = new Vector2(movement.x, movement.y);
        }
    }

    void Sprint()
    {
        if (!sprintEnabler && movementEnabler)
        {
            StartCoroutine(sprint_delay());
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
        yield return new WaitForSeconds(knockbackTime / 1);
        animator.enabled = true;
        movementEnabler = true;
    }

    private IEnumerator sprint_delay()
    {
        sprintEnabler = true;
        walkSpeedMutiplyer = 3;
        yield return new WaitForSeconds(0.2f);
        walkSpeedMutiplyer = 1;
        yield return new WaitForSeconds(2f);
        sprintEnabler = false;
    }
}
