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
        if (collision.gameObject.CompareTag("EnemyHead"))
        {
            Debug.Log("Hit enemy head");
            // Bounce the player up a little
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

            // Defeat the enemy
            EnemyAI enemyAI = collision.transform.parent.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.DefeatEnemy();
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy body");
            // Handle the case where the player hits the enemy from the side or below
            Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
            GetComponent<HealthManager>().TakeDamage(1, knockbackDir);
        }
    }

    void Update()
    {
        Sprint();

        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Raycast downwards to check for ground
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius, groundLayer);
        isGrounded = hit.collider != null;

        // Debug to check if the player is considered grounded
        Debug.Log("Is Grounded: " + isGrounded);

        // Debug to show the collider's tag the player is currently on top of
        if (hit.collider != null)
        {
            Debug.Log("Standing on: " + hit.collider.tag);
        }

        animator.SetBool("IsGrounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFalling", false);

            // Debug to confirm jump
            Debug.Log("Jumping");
        }

        if (!isGrounded && rb.velocity.y < 0)
        {
            animator.SetBool("IsFalling", true);
            animator.SetBool("IsJumping", false);

            // Debug to confirm the player is falling
            Debug.Log("Falling");
        }
        if (Input.GetKeyDown(KeyCode.S) && !isGrounded)
        {
            StartCoroutine(DropDown());

            // Debug to confirm drop down
            Debug.Log("Trying to Drop Down");
        }
        else if (isGrounded)
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

    private bool IsComingFromBelow(Collider2D platform)
    {
        // Assuming the player's position is the center at their feet
        return transform.position.y < platform.bounds.max.y;
    }
}





  
