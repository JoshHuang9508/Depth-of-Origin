using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ProjectileMovement_Player : WeaponMovementRanged
{
    private void Start()
    {
        objectRigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        ProjectileFly(startAngle);
        StartCoroutine(DestroyCooldown());
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

                bool isCrit = Random.Range(0f, 100f) <= player.critRate;

                damageableObject.OnHit(
                    rangedWeapon.weaponDamage * (1 + (0.01f * player.strength)) * (isCrit ? 1 + (0.01f * player.critDamage) : 1),
                    isCrit,
                    direction * rangedWeapon.knockbackForce,
                    rangedWeapon.knockbackTime);

                Destroy(gameObject);
            }
        }

        if (collision.CompareTag("Wall") || collision.CompareTag("Shield") || collision.CompareTag("Object"))
        {
            Destroy(gameObject);
        }
    }

    public void ProjectileFly(Quaternion angle)
    {
        Vector3 angleVec3 = angle.eulerAngles;
        objectRigidbody.velocity = new Vector3(
            rangedWeapon.flySpeed * Mathf.Cos(angleVec3.z * Mathf.Deg2Rad),
            rangedWeapon.flySpeed * Mathf.Sin(angleVec3.z * Mathf.Deg2Rad),
            0);
    }

    private IEnumerator DestroyCooldown()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
