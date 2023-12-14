using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;

[CreateAssetMenu(fileName = "new melee weapon", menuName = "Items/Weapon/Melee Weapon")]
public class MeleeWeaponSO : WeaponSO, IEquipable, IUnequipable
{
    [Header("Object Reference")]
    public GameObject weaponObject;

    [Header("Melee Weapon Setting")]
    public float attackSpeed = 1f;


    public bool EquipObject(int amount, GameObject character, List<ItemParameter> itemState = null)
    {
        PlayerBehaviour player = character.GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            player.SetEquipment(this, WeaponType.Melee);
        }
        return false;
    }

    public bool UnequipObject(int amount, GameObject character, List<ItemParameter> itemstate)
    {
        PlayerBehaviour player = character.GetComponent<PlayerBehaviour>();

        if (player != null)
        {
            player.UnEquipment(this, WeaponType.Melee);
        }
        return false;
    }
}
