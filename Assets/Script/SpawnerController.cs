using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public int spawnLimit;
    public int maxSpawnTimes;

    public List<EnemySO> spawnList;

    public int stayMobs;
    private bool spawnEnabler = true;

    private void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && stayMobs <= spawnLimit && spawnEnabler && maxSpawnTimes != 0)
        {
            SpawnMobs();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) stayMobs++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) stayMobs--;
    }

    private void SpawnMobs()
    {
        maxSpawnTimes--;

        spawnEnabler = false;

        int randomSpawnIndex = Random.Range(0, spawnList.Count);

        //Debug.Log(randomSpawnIndex);

        var spawnMob = Instantiate(
            spawnList[randomSpawnIndex].EnemyObject,
            new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z),
            Quaternion.identity,
            GameObject.FindWithTag("Entity").transform
            );

        StartCoroutine(delay(enabler =>
        spawnEnabler = enabler,
        Random.Range(3f, 10f)));

        spawnMob.GetComponent<EnemyBehavior>().enemySO = spawnList[randomSpawnIndex];
    }

    private IEnumerator delay(System.Action<bool> callback, float delayTime)
    {
        callback(false);
        yield return new WaitForSeconds(delayTime);
        callback(true);
    }
}
