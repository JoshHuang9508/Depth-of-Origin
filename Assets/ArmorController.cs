using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorController : MonoBehaviour
{
    public Image head,Armor, weapons,book;
    public GameObject target;
    PlayerBehaviour player;
    private void Awake()
    {
        initialImage();
    }

    public void initialImage()
    {
        head.gameObject.SetActive(false);
        Armor.gameObject.SetActive(false);
        weapons.gameObject.SetActive(false);
        book.gameObject.SetActive(false);

    }
    private void Start()
    {
        player = target.GetComponent<PlayerBehaviour>();
    }

    private void Update()
    {
        SetImage();
    }

    public void SetImage()
    {

        if(player.weaponSO != null)
        {
            weapons.gameObject.SetActive (true);
            weapons.sprite = player.weaponSO.Image;
        }
    }


}
