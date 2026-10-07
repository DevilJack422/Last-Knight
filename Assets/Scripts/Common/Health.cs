using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    private IDamageBlocker blocker;

    //Action cho thanh mau <current, max>
    public event Action<int, int> HealthChanged;
    //Action hit <nguon sat thuong, luc day>
    public event Action<Vector2, float> Damaged;
    public event Action Died;

    public int currentHealth {get; private set;}
    public int MaxHealth => maxHealth;
    public bool IsDead {get; private set;}

    void Awake()
    {
        currentHealth = maxHealth;
        blocker = GetComponent<IDamageBlocker>();
    }

    public void TakeDamage(int damage, Vector2 damageSource, float knockBackForce)
    {
        if (IsDead) return;
        if (blocker != null && blocker.TryBlock(damageSource))
        {
            Debug.Log("Attack blocked");
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        HealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            IsDead = true;
            Died?.Invoke();
        }
        else
        {
            Damaged?.Invoke(damageSource, knockBackForce);
        }
    }
}
