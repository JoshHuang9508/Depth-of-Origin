using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDestory : MonoBehaviour , Damage_Interface
{
    int HitCounter;
    

    public void OnHit(float damage, Vector2 knockbackForce, float knockbackTime)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        HitCounter = 3;
        //box.position = transform.position;
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
