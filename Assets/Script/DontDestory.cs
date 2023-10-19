using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryOnLoad : MonoBehaviour
{
    private static GameObject instance;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
