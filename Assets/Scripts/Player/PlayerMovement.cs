using UnityEditor.Tilemaps;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Health), typeof(PlayerHitReaction))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField, Range(0f, 1f)] private float doubleJumpMultiplier = 0.75f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private static readonly int IsGroundHash = Animator.StringToHash("isGround");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int VerticalSpeedHash = Animator.StringToHash("VerticalSpeed");

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Health health;
    private PlayerHitReaction hitReaction;
    private bool canDoubleJump;

    public bool IsGrounded { get; private set; }
    public int FacingDirection => spriteRenderer.flipX ? -1 : 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        hitReaction = GetComponent<PlayerHitReaction>();
    }

    void Update()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (health.IsDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        UpadateAnimator();
        Jump();

        if (hitReaction.IsKnockedBack) return;

        Move();
    }

    private void UpadateAnimator()
    {
        animator.SetBool(IsGroundHash, IsGrounded);
        animator.SetFloat(SpeedHash, Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat(VerticalSpeedHash, rb.linearVelocity.y);
    }

    private void Move()
    {
        float input = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(input * moveSpeed, rb.linearVelocity.y);
        Flip(input);
    }

    private void Jump()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        if (IsGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * doubleJumpMultiplier);

            canDoubleJump = false;
        }
    }

    private void Flip(float input)
    {
        if (input > 0) spriteRenderer.flipX = false;
        else if (input < 0) spriteRenderer.flipX = true;
    }
}
