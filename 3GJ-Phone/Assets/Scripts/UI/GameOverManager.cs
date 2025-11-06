using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;
    
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text reasonText;
    [SerializeField] private TMP_Text statsText;
    
    private bool isGameOver = false;
    
    public enum LoseReason
    {
        TombstoneDestroyed,
        PlayerDied
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void TriggerGameOver(LoseReason reason)
    {
        if (isGameOver) return;
        
        isGameOver = true;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        if (reasonText != null)
        {
            string reasonString = reason == LoseReason.TombstoneDestroyed 
                ? "The Tombstone was Destroyed!" 
                : "You Died!";
            reasonText.text = reasonString;
        }
        
        if (statsText != null)
        {
            int dayssurvived = FindAnyObjectByType<DayCounterUI>()?.GetCurrentDay() ?? 1;
            int moneyEarned = PlayerWallet.instance?.currency ?? 0;
            
            statsText.text = $"Days Survived: {dayssurvived}\nMoney Earned: ${moneyEarned}";
        }
        
        SoundEffectManager.PlaySoundEffect("GameOver");
        
        Time.timeScale = 0f;
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
