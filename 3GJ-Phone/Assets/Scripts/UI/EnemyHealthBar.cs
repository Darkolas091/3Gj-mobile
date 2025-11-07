using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private CanvasGroup canvasGroup;
    
    [Header("Settings")]
    [SerializeField] private float hideDelay = 2f; // Time before fading out
    [SerializeField] private float fadeSpeed = 2f;
    
    private float currentHealth;
    private float maxHealth;
    private float hideTimer;
    private bool isVisible = false;

    private void Start()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
    }

    private void Update()
    {
        if (isVisible)
        {
            hideTimer -= Time.deltaTime;
            
            if (hideTimer <= 0f)
            {
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, fadeSpeed * Time.deltaTime);
                    
                    if (canvasGroup.alpha < 0.01f)
                    {
                        canvasGroup.alpha = 0f;
                        isVisible = false;
                    }
                }
            }
        }
    }

    public void Initialize(float max)
    {
        maxHealth = max;
        currentHealth = max;
        UpdateHealthBar();
    }

    public void UpdateHealth(float current)
    {
        currentHealth = current;
        UpdateHealthBar();
        ShowHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            float fillAmount = maxHealth > 0 ? currentHealth / maxHealth : 0f;
            healthBarFill.fillAmount = fillAmount;
            
            if (fillAmount > 0.6f)
                healthBarFill.color = Color.green;
            else if (fillAmount > 0.3f)
                healthBarFill.color = Color.yellow;
            else
                healthBarFill.color = Color.red;
        }
    }

    private void ShowHealthBar()
    {
        isVisible = true;
        hideTimer = hideDelay;
        
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }
    }

    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}

