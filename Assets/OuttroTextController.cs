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
        StartCoroutine(SetTextContent("�A���J�·t"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(seconddialog());
    }

    IEnumerator seconddialog()
    {
        StartCoroutine(SetTextContent("�N��é��"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(thirddialog());
    }
    IEnumerator thirddialog()
    {
        StartCoroutine(SetTextContent("�uť���ջy����:"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(fourthdialog());
    }
    IEnumerator fourthdialog()
    {
        StartCoroutine(SetTextContent("�L�̷|�Q�X�v"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(fifthdialog());
    }
    IEnumerator fifthdialog()
    {
        StartCoroutine(SetTextContent("���L�̤��|����"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(sixthdialog());
    }
    IEnumerator sixthdialog()
    {
        StartCoroutine(SetTextContent("�o�O�L�k�קK���R�B"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(seventhdialog());
    }

    IEnumerator seventhdialog()
    {
        StartCoroutine(SetTextContent("�A�]�L�k�ڵ�"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(eighthdialog());
    }

    IEnumerator eighthdialog()
    {
        StartCoroutine(SetTextContent("�ҵ{�a"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(ninethdialog());
    }

    IEnumerator ninethdialog()
    {
        StartCoroutine(SetTextContent("ı���a"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(tenthdialog());
    }

    IEnumerator tenthdialog()
    {
        StartCoroutine(SetTextContent("�H����"));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(eleventhdialog());
    }

    IEnumerator eleventhdialog()
    {
        StartCoroutine(SetTextContent("�_�����`�C"));
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
