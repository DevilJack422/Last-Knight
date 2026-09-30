using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Setting")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 15f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;

    private Animator animator;
    private PlayerHealth playerHealth;
    private SpriteRenderer spriteRenderer;
    private float lastAttackTime = -999f;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (playerHealth.GetCurrentHealth <= 0) return;

        Vector3 pos =attackPoint.localPosition;
        pos.x = spriteRenderer.flipX ? -Mathf.Abs(pos.x) : Mathf.Abs(pos.x);
        attackPoint.localPosition = pos;

        if (Input.GetKeyDown(KeyCode.K) & Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");
    }

    public void DealDamageToEnemy()
    {
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemy)
        {
            SlimeControl slimeHealth = enemy.GetComponent<SlimeControl>();

            if (slimeHealth != null)
            {
                slimeHealth.TakeDamge(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}

