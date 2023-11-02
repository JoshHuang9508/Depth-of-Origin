using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMovement_0Deg : WeaponMovementMelee
{
    Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damage_Interface damageableObject = collision.GetComponentInParent<Damage_Interface>();

        if (damageableObject != null)
        {
            if (collision.CompareTag("HitBox") || collision.CompareTag("BreakableObject"))
            {
                Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
                Vector2 direction = (Vector2)(collision.gameObject.transform.position - parentPos).normalized;

                bool isCrit = Random.Range(0f, 100f) <= player.critRate ? true : false;

                damageableObject.OnHit(
                    weapon.weaponDamage * (1 + (0.01f * player.strength)) * (isCrit ? 1 + (0.01f * player.critDamage) : 1),
                    isCrit,
                    direction * weapon.knockbackForce,
                    weapon.knockbackTime);

                //camera shake
                CameraShake cameraShake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
                StartCoroutine(cameraShake.Shake(0.1f, 0.2f));
            }
        }
    }

    private void Start()
    {
        WeaponSwing(isflip);
    }

    public void WeaponSwing(bool _isflip)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        summonWeapon = GetComponentInParent<SummonWeapon>();
        player = GetComponentInParent<PlayerBehaviour>();

        spriteRenderer.flipX = _isflip;
        spriteRenderer.transform.Rotate(Vector3.forward, 90f);
        animator.speed = weapon.attackSpeed;
        animator.SetBool("isflip", _isflip);
        animator.SetTrigger("swing");
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(weapon.attackCooldown);
        summonWeapon.CooldownOver();
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
