using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightController : MonoBehaviour
{
    [Header("Setting")]
    public Gradient gradient;
    public float dayTime, nightTime;
    public float gap;

    Light2D globalLight;

    void Start()
    {
        globalLight = GetComponent<Light2D>();

        globalLight.color = gradient.Evaluate(1);
        StartCoroutine(DayNightCycle());
    }

    private IEnumerator DayNightCycle()
    {
        yield return new WaitForSeconds(dayTime);
        for(float i = 1f; i > 0; i -= 0.01f)
        {
            globalLight.color = gradient.Evaluate(i);
            yield return new WaitForSeconds(gap);
        }
        yield return new WaitForSeconds(nightTime);
        for (float i = 0f; i < 1; i += 0.01f)
        {
            globalLight.color = gradient.Evaluate(i);
            yield return new WaitForSeconds(gap);
        }
        StartCoroutine(DayNightCycle());
    }
}
