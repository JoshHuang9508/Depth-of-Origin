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
    public EffectType effecttype;
    public enum EffectType
    {
        armor, book, jewelry
    }

    public string ActionName => "Equip";

    public bool PerformAction(GameObject _player, List<ItemParameter> itemState = null)
    {
        PlayerBehaviour player = _player.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            player.SetEquipment(this, effecttype);
            return true;
        }
        return false;
    }
}