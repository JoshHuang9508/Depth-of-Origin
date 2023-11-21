using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentDisplay : MonoBehaviour
{
    public Image meleeWeaponImage;
    public Image rangedWeaponImage;
    public Image potionImage;
    public Image meleeWeaponBorder;
    public Image rangedWeaponBorder;
    public TMP_Text potionAmountText;
    PlayerBehaviour player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    void Update()
    {
        meleeWeaponImage.sprite = player.meleeWeapon != null ? player.meleeWeapon.Image : null;
        meleeWeaponImage.color = player.meleeWeapon != null ? new Color(255, 255, 255, 255) : new Color(255, 255, 255, 0);
        rangedWeaponImage.sprite = player.rangedWeapon != null ? player.rangedWeapon.Image : null;
        rangedWeaponImage.color = player.rangedWeapon != null ? new Color(255, 255, 255, 255) : new Color(255, 255, 255, 0);
        potionImage.sprite = player.potions != null ? player.potions.Image : null;
        potionImage.color = player.potions != null ? new Color(255, 255, 255, 255) : new Color(255, 255, 255, 0);
        potionAmountText.text = player.currentPotionAmont != 0 ? player.currentPotionAmont.ToString() : "";

        switch (player.weaponControl)
        {
            case 0:
                meleeWeaponBorder.color = new Color(190, 0, 0, 0);
                rangedWeaponBorder.color = new Color(190, 0, 0, 0);
                break;
            case 1:
                meleeWeaponBorder.color = new Color(190, 0, 0, 255);
                rangedWeaponBorder.color = new Color(190, 0, 0, 0);
                break;
            case 2:
                meleeWeaponBorder.color = new Color(190, 0, 0, 0);
                rangedWeaponBorder.color = new Color(190, 0, 0, 255);
                break;
        }
    }


}
