using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Setting")]
    public LoadType loadType = LoadType.Scene;
    public int SceneNum = 0;
    public Vector3 transferPos;
    public static bool inAction = false;

    public enum LoadType
    {
        Scene, Chunk
    }

    [Header("Connect Object")]
    public Animator transition;
    

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
                break;

            case LoadType.Chunk:
                GameObject.FindWithTag("Player").transform.position = transferPos;
                GameObject.FindWithTag("CameraHold").transform.position = transferPos;
                break;
        }
        transition.SetTrigger("End");
        inAction = false;
    }
}
