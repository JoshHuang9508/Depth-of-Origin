using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new weapon", menuName = "Items/Weapon")]
public class WeaponSO : ItemSO,IDestoryableItem,IItemAction
{
    [Header("Basic Data")]
    public float attackSpeed = 1f;
    public float attackCooldown;
    public float weaponDamage = 1f;
    public float knockbackForce;
    public float knockbackTime;
    public GameObject weaponObject;

    public string ActionName => "Equip";

    public bool PerformAction(GameObject player, List<ItemParameter> itemState = null)
    {
        PlayerBehaviour weapon = player.GetComponent<PlayerBehaviour>();
        if (weapon != null)
        {
            weapon.SetWeapon(this);
            return true;
        }
        return false;
    }
}
