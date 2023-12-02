using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameController : MonoBehaviour
{
    public string scenename;
    public ProgressBar progressBar;
    void Start()
    {
        progressBar.currentPercent = 0f;
        scenename = PlayerPrefs.GetString("loadscene");
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
        AsyncOperation operation = SceneManager.LoadSceneAsync(scenename);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.currentPercent = progress * 100f;
            yield return null;
        }
    }
}
