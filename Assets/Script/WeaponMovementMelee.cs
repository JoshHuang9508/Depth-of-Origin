using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovementMelee : MonoBehaviour
{
    public WeaponSO weapon;

    SpriteRenderer spriteRenderer;
    Animator animator;
    SummonWeapon summonWeapon;
    PlayerBehaviour player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damage_Interface damageableObject = collision.GetComponentInParent<Damage_Interface>();

        if (damageableObject != null) {
            if (collision.CompareTag("HitBox") || collision.CompareTag("BreakableObject")) {
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
        } else {
            //Debug.LogWarning("collision dont have implement IDamageable");
        }
    }

    public void WeaponSwing(bool isflip)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        summonWeapon = GetComponentInParent<SummonWeapon>();
        player = GetComponentInParent<PlayerBehaviour>();

        spriteRenderer.flipX = isflip;
        animator.speed = weapon.attackSpeed;
        animator.SetBool("isflip", isflip);
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
