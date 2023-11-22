using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpikeController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float damage;
    [SerializeField] private float inactiveTime;
    [SerializeField] private float activeTime;

    [Header("Object Reference")]
    [SerializeField] private Animator animator;

    [Header("Dynamic Data")]
    [SerializeField] private float timeElapse;

    [Header("Stats")]
    public bool activeEnabler = true;
    public bool isActive = false;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Activiting();

        if (activeEnabler && isActive && DetectPlayer())
        {
            Damageable damageableObject = GameObject.FindWithTag("Player").GetComponent<Damageable>();

            damageableObject.OnHit(damage, false, Vector2.zero, 0);
        }
    }

    private bool DetectPlayer()
    {
        List<Collider2D> colliderResult = new();
        Physics2D.OverlapCollider(GetComponent<Collider2D>(), new(), colliderResult);

        bool isPlayerInRange = false;
        for (int i = 0; i < colliderResult.Count; i++)
        {
            if (colliderResult[i] != null && colliderResult[i].CompareTag("Player")) isPlayerInRange = true;
        }

        return isPlayerInRange;
    }

    private void Activiting()
    {
        timeElapse += Time.deltaTime;
        animator.SetBool("isActive", isActive);

        if (timeElapse >= inactiveTime && !isActive)
        {
            isActive = true;
        }
        else if (timeElapse >= activeTime && isActive)
        {
            isActive = false;
        }
    }
}
