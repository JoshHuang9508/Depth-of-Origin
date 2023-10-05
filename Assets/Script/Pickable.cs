using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    bool pickEnabler = false;
    Rigidbody2D currentRb;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(pickup_delay());
    }

    // Update is called once per frame
    void Update()
    {
        currentRb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && pickEnabler)
        {
            int movement_x = (this.transform.position.x - collision.transform.position.x <= 0) ? 1 : -1;
            int movement_y = (this.transform.position.y - collision.transform.position.y <= 0) ? 1 : -1;
            Vector3 movement = new Vector3(movement_x * 8, movement_y * 8, 0.0f);
            currentRb.velocity = new Vector2(movement.x, movement.y);
            float distance = Vector2.Distance(this.transform.position, collision.transform.position);
            if (distance <= 0.2) Destroy(gameObject);
        }
    }

    private IEnumerator pickup_delay()
    {
        yield return new WaitForSeconds(1.5f);
        pickEnabler = true;
    }
}
