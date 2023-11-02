using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WeaponMovementRanged : MonoBehaviour
{
    [Header("Object Settings")]
    public RangedWeaponSO rangedWeapon;

    [Header("Read Only Value")]
    public Quaternion startAngle;
    public SummonWeapon summonWeapon;
    public PlayerBehaviour player;
    public Collider2D objectCollider;
    public Rigidbody2D objectRigidbody;
    public SpriteRenderer spriteRenderer;


    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(rangedWeapon.attackCooldown);
        summonWeapon.CooldownOver();
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
