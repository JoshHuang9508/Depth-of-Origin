using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BrightnessControll : MonoBehaviour
{
    Collider2D Collider2D;
    Light2D light;
    public float ChangeBrightnessTime;
    // Start is called before the first frame update
    void Start()
    {
        Collider2D = GetComponent<Collider2D>();
        light = GetComponent<Light2D>();
        light.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(BrihgtnessIncreseCountinue());

    }


    IEnumerator BrihgtnessIncreseCountinue()
    {
        for(light.intensity=light.intensity; light.intensity < 1.2; light.intensity += 0.05f)
        {

            yield return new WaitForSeconds(ChangeBrightnessTime);
        
        }
        
        
    }
}
