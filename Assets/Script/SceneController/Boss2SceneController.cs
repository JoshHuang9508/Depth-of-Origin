using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2SceneController : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject chest;
    [SerializeField] private ChestController rewardChest;

    [Header("Dynamic Data")]
    [SerializeField] private bool bossAlive = false;
    [SerializeField] private int slimeCounter = 0;
    [SerializeField] private Vector3 chestSummonPos;
    [SerializeField] private List<GameObject> entitylist = new();
    

    private void Start()
    {
        bossAlive = false;
    }

    private void Update()
    {
        slimeCounter = 0;
        entitylist = new();

        int entitycount = GameObject.Find("Entity").transform.childCount;

        for (int i = 0; i < entitycount; i++)
        {
            entitylist.Add(GameObject.Find("Entity").transform.GetChild(i).gameObject);
        }

        foreach (var item in entitylist)
        {
            Boss2Behavior boss = item.GetComponent<Boss2Behavior>();
            if (boss != null)
            {
                slimeCounter++;
            }
        }

        if(bossAlive && slimeCounter == 0)
        {
            GameObject chestSummoned = Instantiate(
                chest, 
                transform.position, 
                Quaternion.identity, 
                GameObject.Find("Object_Grid").transform);
            chestSummoned.GetComponent<ChestController>().coins = rewardChest.coins;
            chestSummoned.GetComponent<ChestController>().lootings = rewardChest.lootings;

            bossAlive = false;
        }
    }

    public void SummonBoss()
    {
        if (!bossAlive)
        {
            spawner.GetComponent<SpawnerController>().SpawnMobs();
            bossAlive = true;
        }
    }
}
