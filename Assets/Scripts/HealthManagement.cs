using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class HealthManagement : MonoBehaviour
{
    [SerializeField] private Image healthAmount;

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthAmount.fillAmount = (float)currentHealth / maxHealth;
    }
}
