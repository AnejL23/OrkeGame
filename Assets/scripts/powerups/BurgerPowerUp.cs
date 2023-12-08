using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BurgerPowerUp : MonoBehaviour
{
    public float duration = 10f; // Duration the power-up lasts
    public AudioClip pickupSound; // Assign this in the Inspector

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    void Start()
    {
        // Get the SpriteRenderer component from the GameObject where this script is attached
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Get the AudioSource component from the GameObject where this script is attached
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Burger picked up by player.");
            StartCoroutine(Pickup(other));
        }
    }

    IEnumerator Pickup(Collider2D player)
    {
        Debug.Log("Starting power-up coroutine.");
        // Play the pickup sound
        if (audioSource != null && pickupSound != null)
        {
            audioSource.PlayOneShot(pickupSound);
            Debug.Log("Should be playing sound now");
        }
        else
        {
            Debug.Log("AudiSource or audico clip is missing");
        }

            // Hide the burger sprite
            if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        // Grant the player the power to destroy blocks
        PlayerDestruction playerDestruction = player.GetComponent<PlayerDestruction>();
        if (playerDestruction != null)
        {
            playerDestruction.canDestroy = true;
        }

        // Wait for the duration of the power-up
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(duration);

        // After duration ends, remove the power to destroy blocks
        if (playerDestruction != null)
        {
            playerDestruction.canDestroy = false;
        }

        // Destroy the power-up object
        Debug.Log("Destroying burger GameObject.");
        Destroy(gameObject);
    }
}