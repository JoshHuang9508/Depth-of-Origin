using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new equippableItem", menuName = "Items/Equippable Itme")]
public class EquippableItemSO : ItemSO,IItemAction,IDestoryableItem
{
    [Header("Effect settings")]
    public float E_walkSpeed;
    public float E_health;
    public float E_attackSpeed = 1f;
    public float E_attackCooldown;
    public float E_weaponDamage = 1f;
    public float E_knockbackForce;
    public float E_knockbackTime;
    EffectType effecttype;
    public enum EffectType
    {
        armor, book, jewelry
    }

    public string ActionName => "Equip";

    public bool PerformAction(GameObject player, List<ItemParameter> itemState = null)
    {
        switch (effecttype)
        {
            case EffectType.armor:

                break;
            case EffectType.book:

                break;
            case EffectType.jewelry:

                break;
        }
        return false;
    }
}