using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BrightnessControll : MonoBehaviour
{
    [Header("Settings")]
    public float ChangeGap;

    Light2D torchLight;

    void Start()
    {
        torchLight = GetComponent<Light2D>();
        torchLight.intensity = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(BrihgtnessIncreseCountinue());
    }

    IEnumerator BrihgtnessIncreseCountinue()
    {
        for(torchLight.intensity = torchLight.intensity; torchLight.intensity < 1.2; torchLight.intensity += 0.05f)
        {
            yield return new WaitForSeconds(ChangeGap);
        }
    }
}
