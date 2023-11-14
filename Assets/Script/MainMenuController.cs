using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using TMPro;
using System;
using UnityEngine.Video;

public class MainMenuController : MonoBehaviour
{
    [Header("GameLogo Settings")]
    public GameObject mainmenu,logopanel;
    public Image backgroundImage;
    public VideoPlayer videoplayer;
    public float fadeInDuration = 0.5f, fadeOutDuration = 5.0f;

    [Header("Volume Settings")]
    public TMP_Text volumeValueText;
    public Slider volumeSlider;
    public float defaultVolume = 50f;

    [Header("Graphic Settings")]
    private bool isfullscreen;
    [Space(10)]
    public Toggle fullscreenToggle;


    [Header("Level To load")]
    public string newGameLevel;
    public string leveltoLoad;
    public GameObject NoSavedGameDialog;

    [Header("Resolution DropDown")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;



    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        logopanel.SetActive(true);
        mainmenu.SetActive(false);
        StartCoroutine(OpenMainMenu());
    }

    IEnumerator OpenMainMenu()
    {
        yield return new WaitForSeconds(4f);
        logopanel.SetActive(false);
        mainmenu.SetActive(true);
        StartCoroutine(MainMenuFadeIn());
    }

    IEnumerator MainMenuFadeIn()
    {
        float elapsedTime = 0f;
        Color imageColor = backgroundImage.color;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            imageColor.a = Mathf.Clamp01(elapsedTime / fadeInDuration);
            backgroundImage.color = imageColor;
            yield return null;
        }

    }

    public void SetResolution(int ResolutionIndex)
    {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }



    public void NewGameYesClicked()
    {
        SceneManager.LoadScene(newGameLevel);
    }
    
    public void LoadGameYesClicked()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            leveltoLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(leveltoLoad);
        }
        else
        {
            NoSavedGameDialog.SetActive(true);
        }
    }
    public void QuitBtnClicked()
    {
        Application.Quit();
    }

    public void ResetButton(string type)
    {
        if(type == "Graphic")
        {
            fullscreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentresolution = Screen.currentResolution;
            Screen.SetResolution(currentresolution.width, currentresolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicApply();
        }
        if(type == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeValueText.text = defaultVolume.ToString()+"%";
            VolumeApply();
        }
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeValueText.text = volume.ToString()+"%";
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }

    public void setFullScreen(bool _isfullscreen)
    {
        isfullscreen = _isfullscreen;
    }
    public void GraphicApply()
    {

        PlayerPrefs.SetInt("masterfullscreen", isfullscreen ? 1 : 0);
        Screen.fullScreen = isfullscreen;

    }

}
