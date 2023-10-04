using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class WeaponMovement : MonoBehaviour
{
    public float attackSpeed;
    public float attackCooldown;
    public float weaponDamage = 1f;
    public float knockbackForce;
    public float knockbackTime;
    public float knockbackSpeed;
    float swingTime = 10; //總揮動動畫偵數

    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider2;
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

    float rotz = 60f;

    public void WeaponSwing(bool isflip, float startAngle)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2 = GetComponent<BoxCollider2D>();
        summonWeapon = GetComponentInParent<SummonWeapon>();
        if (isflip) spriteRenderer.flipX = true;
        StartCoroutine(swing_animation(isflip, startAngle));
    }

    private IEnumerator swing_animation(bool isflip, float startAngle)
    {
        //Debug.Log(startAngle - 90);

        for (float i = 1; i <= swingTime; i++)
        {
            if (!isflip)
            {
                Quaternion angle = Quaternion.Euler(0, 0, startAngle + rotz - 90);
                rotz -= (120 / swingTime);
                this.transform.rotation = angle;
                yield return new WaitForSeconds(attackSpeed / swingTime);
            }
            else if (isflip)
            {
                Quaternion angle = Quaternion.Euler(0, 0, startAngle - rotz - 90);
                rotz -= (120 / swingTime);
                this.transform.rotation = angle;
                yield return new WaitForSeconds(attackSpeed / swingTime);
            }
        }

        spriteRenderer.enabled = false;
        boxCollider2.enabled = false;

        yield return new WaitForSeconds(attackCooldown);

        //Debug.Log("cooldown over");

        summonWeapon.CooldownOver();
        Destroy(gameObject);
    }
}
