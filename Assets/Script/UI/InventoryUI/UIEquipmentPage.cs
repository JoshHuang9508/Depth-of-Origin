using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentPage : MonoBehaviour
{
    [Header("State")]
    public UIItemSlot armor;
    public UIItemSlot jewelry;
    public UIItemSlot book;
    public UIItemSlot meleeWeapon;
    public UIItemSlot rangedWeapon;
    public UIItemSlot potions;

    [Header("Connect Object")]
    public GameObject target, statsdisplay;
    public TMP_Text health, str, movespeed, def, critrate, critdamage;

    PlayerBehaviour player;


    private void Start()
    {
        target = GameObject.FindWithTag("Player");
        player = target.GetComponent<PlayerBehaviour>();
    }


    private void Update()
    {
        SetImage();
        GetPlayerStats();
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

    public void GetPlayerStats()
    {
        health.text = player.maxHealth.ToString();
        str.text = player.strength.ToString();
        movespeed.text = player.walkSpeed.ToString();
        def.text = player.defence.ToString();
        critrate.text = player.critRate.ToString();
        critdamage.text = player.critDamage.ToString();
    }
}
