using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WeaponMovementRanged : MonoBehaviour
{
    [Header("Object Reference")]
    public RangedWeaponSO rangedWeapon;
    public PlayerBehaviour player;
    public Rigidbody2D objectRigidbody;
    public SpriteRenderer spriteRenderer;

    [Header("Dynamic Data")]
    public Quaternion startAngle;
}
