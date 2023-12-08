using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPickup : MonoBehaviour
{
    public float staminaAmount = 50f; // Amount of stamina to refill

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.RefillStamina(staminaAmount);
                Destroy(gameObject); // Destroy the stamina pickup object
            }
        }
    }
}