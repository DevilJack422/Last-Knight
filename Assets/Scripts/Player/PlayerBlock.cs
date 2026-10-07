using System;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(PlayerMovement), typeof(PlayerHitReaction))]
public class PlayerBlock : MonoBehaviour, IDamageBlocker
{
    [Header("Block")]
    [Tooltip("Cool down time after block got breaked")]
    [SerializeField] private float blockCoolDown = 0.5f;
    [Tooltip("On: Block only in font. Off: Block in any direction")]
    [SerializeField] private bool onlyBlockFont = false;
    [Tooltip("ON: Block an attack and got breaked, OFF: Can block as long as J is pressed")]
    [SerializeField] private bool breakOnBlockedHit = true;
    

    private static readonly int IsBlockingHash = Animator.StringToHash("isBlocking");

    private Health health;
    private PlayerMovement movement;
    private PlayerHitReaction hitReaction;
    private Animator animator;
    private bool hasBlockingParam;

    private float coolDownEndTime;
    private bool ignoreHeldMove;

    public event Action Blocked;

    public bool IsBlocking {get; private set;}

    void Awake() 
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        hitReaction = GetComponent<PlayerHitReaction>();
        health = GetComponent<Health>();

        hasBlockingParam = HasAnimatorParameter("isBlocking");
    }

    void Update()
    {
        if (IsBlocking)
        {
            UpdateBlocking();
        }
        else
        {
            TryStartBlock();
        }
    }

    void OnDisable()
    {
        IsBlocking = false;
    }

    private void TryStartBlock()
    {
        if (!Input.GetKeyDown(KeyCode.J)) return;
        if (Time.time < coolDownEndTime) return;
        if (!CanActNow()) return;

        IsBlocking = true;
        ignoreHeldMove = Input.GetAxisRaw("Horizontal") != 0f;
        SetAnimator(true);
    }

    private void UpdateBlocking()
    {
        if (!Input.GetKey(KeyCode.J))
        {
            EndBlock(startCoolDown: false);
            return;
        }

        if (!CanActNow())
        {
            EndBlock(startCoolDown: true);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.K))
        {
            EndBlock(startCoolDown: true);
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal == 0f)
        {
            ignoreHeldMove = false;
        }
        else if (!ignoreHeldMove)
        {
            EndBlock(startCoolDown: true);
        }
    }

    private void EndBlock(bool startCoolDown)
    {
        IsBlocking = false;
        SetAnimator(false);

        if (startCoolDown)
        {
            coolDownEndTime = Time.time + blockCoolDown;
        }
    }

    private bool CanActNow()
    {
        if (health.IsDead) return false;
        if (hitReaction.IsKnockedBack) return false;
        return true;
    }

    public bool TryBlock(Vector2 damageSource)
    {
        if (!IsBlocking) return false;

        if (onlyBlockFont)
        {
            float directionToSource = Mathf.Sign(damageSource.x - transform.position.x);
            if (directionToSource != movement.FacingDirection) return false;
        }

        Blocked?.Invoke();

        if (breakOnBlockedHit)
        {
            EndBlock(startCoolDown: true);
        }
        return true;
    }

    private void SetAnimator(bool blocking)
    {
        if (hasBlockingParam)
        {
            animator.SetBool(IsBlockingHash, blocking);
        }
    }

    private bool HasAnimatorParameter(String parameterName)
    {
        foreach (AnimatorControllerParameter p in animator.parameters)
        {
            if (p.name == parameterName) return true;
        }
        return false;
    }
}
