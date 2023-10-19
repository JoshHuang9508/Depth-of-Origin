using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public int SpawnLimit;

    public List<EnemySO> spawnList;

    public int stayMobs;
    private bool spawnEnabler = true;

    private void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && stayMobs <= SpawnLimit && spawnEnabler)
        {
            SpawnMobs();
            Debug.Log("Trying to spawn mobs");
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
        Random.Range(3, 10)));

        spawnMob.GetComponent<EnemyBehavior>().enemySO = spawnList[randomSpawnIndex];
    }

    private IEnumerator delay(System.Action<bool> callback, float delayTime)
    {
        callback(false);
        yield return new WaitForSeconds(delayTime);
        callback(true);
    }
}
