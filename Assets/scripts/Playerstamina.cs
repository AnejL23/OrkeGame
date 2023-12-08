using UnityEngine;
using UnityEngine.UI; // Required for working with the UI

public class PlayerStamina : MonoBehaviour
{
    public float maxStamina = 100f; // The maximum stamina
    public float currentStamina; // Current stamina
    public Image staminaBar; // Reference to the UI Image for the stamina bar
    public float sprintSpeed = 8f; // Speed when sprinting
    public float walkSpeed = 5f; // Normal walking speed

    private float sprintDrain = 10f; // Stamina drain per second while sprinting
    private float staminaRegen = 5f; // Stamina regeneration per second when not sprinting
    private Rigidbody2D rb;
    private bool isSprinting;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentStamina = maxStamina; // Start with full stamina
    }

    void Update()
    {
        HandleSprinting();
        UpdateStaminaBar();
    }

    void HandleSprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        if (isSprinting)
        {
            // Decrease stamina and increase speed
            currentStamina -= sprintDrain * Time.deltaTime;
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * sprintSpeed, rb.velocity.y);
        }
        else
        {
            // Regenerate stamina
            currentStamina += staminaRegen * Time.deltaTime;
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * walkSpeed, rb.velocity.y);
        }

        // Clamp the current stamina so it doesn't exceed max limits
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }
    }
}
