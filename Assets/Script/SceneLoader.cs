using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    int loadType = 1;
    public float transitionTime;
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

        yield return new WaitForSeconds(transitionTime);

        switch (loadType)
        {
            case 1:
                SceneManager.LoadScene(SceneNum);
                break;

            case 2:
                GameObject.FindWithTag("Player").transform.position = transferPos;
                yield return new WaitForSeconds(1f);
                transition.SetTrigger("End");
                break;
        }
        
    }
}
