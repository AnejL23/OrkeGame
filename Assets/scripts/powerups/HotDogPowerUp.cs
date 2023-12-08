using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotDogPowerUp : MonoBehaviour
{
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();
            if (playerShooting != null)
            {
                playerShooting.EnableShooting();
            }
            AudioSource.PlayClipAtPoint(pickupSound, transform.position); // Play sound
            Destroy(gameObject); // Destroy the hot dog power-up
        }
    }
}

