using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Controller : MonoBehaviour
{
    public int deathCounter;
    [Header("Object Reference")]
    [SerializeField] private GameObject spawner;
    [SerializeField] private EnemySO boss;
    public Vector3 summonPos;
    private void Start()
    {
        var bossSummoned = Instantiate(
                boss.EnemyObject,
                summonPos,
                Quaternion.identity,
                GameObject.FindWithTag("Entity").transform);
        bossSummoned.GetComponent<EnemyBehavior>().enemy = boss;
    }

    // Update is called once per frame
    void Update()
    {
        if(deathCounter >= 7)
        {
            
        }
    }
}
