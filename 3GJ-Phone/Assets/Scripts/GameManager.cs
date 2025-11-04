using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Day/Night Settings")]
    [SerializeField] private Light sun;
    [SerializeField] private float dayDuration = 120f;   
    [SerializeField] private float nightDuration = 60f;
    private float time;
    private float cycleDuration;

    public GameState state;
    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        cycleDuration = dayDuration + nightDuration;
        UpdateGameState(GameState.Day);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= cycleDuration)
            time = 0f;

        RotateSun();
        UpdateState();
    }

    private void RotateSun()
    {
       
        float angle = (time / cycleDuration) * 360f;
        sun.transform.rotation = Quaternion.Euler(angle - 90f, 170f, 0f);

       
        float t = Mathf.Sin((time / cycleDuration) * Mathf.PI * 2f) * 0.5f + 0.5f;
        sun.intensity = Mathf.Lerp(0.1f, 1f, t);
    }

    private void UpdateState()
    {
        if (time < dayDuration && state != GameState.Day)
            UpdateGameState(GameState.Day);
        else if (time >= dayDuration && state != GameState.Night)
            UpdateGameState(GameState.Night);
    }

    private void UpdateGameState(GameState newState)
    {
        state = newState;
        Debug.Log($"Game state changed to: {newState}");
        OnGameStateChanged?.Invoke(newState);
    }

    public enum GameState
    {
        Day,
        Night
    }
}
