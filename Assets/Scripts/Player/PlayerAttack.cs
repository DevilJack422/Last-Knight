using Unity.Multiplayer.Center.Common;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(Health))]
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 15f;
    [SerializeField] private float attackCoolDown = 2f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;

    private static readonly int AttackHash = Animator.StringToHash("Attack");

    private Animator animator;
    private Health health;
    private PlayerMovement movement;
    private PlayerBlock block;
    private float lastAttackTime = -999f;

    void Awake()
    {
        health = GetComponent<Health>();
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        block = GetComponent<PlayerBlock>();
    }

    void Update()
    {
        if (health.IsDead || (block != null && block.IsBlocking)) return;

        AlignAttackPoint();

        if (Input.GetKeyDown(KeyCode.K) && Time.time >= lastAttackTime + attackCoolDown)
        {
            Attack();
        }
    }

    private void AlignAttackPoint()
    {
        Vector3 pos = attackPoint.localPosition;
        pos.x = Mathf.Abs(pos.x) * movement.FacingDirection;
        attackPoint.localPosition = pos;
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        animator.SetTrigger(AttackHash);
    }

    public void DealDamageToEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(damage, transform.position, 0f);
            }
        }
    }

    private void OnDrawGizmos() 
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
