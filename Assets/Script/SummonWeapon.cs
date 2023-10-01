using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonWeapon : MonoBehaviour
{
    bool isflip;
    float startAngle;

    Vector2 mousePos;
    Vector2 currentPos;
    Vector2 Diraction;

    public GameObject weapons;
    public Interactable Interactable;
    WeaponMovement weaponMovement;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentPos = transform.position;
        Diraction = (mousePos - currentPos).normalized;
        startAngle = Mathf.Atan2(Diraction.y, Diraction.x) * Mathf.Rad2Deg;

        if (Diraction.x < 0)
        {
            isflip = true;
        }
        else if (Diraction.x >= 0)
        {
            isflip = false;
        }
    }

    public void summon_weapon()
    {
        Interactable.enabled = false;
        var sword = Instantiate(weapons, new Vector3(transform.position.x, transform.position.y, transform.position.z), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
        sword.transform.parent = this.transform;
        weaponMovement = GetComponentInChildren<WeaponMovement>();
        weaponMovement.WeaponSwing(isflip, startAngle);
    }

    public void get_return()
    {
        Interactable.enabled = true;
    }
}
