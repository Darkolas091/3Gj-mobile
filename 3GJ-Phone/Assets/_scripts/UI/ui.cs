using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ui : MonoBehaviour
{
    public static bool isRestart = false;
    [Header("UI Panels")]
    public GameObject mainMenu;
    public GameObject gameOverScreen;
    public GameObject optionsPanel;
    public GameObject howToPlayPanel; 

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        gameOverScreen.SetActive(false);
        optionsPanel.SetActive(false);
        howToPlayPanel.SetActive(false); 
        Time.timeScale = 0f;
    }

    public void StartGameDirect()
    {
        mainMenu.SetActive(false);
        gameOverScreen.SetActive(false);
        optionsPanel.SetActive(false);
        howToPlayPanel.SetActive(false); 
        Time.timeScale = 1f;
    }

    public void ShowOptions()
    {
        optionsPanel.SetActive(true);
        mainMenu.SetActive(false);
        gameOverScreen.SetActive(false);
        howToPlayPanel.SetActive(false); 
    }

    public void ShowHowToPlay()
    {
        howToPlayPanel.SetActive(true);
        mainMenu.SetActive(false);
        gameOverScreen.SetActive(false);
        optionsPanel.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        optionsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        gameOverScreen.SetActive(false);
    }
}
