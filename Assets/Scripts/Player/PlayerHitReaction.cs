using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(Rigidbody2D))]
public class PlayerHitReaction : MonoBehaviour
{
    [SerializeField] private float knockBackDuration = 0.2f;
    [SerializeField] private float verticalKnockBack = 2f;

    private static readonly int HitHash = Animator.StringToHash("Hit");

    private Health health;
    private Rigidbody2D rb;
    private Animator animator;
    private Coroutine knockBackRoutine;

    public bool IsKnockedBack {get; private set;}

    void Awake()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        health.Damaged += OnDamage;        
    }

    void OnDisable()
    {
        health.Damaged -= OnDamage;       
    }

    private void OnDamage(Vector2 damageSource, float force)
    {
        animator.SetTrigger(HitHash);

        float horizontal = Mathf.Sign(transform.position.x - damageSource.x);
        Vector2 direction = new Vector2(horizontal, verticalKnockBack);

        if (knockBackRoutine != null) StopCoroutine(knockBackRoutine);

        knockBackRoutine = StartCoroutine(KnockBack(direction, force));
    }

    private IEnumerator KnockBack(Vector2 direction, float force)
    {
        IsKnockedBack = true;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockBackDuration);

        IsKnockedBack = false;
    }
}
