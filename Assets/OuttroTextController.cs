using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OuttroTextController : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float fadeInDuration, fadeOutDuration;
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
        StartCoroutine(SetTextContent("你走入黑暗"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(seconddialog());
    }

    IEnumerator seconddialog()
    {
        StartCoroutine(SetTextContent("意識矇矓"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(thirddialog());
    }
    IEnumerator thirddialog()
    {
        StartCoroutine(SetTextContent("只聽見耳語說著:"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(fourthdialog());
    }
    IEnumerator fourthdialog()
    {
        StartCoroutine(SetTextContent("他們會被驅逐"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(fifthdialog());
    }
    IEnumerator fifthdialog()
    {
        StartCoroutine(SetTextContent("但他們不會消散"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(sixthdialog());
    }
    IEnumerator sixthdialog()
    {
        StartCoroutine(SetTextContent("這是無法避免的命運"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(seventhdialog());
    }

    IEnumerator seventhdialog()
    {
        StartCoroutine(SetTextContent("你也無法拒絕"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(eighthdialog());
    }

    IEnumerator eighthdialog()
    {
        StartCoroutine(SetTextContent("啟程吧"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(ninethdialog());
    }

    IEnumerator ninethdialog()
    {
        StartCoroutine(SetTextContent("覺悟吧"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(tenthdialog());
    }

    IEnumerator tenthdialog()
    {
        StartCoroutine(SetTextContent("以探索"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(eleventhdialog());
    }

    IEnumerator eleventhdialog()
    {
        StartCoroutine(SetTextContent("起源之深。"));
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(0);
    }


    IEnumerator SetTextContent(string description)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
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
