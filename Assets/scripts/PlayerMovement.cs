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
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    [SerializeField] private LayerMask platformLayerMask; // Assign the platform layer here
    private BoxCollider2D playerCollider;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool wasGroundedLastFrame;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalSpeed = moveSpeed;
        currentStamina = maxStamina;
        Debug.Log("Initial Stamina: " + currentStamina);
        UpdateStaminaSlider();

        playerCollider = GetComponent<BoxCollider2D>(); // Get the player's collider
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the enemy head
        if (collision.gameObject.CompareTag("EnemyHead"))
        {
            // Confirm the player is coming from above
            if (IsPlayerComingFromAbove(collision))
            {
                // Bounce the player up
                rb.velocity = new Vector2(rb.velocity.x, bounceForce);

                // Defeat the enemy
                collision.gameObject.GetComponentInParent<EnemyAI>().DefeatEnemy();

                // Do not proceed to take damage since we hit the enemy head
                return;
            }
        }

        // Check if the collision is with the enemy body
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // Player hits enemy body, take damage
            Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
            GetComponent<HealthManager>().TakeDamage(1, knockbackDir);
        }
    }


    void Update()
    {
        // Handle horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Handle sprinting
        Sprint();

        // Update the animator with the speed
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Check if the player is grounded
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius, groundLayer);
        isGrounded = hit.collider != null;
        animator.SetBool("IsGrounded", isGrounded);

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || isClimbing))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFalling", false);
            isClimbing = false;
        }

        // Detect if player is falling
        if (!isGrounded && rb.velocity.y < 0 && !isClimbing)
        {
            animator.SetBool("IsFalling", true);
            animator.SetBool("IsJumping", false);
        }

        // Climb up or down
        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * currentLadder.climbSpeed);
            rb.gravityScale = 0; // Remove gravity effect while climbing

            // Stop any other vertical movement while climbing
            if (verticalInput == 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }
        else if (!isClimbing && !isGrounded)
        {
            // If in the air and not climbing, make sure gravity affects the player
            rb.gravityScale = 1;
        }

        // Dropping down through platforms
        if (Input.GetKeyDown(KeyCode.S) && !isGrounded && !isClimbing)
        {
            StartCoroutine(DropDown());
        }

        // If the player is grounded, reset the falling animation
        if (isGrounded)
        {
            animator.SetBool("IsFalling", false);
        }
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
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


    //super powers za sendvic
    public void ActivateSuperPower(float staminaDuration, float extraSpeed, float speedDuration)
    {
        Debug.Log("Aktiviranje super mo?i - neomejena vzdržljivost in pove?anje hitrosti");
        StartCoroutine(SuperStamina(staminaDuration));
        StartCoroutine(SpeedBoost(extraSpeed, speedDuration));
    }

    private IEnumerator SuperStamina(float duration)
    {
        float previousStamina = currentStamina; // Shranite trenutno vrednost vzdržljivosti
        currentStamina = Mathf.Infinity; // Nastavite na neskon?no
        UpdateStaminaSlider(); // Posodobite UI
        Debug.Log("Super vzdržljivost aktivirana");

        yield return new WaitForSeconds(duration);

        currentStamina = previousStamina; // Obnovite na shranjeno vrednost vzdržljivosti
        UpdateStaminaSlider(); // Posodobite UI
        Debug.Log("Super vzdržljivost deaktivirana");
    }

    private IEnumerator SpeedBoost(float extraSpeed, float duration)
    {
        moveSpeed += extraSpeed; // pove?ajte hitrost
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed; // resetirajte hitrost na prvotno vrednost
    }



    //pudding powerUp 


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

    private IEnumerator DropDown()
    {
        // Allow the player to drop down through the platform
        if (isGrounded)
        {
            // Find all colliders that are considered one-way platforms
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, platformLayerMask);
            foreach (Collider2D collider in colliders)
            {
                // Disable the platform's collider temporarily
                collider.enabled = false;
            }
            yield return new WaitForSeconds(0.5f); // Wait for half a second
            foreach (Collider2D collider in colliders)
            {
                // Re-enable the platform's collider
                collider.enabled = true;
            }
        }
    }

    public void SetClimbing(bool climbing, LadderClimb ladder = null)
    {
        isClimbing = climbing;
        currentLadder = climbing ? ladder : null;
       // animator.SetBool("IsClimbing", climbing); // Ensure you have an IsClimbing parameter in your Animator

        if (!climbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); 
        }
    }

    private bool IsPlayerComingFromAbove(Collision2D collision)
    {
        // Get the highest contact point
        Vector2 highestContactPoint = collision.contacts[0].point;
        foreach (var contact in collision.contacts)
        {
            if (contact.point.y > highestContactPoint.y)
            {
                highestContactPoint = contact.point;
            }
        }

        // Compare the highest contact point to the center of the player
        return highestContactPoint.y < transform.position.y;
    }
}







