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
    Animator animator;
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

    public void WeaponSwing(bool isflip)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2 = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        summonWeapon = GetComponentInParent<SummonWeapon>();

        StartCoroutine(swing_animation(isflip));
    }

    private IEnumerator swing_animation(bool isflip)
    {
        //Debug.Log(startAngle - 90);

        animator.SetBool("isflip", isflip);
        spriteRenderer.flipX = isflip;
        animator.speed = attackSpeed;
        yield return new WaitForSeconds(0.2f / attackSpeed);
        yield return new WaitForSeconds(attackCooldown);

        //Debug.Log("cooldown over");

        summonWeapon.CooldownOver();
        Destroy(gameObject);
    }
}
