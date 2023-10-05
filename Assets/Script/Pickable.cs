using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    bool pickEnabler = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(pickup_delay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && pickEnabler)
        {
            //Destroy(gameObject);
            Vector3 Dir = collision.transform.position - transform.position;
            float distance = Vector3.Distance(transform.position, collision.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, collision.transform.position, distance);
        }
    }

    private IEnumerator pickup_delay()
    {
        yield return new WaitForSeconds(1.5f);
        pickEnabler = true;
    }
}
