using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Health target;
    [SerializeField] private Image healthAmount;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("HealthBar UI is not have target yet!", this);

            return;
        }

        UpdateHealthBar(target.currentHealth, target.MaxHealth);
    }

    void OnEnable()
    {
        if (target != null) target.HealthChanged += UpdateHealthBar;
    }

    void OnDisable()
    {
        if (target != null) target.HealthChanged -= UpdateHealthBar;
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthAmount.fillAmount = (float)currentHealth / maxHealth;
    }
}
