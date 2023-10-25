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
            title.text = "";
            description.text = "";
        }
        public void SetDescription(ItemSO item)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = item.Image;

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

            description.text = item.Description;
        }
    }
}