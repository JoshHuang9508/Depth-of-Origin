using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmorController : MonoBehaviour
{
    public Image head,Armor, weapons,book;
    public GameObject target,statsdisplay;
    public TMP_Text health, str, movespeed, def,critrate,critdamage;
    PlayerBehaviour player;
    private void Awake()
    {
        initial();
    }

    public void initial()
    {
        head.gameObject.SetActive(false);
        Armor.gameObject.SetActive(false);
        weapons.gameObject.SetActive(false);
        book.gameObject.SetActive(false);
        health.text = "0";
        str.text = "0";
        def.text = "0";
        movespeed.text = "0";
        critrate.text = "0";
        critdamage.text = "0";
    }
    private void Start()
    {
        player = target.GetComponent<PlayerBehaviour>();
    }

    private void Update()
    {
        SetImage();
        GetPlayerStats();
    }

    public void SetImage()
    {

        if(player.weaponSO != null)
        {
            weapons.gameObject.SetActive (true);
            weapons.sprite = player.weaponSO.Image;
        }
    }

    public void GetPlayerStats()
    {
        if(player.weaponSO != null)
        {
            health.text = player.weaponSO.health.ToString();
            str.text = player.weaponSO.strength.ToString();
            movespeed.text = player.walkSpeed.ToString();
            def.text = "0";
            critrate.text = player.weaponSO.critchance.ToString();
            critdamage.text = player.weaponSO.critdamage.ToString();
        }
    }
}
