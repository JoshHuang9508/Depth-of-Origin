using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GamePause_UI_Controller : MonoBehaviour
{
    UIDocument Options_UI;

    void Start()
    {
        Options_UI = GetComponentInParent<UIDocument>();
        Options_UI.rootVisualElement.style.display = DisplayStyle.None;
    }
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button btn_continue = root.Q<Button>("btn_continue");

        btn_continue.clicked += () => GameContinue();
        btn_continue.clicked += () => Debug.Log("click continue");
    }

    public void GameContinue()
    {
        Debug.Log("ContinueGame");
        Time.timeScale = 1;
        Options_UI.rootVisualElement.style.display = DisplayStyle.None;
    }

    public void GamePause()
    {
        Debug.Log("PauseGame");
        Time.timeScale = 0;
        Options_UI.rootVisualElement.style.display = DisplayStyle.Flex;
    }
}
