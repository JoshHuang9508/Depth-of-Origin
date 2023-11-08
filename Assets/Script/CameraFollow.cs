using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Setting")]
    public Transform target;
    public Collider2D mapBounds;
    public float smoothing;
    public Vector3 offset = Vector3.zero;

    [Header("Status")]
    public bool isFollowing_x = true;
    public bool isFollowing_y = true;

    private void FixedUpdate()
    {
        try
        {
            mapBounds = GameObject.FindWithTag("WorldEdge").GetComponent<Collider2D>();
        }
        catch
        {
            mapBounds = null;
        }

        Vector3 newPosition = Vector3.Lerp(transform.position, target.transform.position + offset, smoothing);

        if(mapBounds != null)
        {
            Vector2 targetPos_x = new Vector2(newPosition.x, 0);
            Vector2 targetPos_y = new Vector2(0, newPosition.y);

            if (!mapBounds.bounds.Contains(targetPos_x)) isFollowing_x = false;
            else isFollowing_x = true;

            if (!mapBounds.bounds.Contains(targetPos_y)) isFollowing_y = false;
            else isFollowing_y = true;

            Vector3 currentPos = transform.position;

            transform.position = new Vector3(isFollowing_x ? newPosition.x : currentPos.x, isFollowing_y ? newPosition.y : currentPos.y, newPosition.z);
        }
        else
        {
            transform.position = newPosition;
        }
    }
}
