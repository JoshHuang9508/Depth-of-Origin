using System.Collections;
using System.Collections.Generic;
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
                //StartCoroutine(LoadAsyncScene());
                SceneManager.LoadScene(SceneNum, LoadSceneMode.Single);
                transition.SetTrigger("End");
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                break;
            case 2:
                GameObject.FindWithTag("Player").transform.position = transferPos;
                yield return new WaitForSeconds(1f);
                transition.SetTrigger("End"); 
                break;
        }
    }

    IEnumerator LoadAsyncScene()
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneNum, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        Debug.Log(asyncLoad.ToString());

        // Wait until the asynchronous scene fully loads
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
            Debug.Log("Scene is Valid");
            SceneManager.SetActiveScene(sceneToLoad);
            transition.SetTrigger("End");
        }
    }
}
