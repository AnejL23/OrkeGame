using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public string groundCheckName = "groundCheck"; // The name of the ground check object
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Transform groundCheck; // Now private, we'll assign it in Start

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Try to find the groundCheck object by name
        if (groundCheck == null)
        {
            groundCheck = transform.Find(groundCheckName);
            if (groundCheck == null)
            {
                Debug.LogError("No groundCheck found. Make sure you have a child object named exactly '" + groundCheckName + "'", this);
            }
        }
    }

    void Update()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        }

        Move();
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void OnDrawGizmos()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
