using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovementMelee : MonoBehaviour
{
    [Header("Object Settings")]
    public WeaponSO weapon;

    [Header("Status")]
    public bool isflip;

    public PlayerBehaviour player;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
}
