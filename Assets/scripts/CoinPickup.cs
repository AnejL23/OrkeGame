using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CoinPickup : MonoBehaviour
{
    public AudioClip pickupSound;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddCoins(1);
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            Destroy(gameObject); // Destroy the coin
        }
    }
}
