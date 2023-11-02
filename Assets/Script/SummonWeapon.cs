using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonWeapon : MonoBehaviour
{
    public Interactable Interactable;
    public WeaponSO weapon;
    public PlayerBehaviour player;
    SpriteRenderer spriteRenderer;

    bool isflip;
    float startAngle;
    bool summonEnabler = true;

    Vector2 mousePos;
    Vector2 currentPos;
    Vector2 Diraction;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentPos = transform.position;
        Diraction = (mousePos - currentPos).normalized;
        startAngle = Mathf.Atan2(Diraction.y, Diraction.x) * Mathf.Rad2Deg;

        if (Diraction.x < 0)
        {
            isflip = true;
        }
        else if (Diraction.x >= 0)
        {
            isflip = false;
        }


        if (player.currentWeapon == null)
        {
            spriteRenderer.sprite = null;
            weapon = null;
        }
        else if (player.currentWeapon is WeaponSO && (summonEnabler ? weapon = player.currentWeapon : true))
        {
            switch (weapon.weaponType)
            {
                case WeaponSO.WeaponType.Melee:
                    spriteRenderer.sprite = null;
                    break;
                case WeaponSO.WeaponType.Ranged:
                    spriteRenderer.sprite = weapon.Image;
                    transform.rotation = Quaternion.Euler(0, 0, startAngle);
                    break;
            }
        }
    }

    public void Summon()
    {
        if (summonEnabler && weapon != null)
        {
            for (var i = this.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(this.transform.GetChild(i).gameObject);
            }

            summonEnabler = false;

            switch (weapon.weaponType)
            {
                case WeaponSO.WeaponType.Melee:
                    GameObject meleeWeaponSummoned = Instantiate(weapon.weaponObject, new Vector3(
                        transform.position.x, transform.position.y, transform.position.z), 
                        new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), 
                        this.transform);
                    meleeWeaponSummoned.GetComponent<WeaponMovementMelee>().weapon = weapon;
                    meleeWeaponSummoned.GetComponent<WeaponMovementMelee>().isflip = isflip;
                    transform.rotation = Quaternion.Euler(0, 0, startAngle - 90);
                    break;
                case WeaponSO.WeaponType.Ranged:
                    RangedWeaponSO rangedWeapon = weapon as RangedWeaponSO;

                    GameObject rangedWeaponSummoned = Instantiate(rangedWeapon.projectileObject, new Vector3(
                        transform.position.x, transform.position.y, transform.position.z), 
                        Quaternion.Euler(0, 0, startAngle - 90), 
                        GameObject.FindWithTag("Item").transform);
                    switch (rangedWeapon.projectileType)
                    {
                        case RangedWeaponSO.ProjectileType.Straight:
                            rangedWeaponSummoned.AddComponent<ProjectileMovement_Straight>();
                            break;
                        case RangedWeaponSO.ProjectileType.Split:
                            rangedWeaponSummoned.AddComponent<ProjectileMovement_Split>();
                            break;
                        case RangedWeaponSO.ProjectileType.Unlimited:
                            rangedWeaponSummoned.AddComponent<ProjectileMovement_Unlimited>();
                            break;
                    }
                    rangedWeaponSummoned.GetComponent<WeaponMovementRanged>().rangedWeapon = rangedWeapon;
                    rangedWeaponSummoned.GetComponent<WeaponMovementRanged>().startAngle = Quaternion.Euler(0, 0, startAngle);
                    break;
            }
        }
    }

    public void CooldownOver()
    {
        summonEnabler = true;
    }
}
