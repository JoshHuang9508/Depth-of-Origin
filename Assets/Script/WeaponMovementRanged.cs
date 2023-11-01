using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovementRanged : MonoBehaviour
{
    public RangedWeaponSO rangedWeapon;

    SpriteRenderer spriteRenderer;
    Collider2D co;
    Animator animator;
    SummonWeapon summonWeapon;
    Rigidbody2D rb;
    PlayerBehaviour player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);

        Damage_Interface damageableObject = collision.GetComponentInParent<Damage_Interface>();

        if (damageableObject != null)
        {
            if (collision.CompareTag("HitBox") || collision.CompareTag("BreakableObject"))
            {
                Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
                Vector2 direction = (Vector2)(collision.gameObject.transform.position - parentPos).normalized;

                bool isCrit = Random.Range(0f, 100f) <= player.critRate ? true : false;

                damageableObject.OnHit(
                    rangedWeapon.weaponDamage * (1 + (0.01f * player.strength)) * (isCrit ? 1 + (0.01f * player.critDamage) : 1),
                    isCrit,
                    direction * rangedWeapon.knockbackForce,
                    rangedWeapon.knockbackTime);

                DisableItem();
            }
        }

        if (collision.CompareTag("Wall"))
        {
            DisableItem();
        }
    }

    public void ProjectileFly(Quaternion angle)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        co = GetComponent<Collider2D>();
        summonWeapon = GameObject.FindWithTag("Player").GetComponentInChildren<SummonWeapon>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        Vector3 angleVec3 = angle.eulerAngles;
        rb.velocity = new Vector3(rangedWeapon.flySpeed * Mathf.Cos(angleVec3.z * Mathf.Deg2Rad), rangedWeapon.flySpeed * Mathf.Sin(angleVec3.z * Mathf.Deg2Rad), 0);
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(rangedWeapon.attackCooldown);
        summonWeapon.CooldownOver();
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    private void DisableItem()
    {
        spriteRenderer.enabled = false;
        co.enabled = false;
    }
}
