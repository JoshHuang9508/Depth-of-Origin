using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new weapon", menuName = "Items/Weapon")]
public class WeaponSO : ItemSO
{
    [Header("Basic Data")]
    public float attackSpeed = 1f;
    public float attackCooldown;
    public float weaponDamage = 1f;
    public float knockbackForce;
    public float knockbackTime;
    public GameObject weaponObject;

    public bool PerformAction(GameObject player, List<ItemParameter> itemState = null)
    {
        AgentWeapon weapon = player.GetComponent<AgentWeapon>();
        if (weapon != null)
        {
            weapon.SetWeapon(this, itemState == null ? DefaultParameterList : itemState);
            return true;
        }
        return false;
    }
}
