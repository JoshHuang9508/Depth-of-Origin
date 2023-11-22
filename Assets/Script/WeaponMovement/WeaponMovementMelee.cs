using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovementMelee : MonoBehaviour
{
    [Header("Object Reference")]
    public WeaponSO weapon;
    public PlayerBehaviour player;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    [Header("Status")]
    public bool isflip;
}
