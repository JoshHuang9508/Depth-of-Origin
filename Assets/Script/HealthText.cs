using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    public float time_to_live = 0.5f;
    public float floatspeed = 500;
    public Vector3 floatingDir = new Vector3(0, 1, 0);
    public TextMeshProUGUI textmesh;
    public RectTransform rtransform;
    float time_Elapsed = 0.0f;
    Color starting_Color;

    void Start()
    {
        textmesh = GetComponent<TextMeshProUGUI>();
        starting_Color = textmesh.color;
        rtransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        time_Elapsed += Time.deltaTime;
        rtransform.position += floatingDir * floatspeed * Time.deltaTime;
        textmesh.color = new Color(starting_Color.r, starting_Color.g, starting_Color.b, 1 - (time_Elapsed / time_to_live));
        if (time_Elapsed >= time_to_live)
        {
            Destroy(gameObject);
        }
    }
}
