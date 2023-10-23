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
    public TMP_Text health, str, movespeed, def;
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
        health.text = "";
        str.text = "";
        def.text = "";
        movespeed.text = "";
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
            health.text = "Health:" + player.weaponSO.health;
            str.text = "Str:" + player.weaponSO.strength;
            movespeed.text = "Move SPD:" + player.walkSpeed;
            def.text = "Def: 0";
        }
    }
}
