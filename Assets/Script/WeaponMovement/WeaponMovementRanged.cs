using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WeaponMovementRanged : MonoBehaviour
{
    [Header("Object Settings")]
    public RangedWeaponSO rangedWeapon;

    [Header("Status")]
    public Quaternion startAngle;

    public PlayerBehaviour player;
    public Rigidbody2D objectRigidbody;
    public SpriteRenderer spriteRenderer;
}
