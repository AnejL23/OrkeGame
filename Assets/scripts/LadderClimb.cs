using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    public float climbSpeed = 3f;
    private bool isClimbing = false;
    private Rigidbody2D rb;

    // Define keys for climbing up and down
    public KeyCode climbUpKey = KeyCode.W;
    public KeyCode climbDownKey = KeyCode.S;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isClimbing)
        {
            // Check if the climb up or down keys are pressed
            if (Input.GetKey(climbUpKey))
            {
                rb.velocity = new Vector2(rb.velocity.x, climbSpeed);
            }
            else if (Input.GetKey(climbDownKey))
            {
                rb.velocity = new Vector2(rb.velocity.x, -climbSpeed);
            }
            else
            {
                // If no keys are pressed, stop the vertical movement
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.SetClimbing(true, this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.SetClimbing(false);
            }
        }
    }
}