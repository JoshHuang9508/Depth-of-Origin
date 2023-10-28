using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "new weapon", menuName = "Items/Weapon")]
public class WeaponSO : ItemSO, IEquipable, IDestoryableItem, IItemAction,ISellable
{
    [Header("Basic Data")]
    public float attackSpeed = 1f;
    public float attackCooldown;
    public float weaponDamage = 1f;
    public float knockbackForce;
    public float knockbackTime;
    public WeaponType weaponType;

    public int price;
    public enum WeaponType
    {
        Melee, Ranged
    }

    [Header("Effect settings")]
    public float E_walkSpeed;
    public float E_maxHealth;
    public float E_strength;
    public float E_defence;
    public float E_critRate;
    public float E_critDamage;
    public GameObject weaponObject;

    public bool EquipObject(int amount, GameObject character, List<ItemParameter> itemState = null)
    {
        PlayerBehaviour player = character.GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            player.SetEquipment(this, weaponType);
        }
        return false;
    }

    public bool SellObject(int amount, GameObject character, List<ItemParameter> itemstate)
    {
        PlayerBehaviour player = character.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            player.coinAmount += price;
        }
        return false;
    }
}
