using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmorController : MonoBehaviour
{
    public Image jewelry, armor, weapons, book;
    public GameObject target, statsdisplay;
    public TMP_Text health, str, movespeed, def, critrate, critdamage;
    PlayerBehaviour player;
    private void Awake()
    {
        initial();
    }

    public void initial()
    {
        jewelry.gameObject.SetActive(false);
        armor.gameObject.SetActive(false);
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

        if(player.weapon1 != null)
        {
            weapons.gameObject.SetActive (true);
            weapons.sprite = player.weapon1.Image;
        }
        if(player.armor != null)
        {
            armor.gameObject.SetActive(true);
            armor.sprite = player.armor.Image;
        }
        if (player.jewelry != null)
        {
            jewelry.gameObject.SetActive(true);
            jewelry.sprite = player.jewelry.Image;
        }
    }

    public void GetPlayerStats()
    {
        if(player.weapon1 != null)
        {
            health.text = player.weapon1.health.ToString();
            str.text = player.weapon1.strength.ToString();
            movespeed.text = player.walkSpeed.ToString();
            def.text = "0";
            critrate.text = player.weapon1.critchance.ToString();
            critdamage.text = player.weapon1.critdamage.ToString();
        }
    }
}
