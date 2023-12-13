using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Projectile_SlimeBalls_Behavior : MonoBehaviour
{
    [SerializeField] private EdibleItemSO poisonEffect;
    public float alivetime = 10f;
    float currentTime=0f;
    bool damageEnabler = true;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= alivetime)
        {
            Destroy(gameObject);
        }

        if(DetectPlayer() && damageEnabler)
        {
            Damageable damageableObject = GameObject.FindWithTag("Player").GetComponent<Damageable>();
            GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>().SetEffection(poisonEffect, poisonEffect.effectTime);
            damageableObject.OnHit(10, false, Vector2.zero, 0);

            StartCoroutine(SetTimer(callback => { damageEnabler = callback; }, 2));
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

    private IEnumerator SetTimer(System.Action<bool> callback, float time)
    {
        callback(false);
        yield return new WaitForSeconds(time);
        callback(true);
    }
}
