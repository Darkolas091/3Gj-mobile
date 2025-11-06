using UnityEngine;
using TMPro;

public class DayCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    
    private int currentDay = 1;
    private int cyclesCompleted = 0;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void Start()
    {
        UpdateDayDisplay();
    }

    private void OnGameStateChanged(GameManager.GameState newState)
    {
        // Increment day when transitioning from night to day
        if (newState == GameManager.GameState.Day)
        {
            cyclesCompleted++;
            if (cyclesCompleted > 0)
            {
                currentDay++;
                UpdateDayDisplay();
            }
        }
    }
    
    private void UpdateDayDisplay()
    {
        if (dayText != null)
        {
            dayText.text = $"Day {currentDay}";
        }
    }

    public int GetCurrentDay() => currentDay;
}