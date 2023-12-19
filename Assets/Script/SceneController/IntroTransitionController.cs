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
        StartCoroutine(SetTextContent("你走進了森林"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(seconddialog());
    }
    IEnumerator seconddialog()
    {
        StartCoroutine(SetTextContent("過了很長一段時間"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine (thirddialog());
    }
    IEnumerator thirddialog()
    {
        StartCoroutine(SetTextContent("幾道刺眼的光透射進來"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(fourthdialog());
    }
    IEnumerator fourthdialog()
    {
        StartCoroutine(SetTextContent("你知道你已穿過森林"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(fifthdialog());
    }
    IEnumerator fifthdialog()
    {
        StartCoroutine(SetTextContent("隨之映入眼簾的"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(sixthdialog());
    }
    IEnumerator sixthdialog()
    {
        StartCoroutine(SetTextContent("是一座生氣盎然的村莊。"));
        yield return new WaitForSeconds(waitTime);
        PlayerPrefs.SetInt("loadscene", 4);
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
