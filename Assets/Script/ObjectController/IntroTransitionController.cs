using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroTransitionController : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float fadeInDuration,fadeOutDuration;
    [SerializeField] private GameObject temp;
    float waitTime;

    private void Start()
    {
        text.text = "";
        text.gameObject.SetActive(true);
        waitTime = fadeInDuration + fadeOutDuration + 0.5f;
        StartCoroutine(firstdialog());
    }

    IEnumerator firstdialog()
    {
        StartCoroutine(SetTextContent("You walked into the forest"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(seconddialog());
    }
    IEnumerator seconddialog()
    {
        StartCoroutine(SetTextContent("Walking..."));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine (thirddialog());
    }
    IEnumerator thirddialog()
    {
        StartCoroutine(SetTextContent("After a long time"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(fourthdialog());
    }
    IEnumerator fourthdialog()
    {
        StartCoroutine(SetTextContent("After a long time"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(fifthdialog());
    }
    IEnumerator fifthdialog()
    {
        StartCoroutine(SetTextContent("You finally see some light"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(sixthdialog());
    }
    IEnumerator sixthdialog()
    {
        StartCoroutine(SetTextContent("you walked through the forest"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(seventhdialog());
    }
    IEnumerator seventhdialog()
    {
        StartCoroutine(SetTextContent("and see a village"));
        yield return new WaitForSeconds(waitTime);
        temp.GetComponent<SceneLoaderController>().Load();
    }

    IEnumerator SetTextContent(string description)
    {
        text.color = new Color(text.color.r,text.color.g,text.color.b,0f);
        Debug.Log(description);
        text.text = description;
        while (text.color.a <= 1f)
        {
            float alpha = Mathf.Clamp01(text.color.a + (Time.deltaTime / fadeInDuration));
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);

        while (text.color.a > 0f)
        {
            float alpha = Mathf.Clamp01(text.color.a - (Time.deltaTime / fadeOutDuration));
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }
    }

    
}
