using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ProjectileMovement_Unlimited : WeaponMovementRanged
{
    [Header("Read Only Value")]
    public Animator animator;
    public TrailRenderer trail;
    public Light2D spriteLight;
    public float summonGap;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trail = GetComponentInChildren<TrailRenderer>();
        objectRigidbody = GetComponent<Rigidbody2D>();
        objectCollider = GetComponent<Collider2D>();
        spriteLight = GetComponentInChildren<Light2D>();
        summonWeapon = GameObject.FindWithTag("Player").GetComponentInChildren<SummonWeapon>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        SummonProjectile();
        DisableItem();
    }

    private void Update()
    {
        summonGap += Time.deltaTime;
        if (summonGap >= 0.3 && Input.GetKey(KeyCode.Mouse0))
        {
            summonGap = 0;
            SummonProjectile();
        }
        if(!Input.GetKey(KeyCode.Mouse0))
        {
            summonWeapon.CooldownOver();
            Destroy(gameObject);
        }
    }

    private void SummonProjectile()
    {
        Debug.Log(transform.eulerAngles.z);
        var splitArrowSummoned = Instantiate(
        rangedWeapon.projectileObject,
        summonWeapon.transform.position,
        Quaternion.Euler(0, 0, summonWeapon.transform.eulerAngles.z - 90),
        GameObject.FindWithTag("Item").transform);

        splitArrowSummoned.AddComponent<ProjectileMovement_Straight>();
        splitArrowSummoned.GetComponent<WeaponMovementRanged>().rangedWeapon = rangedWeapon;
        splitArrowSummoned.GetComponent<WeaponMovementRanged>().startAngle = Quaternion.Euler(0, 0, summonWeapon.transform.eulerAngles.z);
    }

    private void DisableItem()
    {
        spriteRenderer.enabled = false;
        objectCollider.enabled = false;
        trail.emitting = false;
        spriteLight.enabled = false;
    }
}
