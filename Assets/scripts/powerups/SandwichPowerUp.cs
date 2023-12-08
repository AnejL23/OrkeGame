using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandwichPowerUp : MonoBehaviour
{
    public float duration = 12f; // ?as trajanja neskon?ne vzdržljivosti
    public float speedBoost = 10f; // pove?anje hitrosti
    public float speedDuration = 10f; // ?as trajanja pove?anja hitrosti

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Power-Up zaznan - igralec je zaužil Sandwich Power-Up");
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ActivateSuperPower(duration, speedBoost, speedDuration);
                // ... (ostala koda)
            }
            else
            {
                Debug.Log("Napaka: PlayerMovement komponenta ni najdena na igralcu!");
            }
        }
    }
}
