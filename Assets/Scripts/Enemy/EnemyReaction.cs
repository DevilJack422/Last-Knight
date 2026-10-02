using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyReaction : MonoBehaviour
{
    [SerializeField] private float deathDelay = 1f;

    private readonly int DeathHash = Animator.StringToHash("Death");
    private readonly int HitDash = Animator.StringToHash("Hit");

    private Health health;
    private Animator animator;

    void Awake()
    {
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        health.Damaged += OnDamage;
        health.Died += OnDie;
    }

    void OnDisable()
    {
        health.Damaged -= OnDamage;
        health.Died -= OnDie;
    }

    private void OnDamage(Vector2 damageSource, float force)
    {
        animator.SetTrigger(HitDash);
    }

    private void OnDie()
    {
        animator.SetTrigger(DeathHash);
        Destroy(gameObject, deathDelay);
    }
}
