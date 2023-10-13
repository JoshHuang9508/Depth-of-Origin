using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new equippableItem", menuName = "Items/Equippable Itme")]
public class EquippableItemSO : ItemSO
{
    [Header("Effect settings")]
    public float E_walkSpeed;
    public float E_health;
    public float E_attackSpeed = 1f;
    public float E_attackCooldown;
    public float E_weaponDamage = 1f;
    public float E_knockbackForce;
    public float E_knockbackTime;

    public string ActionName => "Equip";

    public bool PerformAction(GameObject player, List<ItemParameter> itemState = null)
    {
        /*AgentWeapon weapon = player.GetComponent<AgentWeapon>();
        if (weapon != null)
        {
            weapon.SetWeapon(this, itemState == null ? DefaultParameterList : itemState);
            return true;
        }*/
        return false;
    }
}