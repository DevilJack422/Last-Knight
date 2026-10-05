using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class SlimeAI : MonoBehaviour
{
    private enum State {Patrol, Chase, Attack}

    [Header("Patrol")]
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private Transform[] points;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float chaseSpeed = 10f;
    [SerializeField] private Transform player;

    [Header("Attack")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float knockBackForce = 2f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform attackPoint;

    private const float WaypointReachDistance = 0.25f;
    private static readonly int AttackHash = Animator.StringToHash("Attack");

    private State state = State.Patrol;
    private int waypointIndex;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Health health;
    private Health playerHealth;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        playerHealth = player.GetComponent<Health>();
    }

    void Update()
    {
        if (health.IsDead) return;

        if (playerHealth.IsDead)
        {
            Patrol();
            return;
        }

        if (state == State.Attack) return;

        AlignAttackPoint();

        state = DecideState();
        switch (state)
        {
            case State.Patrol: Patrol(); break;
            case State.Chase: ChasePlayer(); break;
            case State.Attack: StartCoroutine(AttackRoutine()); break;
        }


    }

    private State DecideState()
    {
        if (IsPlayerInAttackRange())
        {
            return State.Attack;
        }

        if (Vector2.Distance(attackPoint.position, player.position) <= detectionRange)
        {
            return State.Chase;
        }

        return State.Patrol;
    }

    private void Patrol()
    {
        if (points == null || points.Length == 0) return;

        if (Vector2.Distance(transform.position, points[waypointIndex].position) < WaypointReachDistance)
        {
            waypointIndex = (waypointIndex + 1) % points.Length;
        }

        Vector2 target = points[waypointIndex].position;
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        FaceToward(target.x);
    }

    private void ChasePlayer()
    {
        Vector2 target = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, chaseSpeed * Time.deltaTime);
        FaceToward(player.position.x);
    }

    private IEnumerator AttackRoutine()
    {
        animator.SetTrigger(AttackHash);
        FaceToward(player.position.x);

        yield return new WaitForSeconds(attackCooldown);

        state = State.Patrol; //Update se tu dong chon lai trang thai o frame sau
    }

    public void DealDamageToPlayer()
    {
        if (health.IsDead || playerHealth.IsDead) return;

        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        if (hit != null && hit.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(damage, transform.position, knockBackForce);
        }
    }

    private void FaceToward(float targetX)
    {
        spriteRenderer.flipX = (transform.position.x - targetX) > 0f;
    }

    private void AlignAttackPoint()
    {
        Vector3 pos = attackPoint.localPosition;
        pos.x = spriteRenderer.flipX ? -Mathf.Abs(pos.x) : Mathf.Abs(pos.x);
        attackPoint.localPosition = pos;
    }

    private bool IsPlayerInAttackRange()
    {
        return Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer) != null;
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
