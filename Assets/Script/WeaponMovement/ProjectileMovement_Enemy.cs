using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ProjectileMovement_Enemy : WeaponMovementRanged
{
    [Header("Connect Object")]
    public EnemySO enemy;

    [Header("Read Only Value")]
    public Animator animator;
    public TrailRenderer trail;
    public Light2D spriteLight;

    private void Start()
    {
        objectRigidbody = GetComponent<Rigidbody2D>();

        ProjectileFly(startAngle);
        StartCoroutine(DestroyCooldown());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageableObject = collision.GetComponentInParent<Damageable>();

        if (damageableObject != null)
        {
            if (collision.CompareTag("PlayerHitBox"))
            {
                Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
                Vector2 direction = (Vector2)(collision.gameObject.transform.position - parentPos).normalized;

                damageableObject.OnHit(
                    enemy.attackDamage,
                    false,
                    direction * enemy.knockbackForce,
                    enemy.knockbackTime);

                Destroy(gameObject);
            }
        }

        if (collision.CompareTag("Wall") || collision.CompareTag("BreakableObject") || collision.CompareTag("Object"))
        {
            Destroy(gameObject);
        }
    }

    public void ProjectileFly(Quaternion angle)
    {
        Vector3 angleVec3 = angle.eulerAngles;
        objectRigidbody.velocity = new Vector3(
            enemy.projectileFlySpeed * Mathf.Cos(angleVec3.z * Mathf.Deg2Rad),
            enemy.projectileFlySpeed * Mathf.Sin(angleVec3.z * Mathf.Deg2Rad),
            0);
    }

    private IEnumerator DestroyCooldown()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
