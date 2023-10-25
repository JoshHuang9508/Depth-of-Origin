using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmorController : MonoBehaviour
{
    public Image armor, jewelry, book, weapon1, weapon2, weapon3;
    public GameObject target, statsdisplay;
    public TMP_Text health, str, movespeed, def, critrate, critdamage;
    PlayerBehaviour player;
    private void Awake()
    {
        initial();
    }

    public void initial()
    {
        armor.gameObject.SetActive(false);
        jewelry.gameObject.SetActive(false);
        book.gameObject.SetActive(false);
        weapon1.gameObject.SetActive(false);
        weapon2.gameObject.SetActive(false);
        weapon3.gameObject.SetActive(false);
        
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
        if (player.weapon1 != null)
        {
            weapon1.gameObject.SetActive(true);
            weapon1.sprite = player.weapon1.Image;
        }
        if (player.weapon2 != null)
        {
            weapon2.gameObject.SetActive(true);
            weapon2.sprite = player.weapon2.Image;
        }
        if (player.weapon3 != null)
        {
            weapon3.gameObject.SetActive(true);
            weapon3.sprite = player.weapon3.Image;
        }
        if (player.armor != null)
        {
            armor.gameObject.SetActive(true);
            armor.sprite = player.armor.Image;
        }
        if (player.jewelry != null)
        {
            jewelry.gameObject.SetActive(true);
            jewelry.sprite = player.jewelry.Image;
        }
        if (player.book != null)
        {
            book.gameObject.SetActive(true);
            book.sprite = player.book.Image;
        }
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
