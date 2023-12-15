using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Controller : MonoBehaviour
{
    public bool counterEnabler =false , spawnBossEnabler = true , bossAlive = false;
    [Header("Object Reference")]
    [SerializeField] public GameObject spawner,Entity,rewardChest;
    int entityAmount;
    List<GameObject> entitylist = new();
    public Vector3 summonPos;
    int slimeCounter = 0, notsmallestSlimeCounter = 0;

    [Header("Chest Control")]
    [SerializeField] public List<Lootings> lootings = new();
    public List<Lootings> chestslootings = new();
    

    private void Start()
    {
        spawnBossEnabler=true;
        foreach (var item in lootings)
        {
            chestslootings.Add(item);
        }
    }

    private void Update()
    {
        slimeCounter = 0;
        notsmallestSlimeCounter = 0;
        entitylist = new();
        int entitycount = 0;
        entitycount = GameObject.Find("Entity").transform.childCount;
        for (int i = 0; i < entitycount; i++)
        {
            entitylist.Add(GameObject.Find("Entity").transform.GetChild(i).gameObject);
        }
        foreach (var item in entitylist)
        {
            SmallestSlime_Controller smallestSlimescript = item.GetComponent<SmallestSlime_Controller>();

            if (smallestSlimescript != null)
            {
                slimeCounter++;
            }
            D2_Boss_splitSlime_Controller d2_split_script = item.GetComponent<D2_Boss_splitSlime_Controller>();
            if (d2_split_script != null)
            {
                notsmallestSlimeCounter++;
            }
            Boss2Behavior boss2script = item.GetComponent<Boss2Behavior>();
            if (boss2script != null)
            {
                notsmallestSlimeCounter++;
                bossAlive = true;
            }
        }

        if(slimeCounter == 0 && notsmallestSlimeCounter == 0 && bossAlive)
        {
            GameObject chest = Instantiate(rewardChest, transform.position, Quaternion.identity, GameObject.Find("Map").transform.GetChild(3));
            chest.GetComponent<ChestController>().lootings = chestslootings;
            bossAlive = false;
        }
        Debug.Log(slimeCounter);
        Debug.Log("notslimecounter" + notsmallestSlimeCounter);
    }

    public void SummonBoss()
    {
        if (spawnBossEnabler)
        {
            spawner.GetComponent<SpawnerController>().SpawnMobs();
            spawnBossEnabler = false;
        }
    }
}
