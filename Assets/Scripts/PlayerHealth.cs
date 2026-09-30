using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Setting")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float deathDelay = 1f;
    [SerializeField] private HealthManagement healthManagement;

    [Header("Get Hit Setting")]
    [SerializeField] private float knockBackDuration = 0.2f;

    private Animator animator;
    private int currentHealth;
    private bool isKnockedBack = false;
    private bool isDead = false;
    private Rigidbody2D rb;

    public bool IsKnockedBack => isKnockedBack;
    public int GetCurrentHealth => currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        healthManagement.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage, Vector2 damageSource, float knockBackForce)
    {
        currentHealth -= damage;
        currentHealth = Math.Clamp(currentHealth, 0, maxHealth);

        healthManagement.UpdateHealthBar(currentHealth, maxHealth);

        float verticalKnockBack = 2f;

        float horizontalKnockBack = Mathf.Sign(transform.position.x - damageSource.x);

        Vector2 knockBackDirection = new Vector2(horizontalKnockBack, verticalKnockBack).normalized;

        StartCoroutine(KnockBack(knockBackDirection, knockBackForce));

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    private void Die()
    {
        animator.SetTrigger("Death");
        StartCoroutine(ReloadSceneAfterDelay());
    }

    IEnumerator ReloadSceneAfterDelay()
    {
        yield return new WaitForSeconds(deathDelay);
        SceneManager.LoadScene("SampleScene");
    }

    IEnumerator KnockBack(Vector2 direction, float force)
    {
        if (currentHealth <= 0)
        {
            isKnockedBack = false;
        }
        else
        {
            isKnockedBack = true;

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction * force, ForceMode2D.Impulse);

            yield return new WaitForSeconds(knockBackDuration);

            isKnockedBack = false;
        }
    }
}
