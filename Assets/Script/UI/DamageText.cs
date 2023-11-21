using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [Header("Setting")]
    public float timeToLive = 0.5f;
    public float floatSpeed = 500;
    public Vector3 floatingDir = new Vector3(0, 1, 0);

    [Header("Status")]
    public float timeElapsed = 0.0f;

    TextMeshProUGUI textmesh;
    RectTransform rtransform;
    Color starting_Color;

    void Start()
    {
        textmesh = GetComponent<TextMeshProUGUI>();
        starting_Color = textmesh.color;
        rtransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        rtransform.position += floatSpeed * Time.deltaTime * floatingDir;
        textmesh.color = new Color(starting_Color.r, starting_Color.g, starting_Color.b, 1 - (timeElapsed / timeToLive));
        if (timeElapsed >= timeToLive)
        {
            Destroy(gameObject);
        }
    }

    public void SetContent(float value, string type)
    {
        Start();
        switch (type)
        {
            case "Damage":
                textmesh.text = Mathf.RoundToInt(value).ToString();
                textmesh.color = new Color(255, 255, 255, 255);
                textmesh.outlineWidth = 0f;
                break;
            case "DamageCrit":
                textmesh.text = Mathf.RoundToInt(value).ToString();
                textmesh.color = new Color(255, 255, 0, 255);
                textmesh.outlineColor = new Color(255, 0, 0, 255);
                textmesh.outlineWidth = 0.4f;
                break;
            case "Heal":
                textmesh.text = Mathf.RoundToInt(value).ToString();
                textmesh.color = new Color(0, 150, 0, 255);
                textmesh.outlineWidth = 0f;
                break;
            case "PlayerHit":
                textmesh.text = Mathf.RoundToInt(value).ToString();
                textmesh.color = new Color(150, 0, 0, 255);
                textmesh.outlineWidth = 0f;
                break;
        }
    }
}
