using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private bool isPaused = false;

    private void Start()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(isPaused);
        }
        
        // Pause/unpause time
        Time.timeScale = isPaused ? 0f : 1f;
        
        // Play UI sound
        SoundEffectManager.PlaySoundEffect(isPaused ? "PauseOpen" : "PauseClose");
    }

    public void ResumeGame()
    {
        isPaused = false;
        
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        
        Time.timeScale = 1f;
        
        SoundEffectManager.PlaySoundEffect("PauseClose");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
