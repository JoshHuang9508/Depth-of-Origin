using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class WeaponMovement : MonoBehaviour
{
    public float attackSpeed;
    public float weaponDamage = 1f;
    public float knockbackForce;
    public float knockbackTime;
    public float knockbackSpeed;
    float swingTime = 20; //總揮動動畫偵數

    bool isflip;
    float startAngle;

    public SpriteRenderer spriteRenderer;
    SummonWeapon summonWeapon;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damage_Interface damageableObject = collision.GetComponentInParent<Damage_Interface>();

        if (damageableObject != null)
        {
            if (collision.CompareTag("Enemy"))
            {
                Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
                Vector2 direction = (Vector2)(collision.gameObject.transform.position - parentPos).normalized;

                damageableObject.OnHit(weaponDamage, direction * knockbackForce * 1000, knockbackTime);
            }
        }
        else
        {
            //Debug.LogWarning("collision dont have implement IDamageable");
        }
    }

    float rotz;

    public void WeaponSwing(bool _isflip, float _startAngle)
    {
        startAngle = _startAngle;
        spriteRenderer.flipX = _isflip;
        isflip = _isflip;
        rotz = 60f;
        StopCoroutine(delay());
        StartCoroutine(delay());
    }

    private IEnumerator delay()
    {
        Debug.Log(startAngle - 90);
        for (float i = 1; i <= swingTime; i++)
        {
            if (!isflip)
            {
                Quaternion angle = Quaternion.Euler(0, 0, startAngle + rotz - 90);
                rotz -= (120 / swingTime);
                this.transform.rotation = angle;
                yield return new WaitForSeconds(1 / attackSpeed);
            }
            else if (isflip)
            {
                Quaternion angle = Quaternion.Euler(0, 0, startAngle - rotz - 90);
                rotz -= (120 / swingTime);
                this.transform.rotation = angle;
                yield return new WaitForSeconds(1 / attackSpeed);
            }
        }
        summonWeapon = GetComponentInParent<SummonWeapon>();
        summonWeapon.get_return();
        Destroy(gameObject);
    }
}
