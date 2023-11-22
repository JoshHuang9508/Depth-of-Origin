using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WeaponSO : ItemSO, IEquipable, IDestoryableItem, ISellable, IBuyable, IUnequipable
{
    [Header("Setting")]
    public WeaponType weaponType;
    public float attackSpeed = 1f;
    public float attackCooldown;
    public float weaponDamage = 1f;
    public float knockbackForce;
    public float knockbackTime;

    [Header("Effection")]
    public float E_walkSpeed;
    public float E_maxHealth;
    public float E_strength;
    public float E_defence;
    public float E_critRate;
    public float E_critDamage;

    [Header("Reference")]
    public GameObject weaponObject;

    public enum WeaponType
    {
        Melee, Ranged
    }


    public bool EquipObject(int amount, GameObject character, List<ItemParameter> itemState = null)
    {
        PlayerBehaviour player = character.GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            player.SetEquipment(this, weaponType);
        }
        return false;
    }

    public bool UnequipObject(int amount, GameObject character, List<ItemParameter> itemstate)
    {
        PlayerBehaviour player = character.GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            player.UnEquipment(this, weaponType);
        }
        return false;
    }
}
