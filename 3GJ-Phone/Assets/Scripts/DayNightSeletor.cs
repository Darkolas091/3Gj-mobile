using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayNightSelector : MonoBehaviour
{
    [Header("UI References")] [SerializeField]
    private Image buttonIcon;

    [SerializeField] private Image buttonBackground;
    [SerializeField] private Button selectButton;
    [SerializeField] private TMP_Text itemCounterText;
    [SerializeField] private TMP_Text itemNameText;

    [Header("Visual Settings")] [SerializeField]
    private Color dayColor = new Color(1f, 0.9f, 0.4f);

    [SerializeField] private Color nightColor = new Color(0.2f, 0.2f, 0.5f);
    [SerializeField] private float transitionDuration = 0.3f;

    [Header("Item Lists")] [SerializeField]
    private List<ItemData> vegetables = new List<ItemData>();

    [SerializeField] private List<ItemData> weapons = new List<ItemData>();


    private GameManager.GameState currentState;
    private int currentVegetableIndex = 0;
    private int currentWeaponIndex = 0;

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
        selectButton.onClick.AddListener(OnButtonClick);


        if (GameManager.instance != null)
        {
            currentState = GameManager.instance.state;
            UpdateVisuals();
        }
    }


    private void OnGameStateChanged(GameManager.GameState newState)
    {
        currentState = newState;
        StartCoroutine(TransitionToNewState());
    }

    private System.Collections.IEnumerator TransitionToNewState()
    {
        float elapsed = 0;
        Color startColor = buttonBackground.color;
        Color targetColor = currentState == GameManager.GameState.Day ? dayColor : nightColor;
        Vector3 startScale = transform.localScale;
        Vector3 pulseScale = startScale * 1.15f;


        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;


            buttonBackground.color = Color.Lerp(startColor, targetColor, t);


            float scaleT = Mathf.Sin(t * Mathf.PI);
            transform.localScale = Vector3.Lerp(startScale, pulseScale, scaleT);

            yield return null;
        }

        buttonBackground.color = targetColor;
        transform.localScale = startScale;

        UpdateVisuals();
        Handheld.Vibrate();
    }

    public void OnButtonClick()
    {
        Handheld.Vibrate();

        if (currentState == GameManager.GameState.Day)
        {
            CycleVegetable();
        }
        else
        {
            CycleWeapon();
        }

        StartCoroutine(ButtonPressAnimation());
    }

    private void CycleVegetable()
    {
        if (vegetables.Count == 0) return;

        currentVegetableIndex = (currentVegetableIndex + 1) % vegetables.Count;
        UpdateVisuals();


        OnVegetableSelected?.Invoke(vegetables[currentVegetableIndex]);

        Debug.Log($"Selected Vegetable: {vegetables[currentVegetableIndex].itemName}");
    }

    private void CycleWeapon()
    {
        if (weapons.Count == 0) return;

        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        UpdateVisuals();


        OnWeaponSelected?.Invoke(weapons[currentWeaponIndex]);

        Debug.Log($"Selected Weapon: {weapons[currentWeaponIndex].itemName}");
    }

    private void UpdateVisuals()
    {
        ItemData currentItem = GetCurrentItem();

        if (currentItem != null)
        {
            buttonIcon.sprite = currentItem.itemIcon;

            if (itemNameText != null)
            {
                itemNameText.text = currentItem.itemName;
            }
        }


        Color targetColor = currentState == GameManager.GameState.Day ? dayColor : nightColor;
        buttonBackground.color = targetColor;


        if (itemCounterText != null)
        {
            if (currentState == GameManager.GameState.Day)
            {
                itemCounterText.text = $"{currentVegetableIndex + 1}/{vegetables.Count}";
            }
            else
            {
                itemCounterText.text = $"{currentWeaponIndex + 1}/{weapons.Count}";
            }
        }
    }

    public ItemData GetCurrentItem()
    {
        if (currentState == GameManager.GameState.Day && vegetables.Count > 0)
        {
            return vegetables[currentVegetableIndex];
        }
        else if (currentState == GameManager.GameState.Night && weapons.Count > 0)
        {
            return weapons[currentWeaponIndex];
        }

        return null;
    }

    private System.Collections.IEnumerator ButtonPressAnimation()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 pressedScale = originalScale * 0.9f;
        float duration = 0.1f;
        float elapsed = 0;


        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, pressedScale, elapsed / duration);
            yield return null;
        }

        elapsed = 0;


        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(pressedScale, originalScale, elapsed / duration);
            yield return null;
        }

        transform.localScale = originalScale;
    }


    public System.Action<ItemData> OnVegetableSelected;
    public System.Action<ItemData> OnWeaponSelected;
}