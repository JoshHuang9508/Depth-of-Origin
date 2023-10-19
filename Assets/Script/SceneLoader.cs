using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    int loadType = 1;
    public int SceneNum;
    public Vector3 transferPos;
    public Animator transition;

    public void LoadScene()
    {
        loadType = 1;
        StartCoroutine(Load_delay());
    }

    public void LoadChunk()
    {
        loadType = 2;
        StartCoroutine(Load_delay());
    }

    private IEnumerator Load_delay()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1.5f);

        switch (loadType)
        {
            case 1:
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneNum, LoadSceneMode.Additive);
                asyncLoad.allowSceneActivation = false;

                while (asyncLoad.progress < 0.9f)
                {
                    Debug.Log("Loading scene " + " [][] Progress: " + asyncLoad.progress);
                    yield return null;
                }

                asyncLoad.allowSceneActivation = true;

                while (!asyncLoad.isDone)
                {
                    yield return null;
                }

                Scene sceneToLoad = SceneManager.GetSceneByBuildIndex(SceneNum);

                if (sceneToLoad.IsValid())
                {
                    SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                    SceneManager.SetActiveScene(sceneToLoad);
                    transition.SetTrigger("End");
                }
                break;

            case 2:
                GameObject.FindWithTag("Player").transform.position = transferPos;
                GameObject.FindWithTag("CameraHold").transform.position = transferPos;
                transition.SetTrigger("End");
                break;
        }
    }
}
