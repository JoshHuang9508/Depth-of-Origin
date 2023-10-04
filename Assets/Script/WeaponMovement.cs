using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class WeaponMovement : MonoBehaviour
{
    public float attackSpeed = 1f;
    public float attackCooldown;
    public float weaponDamage = 1f;
    public float knockbackForce;
    public float knockbackTime;
    public float knockbackSpeed;

    SpriteRenderer spriteRenderer;
    Animator animator;
    SummonWeapon summonWeapon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damage_Interface damageableObject = collision.GetComponentInParent<Damage_Interface>();

        if (damageableObject != null)
        {
            if (collision.CompareTag("Enemy"))
            {
                Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
                Vector2 direction = (Vector2)(collision.gameObject.transform.position - parentPos).normalized;

                damageableObject.OnHit(weaponDamage, direction * knockbackForce * 1000, knockbackTime);
            }
        }
        else
        {
            //Debug.LogWarning("collision dont have implement IDamageable");
        }
    }

    public void WeaponSwing(bool isflip)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        summonWeapon = GetComponentInParent<SummonWeapon>();

        StartCoroutine(swing_animation(isflip));
    }

    private IEnumerator swing_animation(bool isflip)
    {
        //Debug.Log(startAngle - 90);

        animator.SetBool("isflip", isflip);
        spriteRenderer.flipX = isflip;
        animator.speed = attackSpeed;
        yield return new WaitForSeconds(0.2f / attackSpeed);
        yield return new WaitForSeconds(attackCooldown);

        //Debug.Log("cooldown over");

        summonWeapon.CooldownOver();
        Destroy(gameObject);
    }
}
