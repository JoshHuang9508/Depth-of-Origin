using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ProjectileMovement_Straight : WeaponMovementRanged
{
    [Header("Read Only Value")]
    public Animator animator;
    public TrailRenderer trail;
    public Light2D spriteLight;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trail = GetComponentInChildren<TrailRenderer>();
        objectRigidbody = GetComponent<Rigidbody2D>();
        objectCollider = GetComponent<Collider2D>();
        spriteLight = GetComponentInChildren<Light2D>();
        summonWeapon = GameObject.FindWithTag("Player").GetComponentInChildren<SummonWeapon>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        ProjectileFly(startAngle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageableObject = collision.GetComponentInParent<Damageable>();

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

        if (collision.CompareTag("Wall") || collision.CompareTag("Shield") || collision.CompareTag("Object"))
        {
            DisableItem();
        }
    }

    public void ProjectileFly(Quaternion angle)
    {
        Vector3 angleVec3 = angle.eulerAngles;
        objectRigidbody.velocity = new Vector3(rangedWeapon.flySpeed * Mathf.Cos(angleVec3.z * Mathf.Deg2Rad), rangedWeapon.flySpeed * Mathf.Sin(angleVec3.z * Mathf.Deg2Rad), 0);
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
        objectCollider.enabled = false;
        trail.emitting = false;
        spriteLight.enabled = false;
    }
}
