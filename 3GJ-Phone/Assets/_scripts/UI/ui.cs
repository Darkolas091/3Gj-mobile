using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ui : MonoBehaviour
{
    public static bool isRestart = false;
    [Header("UI Panels")]
    public GameObject mainMenu;
    public GameObject gameOverScreen;
    public GameObject optionsPanel;
    public GameObject PausePanel;
    public GameObject howToPlay;
    public GameObject credits;
    private bool isPaused = false;

    [Header("Audio Settings")]
    public Slider volumeSlider;
    public AudioSource audioSource;


    void Start()
    {

        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;


        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;


        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }
    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        gameOverScreen.SetActive(false);
        optionsPanel.SetActive(false);
        PausePanel.SetActive(false);
        credits.SetActive(false);
        Time.timeScale = 0f;
    }

    public void StartGameDirect()
    {
        mainMenu.SetActive(false);
        gameOverScreen.SetActive(false);
        optionsPanel.SetActive(false);
        PausePanel.SetActive(false);
        howToPlay.SetActive(false);
        credits.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowOptions()
    {
        optionsPanel.SetActive(true);
        mainMenu.SetActive(false);
        gameOverScreen.SetActive(false);
        PausePanel.SetActive(false);
        howToPlay.SetActive(false);
        credits.SetActive(false);

    }

    public void ShowHowToPlay()
    {
        mainMenu.SetActive(false);
        gameOverScreen.SetActive(false);
        optionsPanel.SetActive(false);
        PausePanel.SetActive(false);
        howToPlay.SetActive(false);
        credits.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        optionsPanel.SetActive(false);
        gameOverScreen.SetActive(false);
        PausePanel.SetActive(false);
        howToPlay.SetActive(false);
        credits.SetActive(false);
    }
    public void PausePanels()
    {
        mainMenu.SetActive(false);
        optionsPanel.SetActive(false);
        PausePanel.SetActive(true);
        gameOverScreen.SetActive(false);
        howToPlay.SetActive(false);
        credits.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;
    }
    public void HowToPlay ()
    {
        mainMenu.SetActive(false);
        optionsPanel.SetActive(false);
        PausePanel.SetActive(false);
        gameOverScreen.SetActive(false);
        howToPlay.SetActive(true);
        credits.SetActive(false);
    }

    public void CreditsGame()
    {
        mainMenu.SetActive(false);
        optionsPanel.SetActive(false);
        PausePanel.SetActive(false);
        gameOverScreen.SetActive(false);
        howToPlay.SetActive(false);
        credits.SetActive(true);
    }
}
   
