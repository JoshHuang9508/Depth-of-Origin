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
                    //MeleeWeaponSO meleeWeaponSO = weapon as MeleeWeaponSO;

                    var meleeWeaponSummoned = Instantiate(
                        weapon.weaponObject,
                        transform.position, 
                        Quaternion.identity, 
                        this.transform);

                    meleeWeaponSummoned.GetComponent<WeaponMovementMelee>().weapon = weapon;
                    meleeWeaponSummoned.GetComponent<WeaponMovementMelee>().isflip = isflip;

                    transform.rotation = Quaternion.Euler(0, 0, startAngle - 90);

                    StartCoroutine(Cooldown(weapon.attackCooldown));
                    break;

                case WeaponSO.WeaponType.Ranged:
                    RangedWeaponSO rangedWeapon = weapon as RangedWeaponSO;

                    switch (rangedWeapon.projectileType)
                    {
                        case RangedWeaponSO.ProjectileType.Straight:
                            var arrowSummoned = Instantiate(
                                rangedWeapon.projectileObject,
                                transform.position,
                                Quaternion.Euler(0, 0, startAngle - 90),
                                GameObject.FindWithTag("Item").transform);

                            arrowSummoned.AddComponent<ProjectileMovement_Player>();
                            arrowSummoned.GetComponent<WeaponMovementRanged>().rangedWeapon = rangedWeapon;
                            arrowSummoned.GetComponent<WeaponMovementRanged>().startAngle = Quaternion.Euler(0, 0, startAngle);

                            StartCoroutine(Cooldown(weapon.attackCooldown));
                            break;

                        case RangedWeaponSO.ProjectileType.Split:
                            for (int i = -60; i <= 60; i += 30)
                            {
                                var splitArrowSummoned = Instantiate(
                                    rangedWeapon.projectileObject,
                                    transform.position,
                                    Quaternion.Euler(0, 0, startAngle + i - 90),
                                    GameObject.FindWithTag("Item").transform);

                                splitArrowSummoned.AddComponent<ProjectileMovement_Player>();
                                splitArrowSummoned.GetComponent<WeaponMovementRanged>().rangedWeapon = rangedWeapon;
                                splitArrowSummoned.GetComponent<WeaponMovementRanged>().startAngle = Quaternion.Euler(0, 0, startAngle + i);
                            }

                            StartCoroutine(Cooldown(weapon.attackCooldown));
                            break;
                    }
                    break;
            }
        }
    }

    private IEnumerator Cooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        summonEnabler = true;
    }
}
