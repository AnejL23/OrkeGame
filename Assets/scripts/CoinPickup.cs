using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CoinPickup : MonoBehaviour
{
    public AudioClip pickupSound;
    public GameObject plusOnePrefab; // Assign your +1 prefab in the inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddCoins(1);
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Instantiate the +1 prefab at the coin's position
            Instantiate(plusOnePrefab, transform.position, Quaternion.identity, other.transform);

            Destroy(gameObject); // Destroy the coin
        }
    }
}
