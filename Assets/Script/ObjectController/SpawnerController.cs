using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [Header("Setting")]
    public bool autoSpawn = true;
    public float minSpawnDistance = 15;
    public float spawnRange;
    public int mobsStayedLimit = 4;
    public int spawnTimesLimit = -1;
    public float spawnGapMin = 3;
    public float spawnGapMax = 10;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private List<EnemySO> spawnList;

    [Header("Dynamic Data")]
    [SerializeField] private int spawnTimes;
    [SerializeField] private int stayedMobs;

    [Header("Stats")]
    public bool spawnEnabler = true;

    private void Start()
    {
        if (GetComponent<CircleCollider2D>())
        {
            spawnRange = GetComponent<CircleCollider2D>().radius;
        }
    }

    private void Update()
    {
        if(autoSpawn)
        {
            SpawnMobs();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        /*if (collision.CompareTag("Player"))
        {
            spawnEnabler = false;
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) stayedMobs++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) stayedMobs--;
        /*if (collision.CompareTag("Player"))
        {
            spawnEnabler = true;
        }*/
    }

    public void SpawnMobs()
    {
        //detect spawn restrict
        if(spawnEnabler && (stayedMobs < mobsStayedLimit || mobsStayedLimit == -1) && (spawnTimesLimit > spawnTimes || spawnTimesLimit == -1) &&
            GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>().behaviourEnabler)
        {
            //spawn delay
            StartCoroutine(delay(enabler =>
            spawnEnabler = enabler,
            Random.Range(spawnGapMin, spawnGapMax)));

            Vector3 spawnPos = Vector3.zero;

            //detect spawn position
            while ((DetectBlankAreas(spawnPos, new Vector2(1f, 1f), 0.1f) || Vector2.Distance(GameObject.FindWithTag("Player").transform.position, spawnPos) < minSpawnDistance || spawnPos == Vector3.zero))
            {
                //random spawn position
                float spawnX = Random.Range(-1 * spawnRange, spawnRange);
                float spawnY = Random.Range(-1 * Mathf.Sqrt((spawnRange * spawnRange) - (spawnX * spawnX)), Mathf.Sqrt((spawnRange * spawnRange) - (spawnX * spawnX)));
                spawnPos = new Vector3(
                        transform.position.x + spawnX,
                        transform.position.y + spawnY,
                        transform.position.z);
            }

            //spawn mobs
            int randomSpawnIndex = Random.Range(0, spawnList.Count);
            var spawnMob = Instantiate(
                spawnList[randomSpawnIndex].EnemyObject,
                spawnPos,
                Quaternion.identity,
                GameObject.FindWithTag("Entity").transform);
            spawnMob.GetComponent<EnemyBehavior>().enemy = spawnList[randomSpawnIndex];
            spawnTimes++;
        }
        else
        {
            return;
        }
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
                    if (collider.CompareTag("Water") || collider.CompareTag("HitBox") || collider.CompareTag("BreakableObject") || collider.CompareTag("Wall")) return true;
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
