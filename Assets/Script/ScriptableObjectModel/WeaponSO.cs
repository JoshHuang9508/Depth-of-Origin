using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new weapon", menuName = "Items/Weapon")]
public class WeaponSO : ItemSO, IDestoryableItem, IItemAction
{
    [Header("Basic Data")]
    public float attackSpeed = 1f;
    public float attackCooldown;
    public float weaponDamage = 1f;
    public float knockbackForce;
    public float knockbackTime;
    public float E_walkSpeed;
    public float E_maxHealth;
    public float E_strength;
    public float E_defence;
    public float E_critRate;
    public float E_critDamage;
    public GameObject weaponObject;

    public string ActionName => "Equip";
    public bool PerformAction(GameObject _player, List<ItemParameter> itemState = null)
    {
        PlayerBehaviour player = _player.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            player.SetEquipment(this);
            return true;
        }
        return false;
    }
}
