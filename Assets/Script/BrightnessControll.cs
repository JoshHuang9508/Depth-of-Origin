using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BrightnessControll : MonoBehaviour
{
    Collider2D Collider2D;
    Light2D torchLight;
    public float ChangeBrightnessTime;
    // Start is called before the first frame update
    void Start()
    {
        Collider2D = GetComponent<Collider2D>();
        torchLight = GetComponent<Light2D>();
        torchLight.intensity = 0;
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
        for(torchLight.intensity = torchLight.intensity; torchLight.intensity < 1.2; torchLight.intensity += 0.05f)
        {

            yield return new WaitForSeconds(ChangeBrightnessTime);
        
        }
        
        
    }
}
