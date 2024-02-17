using Inventory.Model;
using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentPage : MonoBehaviour
{

    [Header("Settings")]
    public ActionType ActionType;

    [Header("Dynamic Data")]
    public List<UIItemSlot> listOfItemSlots = new();

    [Header("Object Reference")]
    [SerializeField] private InventorySO equipmentData;
    [SerializeField] private TMP_Text healthText, strengthText, moveSpeedText, defenceText, critRateText, critDamageText;
    [SerializeField] private UIItemSlot armor;
    [SerializeField] private UIItemSlot jewelry;
    [SerializeField] private UIItemSlot book;
    [SerializeField] private UIItemSlot meleeWeapon;
    [SerializeField] private UIItemSlot rangedWeapon;
    [SerializeField] private UIItemSlot potions;
    [SerializeField] private UIInventory inventoryUI;
    [SerializeField] private PlayerBehaviour player;


    private void Update()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        SetImage();
        SetPlayerStats();
    }


    public void SetImage()
    {
        meleeWeapon.SetData(player.meleeWeapon != null ? player.meleeWeapon.Image : null, player.meleeWeapon != null ? 1 : 0);
        rangedWeapon.SetData(player.rangedWeapon != null ? player.rangedWeapon.Image : null, player.rangedWeapon != null ? 1 : 0);
        potions.SetData(player.potions != null ? player.potions.Image : null, player.potions != null ? player.currentPotionAmont : 0);
        armor.SetData(player.armor != null ? player.armor.Image : null, player.armor != null ? 1 : 0);
        jewelry.SetData(player.jewelry != null ? player.jewelry.Image : null, player.jewelry != null ? 1 : 0);
        book.SetData(player.book != null ? player.book.Image : null, player.book != null ? 1 : 0);
    }

    public void SetPlayerStats()
    {
        healthText.text = player.MaxHealth.ToString();
        strengthText.text = player.Strength.ToString();
        moveSpeedText.text = player.WalkSpeed.ToString();
        defenceText.text = player.Defence.ToString();
        critRateText.text = player.CritRate.ToString();
        critDamageText.text = player.CritDamage.ToString();
    }
}
