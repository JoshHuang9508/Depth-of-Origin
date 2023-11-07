using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovementMelee : MonoBehaviour
{
    [Header("Object Settings")]
    public WeaponSO weapon;

    [Header("Read Only Value")]
    public Quaternion startAngle;
    public bool isflip;
    public SpriteRenderer spriteRenderer;
    public SummonWeapon summonWeapon;
    public PlayerBehaviour player;
}
