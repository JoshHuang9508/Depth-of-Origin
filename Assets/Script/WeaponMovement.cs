using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class WeaponMovement : MonoBehaviour
{
    public WeaponSO weaponSO;

    SpriteRenderer spriteRenderer;
    Animator animator;
    SummonWeapon summonWeapon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damage_Interface damageableObject = collision.GetComponentInParent<Damage_Interface>();

        if (damageableObject != null)
        {
            if (collision.CompareTag("HitBox") || collision.CompareTag("BreakableObject"))
            {
                Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
                Vector2 direction = (Vector2)(collision.gameObject.transform.position - parentPos).normalized;

                damageableObject.OnHit(weaponSO.weaponDamage, direction * weaponSO.knockbackForce, weaponSO.knockbackTime);

                //camera shake
                CameraShake cameraShake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
                StartCoroutine(cameraShake.Shake(0.1f, 0.2f));
            }
        }
        else
        {
            //Debug.LogWarning("collision dont have implement IDamageable");
        }
    }

    public void WeaponSwing(bool isflip)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        summonWeapon = GetComponentInParent<SummonWeapon>();

        StartCoroutine(swing_animation(isflip));
    }

    private IEnumerator swing_animation(bool isflip)
    {

        //Debug.Log(startAngle - 90);

        spriteRenderer.flipX = isflip;
        animator.speed = weaponSO.attackSpeed;
        animator.SetBool("isflip", isflip);
        animator.SetTrigger("swing");

        yield return new WaitForSeconds(weaponSO.attackCooldown);
        summonWeapon.CooldownOver();
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
