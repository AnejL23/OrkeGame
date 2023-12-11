using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float maxFallSpeed = -10f;
    public float sprintSpeed = 8f;
    public float jumpForce = 5f;
    private float jumpTimer;
    public float maxJumpHoldTime = 0.5f;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    [SerializeField] private LayerMask platformLayerMask;
    private BoxCollider2D playerCollider;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private float originalSpeed;
    public float maxStamina = 100f;
    private float currentStamina;
    public float bounceForce = 5f;
    private float staminaDrain = 40f;
    private float staminaRegen = 1f;
    private bool isSprinting;
    private bool isClimbing;
    private LadderClimb currentLadder;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private ParticleSystem smokeEffect;
    [SerializeField] private Collider2D playerFeetCollider;
    private bool isDropping;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalSpeed = moveSpeed;
        currentStamina = maxStamina;
        playerCollider = GetComponent<BoxCollider2D>();
        if (smokeEffect == null)
        {
            smokeEffect = GetComponentInChildren<ParticleSystem>();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for collision with enemy head
        if (collision.gameObject.CompareTag("EnemyHead") && IsPlayerComingFromAbove(collision))
        {
            rb.velocity = new Vector2(rb.velocity.x, bounceForce);
            collision.gameObject.GetComponentInParent<EnemyAI>().DefeatEnemy();
            return; // Exit the method to prevent other collision checks
        }

        // Check for collision with enemy body
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
            GetComponent<HealthManager>().TakeDamage(1, knockbackDir);
            return; // Exit the method to prevent other collision checks
        }

        // Check for collision with one-way platforms
        if (collision.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"))
        {
            // If the player is moving upwards (jumping), ignore the platform collision
            if (rb.velocity.y > 0)
            {
                Physics2D.IgnoreCollision(playerCollider, collision.collider, true);
            }
            // If the player is not moving upwards or is dropping, enable platform collision
            else if (rb.velocity.y <= 0 && !isDropping)
            {
                Physics2D.IgnoreCollision(playerCollider, collision.collider, false);
            }
        }
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        Sprint();
        HandleSmokeEffect();
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius, groundLayer);
        isGrounded = hit.collider != null;
        animator.SetBool("IsGrounded", isGrounded);
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || isClimbing))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFalling", false);
            isClimbing = false;
            jumpTimer = Time.time;
        }
        else if (Input.GetKey(KeyCode.Space) && Time.time - jumpTimer < maxJumpHoldTime)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + (jumpForce * Time.deltaTime));
        }
        if (!isGrounded && rb.velocity.y < 0 && !isClimbing)
        {
            animator.SetBool("IsFalling", true);
            animator.SetBool("IsJumping", false);
        }
        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * currentLadder.climbSpeed);
            rb.gravityScale = 0;
            if (verticalInput != 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, verticalInput * currentLadder.climbSpeed);
            }
        }
        else
        {
            rb.gravityScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.S) && !isGrounded)
        {
            // Only allow the player to drop if they are on a one-way platform
            Collider2D colliderBelow = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, platformLayerMask);
            if (colliderBelow != null)
            {
                StartCoroutine(DropDown());
            }
        }

        if (isGrounded)
        {
            animator.SetBool("IsFalling", false);
        }
    }

    void FixedUpdate()
    {
        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    void Sprint()
    {
        bool sprintKeyPressed = Input.GetKey(KeyCode.LeftShift);
        float moveInput = Input.GetAxis("Horizontal");
        isSprinting = sprintKeyPressed && Mathf.Abs(moveInput) > 0f && currentStamina > 0;
        if (!float.IsInfinity(currentStamina))
        {
            isSprinting = sprintKeyPressed && Mathf.Abs(moveInput) > 0f && currentStamina > 0;
            if (isSprinting)
            {
                currentStamina -= staminaDrain * Time.deltaTime;
                moveSpeed = sprintSpeed;
            }
            else
            {
                if (!sprintKeyPressed && currentStamina < maxStamina)
                {
                    currentStamina += staminaRegen * Time.deltaTime;
                }
                moveSpeed = 2f;
            }
        }
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateStaminaSlider();
    }

    void UpdateStaminaSlider()
    {
        if (staminaSlider != null && !float.IsInfinity(currentStamina))
        {
            staminaSlider.value = currentStamina / maxStamina;
        }
    }

    public void RefillStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateStaminaSlider();
    }

    public void ActivateSuperPower(float staminaDuration, float extraSpeed, float speedDuration)
    {
        StartCoroutine(SuperStamina(staminaDuration));
        StartCoroutine(SpeedBoost(extraSpeed, speedDuration));
    }

    private IEnumerator SuperStamina(float duration)
    {
        float previousStamina = currentStamina;
        currentStamina = Mathf.Infinity;
        UpdateStaminaSlider();
        yield return new WaitForSeconds(duration);
        currentStamina = previousStamina;
        UpdateStaminaSlider();
    }

    private IEnumerator SpeedBoost(float extraSpeed, float duration)
    {
        moveSpeed += extraSpeed;
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
    }

    public void ActivatePuddingPower(float duration)
    {
        StartCoroutine(PuddingEffect(duration));
    }

    private IEnumerator PuddingEffect(float duration)
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("PassThroughWalls"), true);
        yield return new WaitForSeconds(duration);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("PassThroughWalls"), false);
    }

    private IEnumerator DropDownCoroutine()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("OneWayPlatform"), true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("OneWayPlatform"), false);
    }

    public void SetClimbing(bool climbing, LadderClimb ladder = null)
    {
        isClimbing = climbing;
        currentLadder = ladder;
        GetComponent<Rigidbody2D>().gravityScale = climbing ? 0 : 1;
    }

    private bool IsPlayerComingFromAbove(Collision2D collision)
    {
        Vector2 highestContactPoint = collision.contacts[0].point;
        foreach (var contact in collision.contacts)
        {
            if (contact.point.y > highestContactPoint.y)
            {
                highestContactPoint = contact.point;
            }
        }
        return highestContactPoint.y < transform.position.y;
    }


    private IEnumerator DropDown()
    {
        isDropping = true;
        
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("OneWayPlatform"), true);
        yield return new WaitForSeconds(0.5f); 
        isDropping = false;
        
        yield return new WaitForSeconds(0.2f);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("OneWayPlatform"), false);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"))
        {
            Physics2D.IgnoreCollision(playerFeetCollider, collision.collider, false);
        }
    }

    private void HandleSmokeEffect()
    {
        if (smokeEffect != null)
        {
            if (Mathf.Abs(rb.velocity.x) > 0.1f && isGrounded)
            {
                if (!smokeEffect.isPlaying)
                {
                    smokeEffect.Play();
                }
                var shapeModule = smokeEffect.shape;
                shapeModule.rotation = new Vector3(0, rb.velocity.x > 0 ? 180 : 0, 0);
            }
            else
            {
                if (smokeEffect.isPlaying)
                {
                    smokeEffect.Stop();
                }
            }
        }
        else
        {
            Debug.LogWarning("SmokeEffect not set on " + gameObject.name);
        }
    }
}