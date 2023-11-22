using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public void saveData()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("SavedLevel", currentSceneName);
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
