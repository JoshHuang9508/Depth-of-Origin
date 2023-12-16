using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0.0f;
    }

    public void saveData()
    {
        int currentSceneName = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("SavedLevel", currentSceneName);
        PlayerPrefs.Save();
    }

    public void ContinueButtonClicked()
    {
        Time.timeScale = 1.0f;
    }

    public void ExitButtonClicked()
    {
        
        SceneManager.LoadScene("Main_Menu");
    }
}
