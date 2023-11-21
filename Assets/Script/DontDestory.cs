using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestoryOnLoad : MonoBehaviour
{
    private static GameObject instance;

    void Start()
    {
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);

        DontDestroyOnLoad(instance);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Scene activeScene = SceneManager.GetActiveScene();

        if(activeScene == scene)
        {
            GameObject.FindWithTag("Player").transform.position = GameObject.FindWithTag("Respawn").transform.position;
            GameObject.FindWithTag("CameraHold").transform.position = GameObject.FindWithTag("Respawn").transform.position;
        }
    }
}
