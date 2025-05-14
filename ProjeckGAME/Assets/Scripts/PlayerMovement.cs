using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private PlayerInputActions playerInput;

    private Vector2 moveInput;
    private bool isJumping = false;

    [Header("Jump Settings")]
    [SerializeField] private LayerMask jumpableGround;
    private BoxCollider2D coll;

    private bool isAttacking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();

        playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInput.Enable();

        playerInput.Movement.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.Movement.Move.canceled += ctx => moveInput = Vector2.zero;

        playerInput.Movement.Jump.performed += ctx => Jump();
        playerInput.Movement.Attack.performed += ctx => Attack();  // Pastikan ada action "Attack"
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {
        moveInput = playerInput.Movement.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        rb.velocity = targetVelocity;

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (isAttacking)
        {
            anim.Play("attack"); // nama animasi harus sesuai yang ada di Animator
            return;
        }

        if (!isGrounded())
        {
            anim.Play("jump-all");
        }
        else if (moveInput.x != 0f)
        {
            anim.Play("run");
        }
        else
        {
            anim.Play("idle");
        }

        if (moveInput.x > 0f)
            sprite.flipX = false;
        else if (moveInput.x < 0f)
            sprite.flipX = true;
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void Jump()
    {
        if (isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        anim.Play("attack");

        yield return new WaitForSeconds(0.5f); // sesuaikan durasi animasi attack

        isAttacking = false;
    }
}
