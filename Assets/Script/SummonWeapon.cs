using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonWeapon : MonoBehaviour
{
    public Interactable Interactable;
    public WeaponSO weapon;
    public PlayerBehaviour player;

    bool isflip;
    float startAngle;
    bool summonEnabler = true;

    Vector2 mousePos;
    Vector2 currentPos;
    Vector2 Diraction;

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

    public void Summon()
    {
        if (summonEnabler && (weapon = player.weapon[player.currentWeapon]))
        {
            for (var i = this.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(this.transform.GetChild(i).gameObject);
            }

            summonEnabler = false;

            GameObject weaponSummoned = Instantiate(weapon.weaponObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), this.transform);
            weaponSummoned.GetComponent<WeaponMovement>().weapon = weapon;
            weaponSummoned.GetComponent<WeaponMovement>().WeaponSwing(isflip);

            //**need consider about far distant weapon**
            transform.rotation = Quaternion.Euler(0, 0, startAngle - 90);
        }
    }

    public void CooldownOver()
    {
        summonEnabler = true;
    }
}
