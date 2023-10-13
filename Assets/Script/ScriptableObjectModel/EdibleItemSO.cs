using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new edibleItem", menuName = "Items/Edible Itme")]
public class EdibleItemSO : ItemSO
{
    [Header("Effect settings")]
    public float E_walkSpeed;
    public float E_health;
    public float E_attackSpeed = 1f;
    public float E_attackCooldown;
    public float E_weaponDamage = 1f;
    public float E_knockbackForce;
    public float E_knockbackTime;

    public string ActionName => "Consume";

    [SerializeField] private List<ModifierData> modifiersData = new List<ModifierData>();

    [Serializable]
    public class ModifierData
    {
        public EffectType effectType;
        //public CharacterStatModifierSO statModifier;
        public float value;
    }

    public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
    {
        PlayerBehaviour player = character.GetComponent<PlayerBehaviour>();

        if (player == null) return true;

        foreach (ModifierData data in modifiersData)
        {
            Debug.Log("Use");

            switch (data.effectType)
            {
                case EffectType.a:
                    Debug.Log("a");
                    break;
            }
        }
        return true;
    }

    public enum EffectType
    {
        a, b, c, d
    }
}

public interface IDestoryableItem
{

}

public interface IItemAction
{
    public string ActionName { get; }
    bool PerformAction(GameObject character, List<ItemParameter> itemState);

}