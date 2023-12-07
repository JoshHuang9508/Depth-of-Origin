using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameController : MonoBehaviour
{
    public int sceneIndex;
    public ProgressBar progressBar;
    public GameObject temp;
    void Start()
    {
        progressBar.currentPercent = 0f;
        sceneIndex = PlayerPrefs.GetInt("loadscene");
        temp.GetComponent<SceneLoaderController>().SceneNum = sceneIndex;
        /*if(progressBar.currentPercent == 100f)
        {
            SceneManager.LoadScene(scenename);
        }*/
        StartCoroutine(loading());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator loading()
    {
        yield return null;
        temp.GetComponent<SceneLoaderController>().Load();
        //AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        /*while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.currentPercent = progress * 100f;
            yield return null;
        }*/
    }
}
