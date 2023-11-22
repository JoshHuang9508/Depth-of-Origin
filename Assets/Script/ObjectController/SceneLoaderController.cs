using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] LoadType loadType = LoadType.Scene;
    [SerializeField] private int SceneNum = 0;
    [SerializeField] private GameObject transformPos;

    [Header("Stats")]
    public static bool inAction = false;

    [Header("Object Reference")]
    public Animator transition;

    enum LoadType
    {
        Scene, Chunk
    }


    private void Start()
    {
        transition = GameObject.FindWithTag("Transition").GetComponent<Animator>();
    }

    public void Load()
    {
        if (inAction) return;
        StartCoroutine(Load_delay());
    }

    private IEnumerator Load_delay()
    {
        transition.SetTrigger("Start");
        inAction = true;

        yield return new WaitForSeconds(1.5f);

        switch (loadType)
        {
            case LoadType.Scene:
                //AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneNum, LoadSceneMode.Additive);
                //asyncLoad.allowSceneActivation = false;

                //while (asyncLoad.progress < 0.9f)
                //{
                //    Debug.Log("Loading scene " + " [][] Progress: " + asyncLoad.progress);
                //    yield return null;
                //}

                //asyncLoad.allowSceneActivation = true;

                //while (!asyncLoad.isDone)
                //{
                //    yield return null;
                //}

                //Scene sceneToLoad = SceneManager.GetSceneByBuildIndex(SceneNum);

                //if (sceneToLoad.IsValid())
                //{
                //    SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                //    SceneManager.SetActiveScene(sceneToLoad);
                //    yield return new WaitForSeconds(0.2f);
                //}
                SceneManager.LoadScene(SceneNum, LoadSceneMode.Single);
                GameObject.FindWithTag("Player").transform.position = GameObject.FindWithTag("Respawn").transform.position;
                GameObject.FindWithTag("CameraHold").transform.position = GameObject.FindWithTag("Respawn").transform.position;
                transition.SetTrigger("End");

                inAction = false;

                break;

            case LoadType.Chunk:
                GameObject.FindWithTag("Player").transform.position = transformPos.gameObject.transform.position;
                GameObject.FindWithTag("CameraHold").transform.position = transformPos.gameObject.transform.position;
                transition.SetTrigger("End");

                yield return new WaitForSeconds(0.5f);
                inAction = false;

                break;
        }
        
    }
}
