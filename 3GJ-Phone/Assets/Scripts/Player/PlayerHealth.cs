using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHearts = 3;
    [SerializeField] private int currentHearts;
    [SerializeField] private Image[] heartIcons;

    private void Start()
    {
        currentHearts = maxHearts;
        UpdateHeartUI();
    }

    public void TakeDamage(int damage)
    {
        currentHearts -= damage;
        if (currentHearts < 0) currentHearts = 0;
        UpdateHeartUI();
        if (currentHearts == 0)
        {
            Debug.Log("Player Dead");
            // TODO: Handle player death (respawn, game over, etc.)
        }
    }

    private void UpdateHeartUI()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].enabled = i < currentHearts;
        }
    }

    public void Heal(int amount)
    {
        currentHearts += amount;
        if (currentHearts > maxHearts) currentHearts = maxHearts;
        UpdateHeartUI();
    }
}