using System;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Setting")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRender;
    private bool isGround;
    private float groundCheckRadius = 0.2f;
    private bool doubleJump;
    private float doubleJumpForce;
    private PlayerHealth playerHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (playerHealth.GetCurrentHealth <= 0)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        SetAnimator();
        Move();
        Jump();
    }

    private void SetAnimator()
    {
        float speed = Math.Abs(rb.linearVelocity.x);
        animator.SetBool("isGround", isGround);
        animator.SetFloat("Speed", speed);
        animator.SetFloat("VerticalSpeed", rb.linearVelocity.y);
    }

    private void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        Flip(moveInput);
    }

    private void Jump()
    {
        doubleJumpForce = jumpForce * 0.75f;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGround)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                doubleJump = true;
            }
            else if (doubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
                doubleJump = false;
            }
        }
    }

    private void Flip(float moveInput)
    {
        if (moveInput > 0)
        {
            spriteRender.flipX = false;
        }
        else if (moveInput < 0)
        {
            spriteRender.flipX = true;
        }
    }
}
