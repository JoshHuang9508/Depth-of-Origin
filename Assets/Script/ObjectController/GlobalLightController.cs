using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private Gradient gradient;
    [SerializeField] private float dayTime, nightTime;
    [SerializeField] private float transformTimeGap;

    [Header("Dynamic Data")]
    [SerializeField] private float timeElapse;

    [Header("Object Reference")]
    [SerializeField] private Light2D globalLight;

    private void Start()
    {
        globalLight = GetComponent<Light2D>();
        globalLight.color = gradient.Evaluate(1);
    }

    private void Update()
    {
        timeElapse += Time.deltaTime;

        int dayNightControl = 1;

        if(timeElapse >= dayTime && dayNightControl == 1)
        {
            StartCoroutine(DayNightTransform("Night"));
            timeElapse = 0;
        }
        else if(timeElapse >= nightTime && dayNightControl == 0)
        {
            StartCoroutine(DayNightTransform("Day"));
            timeElapse = 0;
        }
    }

    private IEnumerator DayNightTransform(string type)
    {
        switch (type)
        {
            case "Night":
                for (float i = 1f; i > 0; i -= 0.01f)
                {
                    globalLight.color = gradient.Evaluate(i);
                    yield return new WaitForSeconds(transformTimeGap);
                }
                break;
            case "Day":
                for (float i = 0f; i < 1; i += 0.01f)
                {
                    globalLight.color = gradient.Evaluate(i);
                    yield return new WaitForSeconds(transformTimeGap);
                }
                break;
        }
    }
}
