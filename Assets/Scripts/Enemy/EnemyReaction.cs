using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyReaction : MonoBehaviour
{
    [SerializeField] private float deathDelay = 1f;

    private readonly int DeathHash = Animator.StringToHash("Death");
    private readonly int HitDash = Animator.StringToHash("Hit");

    private Health health;
    private Animator animator;
    private string soundId = "Enemy";
    private string enemyHit = "Enemy_Hit";
    private string enemyDeath = "Enemy_Death";

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
        SoundManager.Instance.PlayClip(soundId, enemyHit);
    }

    private void OnDie()
    {
        animator.SetTrigger(DeathHash);
        SoundManager.Instance.PlayClip(soundId, enemyDeath);
        Destroy(gameObject, deathDelay);
    }
}
