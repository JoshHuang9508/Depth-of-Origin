using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy_Countdown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Destroy_Countdown()
    {
        yield return new WaitForSeconds(20);
        Destroy(gameObject);
    }
}
