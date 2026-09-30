using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeControl : MonoBehaviour
{
    [Header("Health Setting")]
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private float deathDelay = 1f;

    [Header("Patrol Setting")]
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private Transform[] points;

    [Header("Detection Setting")]
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float chaseSpeed = 10f;
    [SerializeField] private Transform player;

    [Header("Attack setting")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float knockBackForce = 2f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform attackPoint;

    private int i;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private PlayerHealth playerHealth;
    private bool isAttack = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();

        playerHealth = player.GetComponent<PlayerHealth>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        if (playerHealth.GetCurrentHealth <= 0)
        {
            Patrol();
            return;
        }

        if (isAttack) return;

        Vector3 pos =attackPoint.localPosition;
        pos.x = spriteRenderer.flipX ? -Mathf.Abs(pos.x) : Mathf.Abs(pos.x);
        attackPoint.localPosition = pos;

        float distanceToPlayer = Vector2.Distance(attackPoint.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            StartCoroutine(Attack());
        }
        else if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.25f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);

        spriteRenderer.flipX = (transform.position.x - points[i].position.x) > 0f;
    }

    private void ChasePlayer()
    {
        Vector2 target = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, chaseSpeed * Time.deltaTime);
        spriteRenderer.flipX = (transform.position.x - player.position.x) > 0f;
    }

    IEnumerator Attack()
    {
        isAttack = true;
        animator.SetTrigger("Attack");

        spriteRenderer.flipX = (transform.position.x - player.position.x) > 0f;

        yield return new WaitForSeconds(attackCooldown);

        isAttack = false;
    }

    public void DealDamageToPlayer()
    {
        if (playerHealth.GetCurrentHealth <= 0 || currentHealth <= 0) return;

        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        if (hit != null)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, transform.position, knockBackForce);
            }
        }
    }

    public void TakeDamge(int damage)
    {
        currentHealth -= damage;

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
        StartCoroutine(ActionAfterDie());

    }

    IEnumerator ActionAfterDie()
    {
        yield return new WaitForSeconds(deathDelay);
        Destroy(transform.gameObject);
        transform.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
