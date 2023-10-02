using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightContorller : MonoBehaviour
{
    float startAngle;

    Vector2 mousePos;
    Vector2 currentPos;
    Vector2 Diraction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentPos = transform.position;
        Diraction = (mousePos - currentPos).normalized;
        startAngle = Mathf.Atan2(Diraction.y, Diraction.x) * Mathf.Rad2Deg;
        Quaternion angle = Quaternion.Euler(0, 0, startAngle - 90);
        this.transform.rotation = angle;
    }
}
