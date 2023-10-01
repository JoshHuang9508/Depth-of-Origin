using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothing;

    public Vector3 offset;

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, target.transform.position+offset, smoothing);
            transform.position = newPosition;
        }
    }
    //a

}
