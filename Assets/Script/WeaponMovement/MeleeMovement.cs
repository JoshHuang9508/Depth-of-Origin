using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMovement : WeaponMovementMelee
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageableObject = collision.GetComponentInParent<Damageable>();

        if (damageableObject != null)
        {
            if (collision.CompareTag("HitBox") || collision.CompareTag("BreakableObject"))
            {
                Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
                Vector2 direction = (Vector2)(collision.gameObject.transform.position - parentPos).normalized;

                bool isCrit = Random.Range(0f, 100f) <= player.critRate;

                damageableObject.OnHit(
                    weapon.weaponDamage * (1 + (0.01f * player.strength)) * (isCrit ? 1 + (0.01f * player.critDamage) : 1),
                    isCrit,
                    direction * weapon.knockbackForce,
                    weapon.knockbackTime);

                //camera shake
                CameraController camera = GameObject.FindWithTag("MainCamera").GetComponentInParent<CameraController>();
                StartCoroutine(camera.Shake(0.1f, 0.2f));
            }
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GetComponentInParent<PlayerBehaviour>();

        WeaponSwing(isflip);
    }

    public void WeaponSwing(bool _isflip)
    {
        spriteRenderer.flipX = _isflip;
        spriteRenderer.transform.Rotate(Vector3.forward, 90f);
        animator.speed = weapon.attackSpeed;
        animator.SetBool("isflip", _isflip);
        animator.SetTrigger("swing");
    }
}
