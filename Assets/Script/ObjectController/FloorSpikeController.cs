using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpikeController : MonoBehaviour
{
    [Header("Setting")]
    public float damage;
    public float inactiveTime;
    public float activeTime;

    [Header("Status")]
    public bool activeEnabler = true;
    public bool isActive = false;

    Animator animator;

    private void Update()
    {
        List<Collider2D> colliderResult = new();
        int colliderCount = Physics2D.OverlapCollider(GetComponent<Collider2D>(), new(), colliderResult);

        bool isPlayerInRange = false;
        for (int i = 0; i < colliderResult.Count; i++)
        {
            if (colliderResult[i] != null && colliderResult[i].CompareTag("Player")) isPlayerInRange = true;
        }

        if(activeEnabler && isActive && colliderCount > 0 && isPlayerInRange)
        {
            Damageable damageableObject = GameObject.FindWithTag("Player").GetComponent<Damageable>();

            damageableObject.OnHit(damage, false, new(), 0);

            StartCoroutine(delay((enabler) => {
                activeEnabler = enabler;
            }, 1));
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        Active();
    }

    private void Active()
    {
        StartCoroutine(delay(callback =>
        {
            isActive = callback;
            animator.SetBool("isActive", callback);

            if (callback == true) StartCoroutine(delay(callback =>
            {
                if (callback == true) Active();
            }, activeTime));
        }, inactiveTime));
    }

    private IEnumerator delay(System.Action<bool> callback, float delayTime)
    {
        callback(false);
        yield return new WaitForSeconds(delayTime);
        callback(true);
    }
}
