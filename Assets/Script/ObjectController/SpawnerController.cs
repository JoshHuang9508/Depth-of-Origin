using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [Header("Spawn Settings")]
    public int spawnLimit;
    public float spawnRange;
    public int maxSpawnTimes;
    public float spawnGapMin;
    public float spawnGapMax;
    public LayerMask targetLayer;
    public List<EnemySO> spawnList;

    [Header("Read Only Value")]
    public int spawnTimes;
    public int stayMobs;
    public bool spawnEnabler = true;
    public bool playerStayed = false;

    private void Update()
    {
        if(spawnEnabler == true && (stayMobs < spawnLimit || spawnLimit == -1) && (maxSpawnTimes > spawnTimes || maxSpawnTimes == -1))
        {
            SpawnMobs();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerStayed = true;
            spawnEnabler = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) stayMobs++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) stayMobs--;
        if (collision.CompareTag("Player"))
        {
            playerStayed = false;
            spawnEnabler = true;
        }
    }

    private void SpawnMobs()
    {
        //random spawn position
        float spawnX = Random.Range(-1 * spawnRange, spawnRange);
        float spawnY = Random.Range(-1 * Mathf.Sqrt((spawnRange * spawnRange) - (spawnX * spawnX)), Mathf.Sqrt((spawnRange * spawnRange) - (spawnX * spawnX)));
        Vector3 spawnPosition = new Vector3(
                transform.position.x + spawnX,
                transform.position.y + spawnY,
                transform.position.z);

        //detect spawn position
        if (DetectBlankAreas(spawnPosition, new Vector2(2f, 2f), 0.1f) || Vector2.Distance(GameObject.FindWithTag("Player").transform.position, spawnPosition) < 10) return;
        
        //spawn mobs
        int randomSpawnIndex = Random.Range(0, spawnList.Count);
        var spawnMob = Instantiate(
            spawnList[randomSpawnIndex].EnemyObject,
            spawnPosition,
            Quaternion.identity,
            GameObject.FindWithTag("Entity").transform
            );
        spawnMob.GetComponent<EnemyBehavior>().enemy = spawnList[randomSpawnIndex];
        spawnTimes++;

        //spawn delay
        StartCoroutine(delay(enabler =>
        spawnEnabler = playerStayed ? false : enabler,
        Random.Range(spawnGapMin, spawnGapMax)));
    }

    private bool DetectBlankAreas(Vector2 areaCenter, Vector2 areaSize, float cellSize)
    {
        for (float x = areaCenter.x - areaSize.x / 2; x < areaCenter.x + areaSize.x / 2; x += cellSize)
        {
            for (float y = areaCenter.y - areaSize.y / 2; y < areaCenter.y + areaSize.y / 2; y += cellSize)
            {
                Vector2 cellPosition = new Vector2(x, y);
                Collider2D[] colliders = Physics2D.OverlapBoxAll(cellPosition, new Vector2(cellSize, cellSize), 0f, targetLayer);

                if (colliders.Length == 0) return true;
                foreach (Collider2D collider in colliders)
                {
                    //Debug.Log(collider.tag);
                    if (collider.CompareTag("Water") || collider.CompareTag("HitBox") || collider.CompareTag("BreakableObject")) return true;
                }
            }
        }

        return false;
    }

    private IEnumerator delay(System.Action<bool> callback, float delayTime)
    {
        callback(false);
        yield return new WaitForSeconds(delayTime);
        callback(true);
    }
}
