using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchLightController : MonoBehaviour
{
    Light2D torchLight;

    // Start is called before the first frame update
    void Start()
    {
        torchLight = GetComponent<Light2D>();
        StartCoroutine(torchLightShining());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator torchLightShining()
    {
        float targetIntensityValue = Random.Range(0.6f, 0.8f);
        float currentIntensityValue = torchLight.intensity;
        int mutiplier = currentIntensityValue < targetIntensityValue ? 1 : -1;
        while (torchLight.intensity * mutiplier <= targetIntensityValue * mutiplier)
        {
            torchLight.intensity += 0.05f * mutiplier;
            yield return new WaitForSeconds(Random.Range(0.1f,0.05f));
        }
        StartCoroutine(torchLightShining());
    }
}
