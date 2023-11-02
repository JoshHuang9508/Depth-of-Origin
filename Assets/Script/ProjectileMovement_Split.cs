using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ProjectileMovement_Split : WeaponMovementRanged
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

        for (int i = -60; i <= 60; i+= 30)
        {
            Debug.Log(transform.eulerAngles.z);
            var splitArrowSummoned = Instantiate(
            rangedWeapon.projectileObject,
            transform.position,
            Quaternion.Euler(0, 0, transform.eulerAngles.z + i),
            GameObject.FindWithTag("Item").transform);

            splitArrowSummoned.AddComponent<ProjectileMovement_Straight>();
            splitArrowSummoned.GetComponent<WeaponMovementRanged>().rangedWeapon = rangedWeapon;
            splitArrowSummoned.GetComponent<WeaponMovementRanged>().startAngle = Quaternion.Euler(0, 0, transform.eulerAngles.z + i + 90);
        }

        Destroy(gameObject);
    }
}
