using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRatatingAxeController : MonoBehaviour
{
    [SerializeField] private GameObject splitProjectile;
    [SerializeField] private EnemySO enemyData;

    private void Start()
    {
        enemyData = GetComponent<ProjectileMovement_Enemy>().enemyData;
    }

    private void OnDestroy()
    {
        for (int i = -180; i <= 180; i += 18)
        {
            SummonArrow(transform.position, i);
        }
    }

    private void SummonArrow(Vector3 position, float angle)
    {
        var ArrowSummoned = Instantiate(
            splitProjectile,
            position,
            Quaternion.Euler(0, 0, angle - 90),
            GameObject.FindWithTag("Item").transform);

        ArrowSummoned.GetComponent<ProjectileMovement_Enemy>().startAngle = Quaternion.Euler(0, 0, angle);
        ArrowSummoned.GetComponent<ProjectileMovement_Enemy>().enemyData = enemyData;
    }
}
