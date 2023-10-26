using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Inventory.Model;
using UnityEngine.Tilemaps;

namespace Inventory.UI
{
    public class UIInventoryDescription : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;

        private void Awake()
        {
            ResetDescription();
        }

        public void ResetDescription()
        {
            itemImage.gameObject.SetActive(false);
            title.gameObject.SetActive(false);
            description.gameObject.SetActive(false);
        }

        string b;

        public void SetDescription(ItemSO item)
        {
            SetImage(item);
            SetTitle(item);

            description.gameObject.SetActive(true);
            try
            {
                var weapon = (WeaponSO)item;
                b = $"- Damage : {weapon.weaponDamage}\n" +
                    $"- ATK Speed : {weapon.attackSpeed}x\n" +
                    $"- ATK CD : {weapon.attackCooldown}s\n" +
                    $"- Knockback : {weapon.knockbackForce}\n" +
                    $"- Stunned : {weapon.knockbackTime}s\n" +
                    $"\n" +
                    $"When equipped :\n" +
                    $"- Max HP + {weapon.E_maxHealth}\n" +
                    $"- Strength + {weapon.E_strength}\n" +
                    $"- Defence + {weapon.E_defence}\n" +
                    $"- Walk SPD + {weapon.E_walkSpeed}\n" +
                    $"- Crit Rate + {weapon.E_critRate}%\n" +
                    $"- Crit DMG + {weapon.E_critDamage}%";
            }
            catch { }
            try
            {
                var edibleItem = (EdibleItemSO)item;
                b = "";
            }
            catch { }
            try
            {
                var equipment = (EquippableItemSO)item;
                b = "";
            }
            catch { }

            description.text = item.Description + $"\n\n{b}";
        }

        private void SetImage(ItemSO item)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = item.Image;
        }

        private void SetTitle(ItemSO item)
        {
            title.gameObject.SetActive(true);
            title.text = item.Name;
            switch (item.Rarity)
            {
                case Rarity.Common:
                    title.outlineColor = new Color(255, 255, 255, 255);
                    break;
                case Rarity.Uncommon:
                    title.outlineColor = new Color(255, 255, 0, 255);
                    break;
                case Rarity.Rare:
                    title.outlineColor = new Color(0, 255, 255, 255);
                    break;
                case Rarity.Exotic:
                    title.outlineColor = new Color(0, 0, 255, 255);
                    break;
                case Rarity.Mythic:
                    title.outlineColor = new Color(255, 0, 255, 255);
                    break;
                case Rarity.Legendary:
                    title.outlineColor = new Color(255, 0, 0, 255);
                    break;
            }
        }
    }
}