using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float smoothing;
    [SerializeField] private Vector3 offset = Vector3.zero;

    [Header("Object Reference")]
    [SerializeField] private Transform target;
    [SerializeField] private Collider2D mapBounds;

    [Header("Stats")]
    public bool isFollowing_x = true;
    public bool isFollowing_y = true;
    public Vector2 newPosition;

    private void Start()
    {
        newPosition = target.position;
    }

    private void FixedUpdate()
    {
        if(target == null)
        {
            return;
        }

        try
        {
            mapBounds = GameObject.FindWithTag("WorldEdge").GetComponent<Collider2D>();
        }
        catch
        {
            mapBounds = null;
        }

        newPosition = Vector2.Lerp(transform.position, target.transform.position + offset, smoothing);

        if(mapBounds != null)
        {
            Vector2 targetPos_x = new Vector2(newPosition.x, 0);
            Vector2 targetPos_y = new Vector2(0, newPosition.y);

            if (!mapBounds.bounds.Contains(targetPos_x)) isFollowing_x = false;
            else isFollowing_x = true;

            if (!mapBounds.bounds.Contains(targetPos_y)) isFollowing_y = false;
            else isFollowing_y = true;

            Vector2 currentPos = transform.position;

            transform.position = new Vector3(isFollowing_x ? newPosition.x : currentPos.x, isFollowing_y ? newPosition.y : currentPos.y, -10);
        }
        else
        {
            transform.position = newPosition;
        }
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            transform.localPosition = originalPos + new Vector3(Random.Range(-1f, 1f) * magnitude, Random.Range(-1f, 1f) * magnitude, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;

        yield return null;
    }
}