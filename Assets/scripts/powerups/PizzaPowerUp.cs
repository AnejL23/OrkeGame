using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaPowerUp : MonoBehaviour
{
    public GameObject transformedPlayerPrefab; // Assign the transformed player prefab in the inspector
    public float duration = 10f; // Duration of the transformation

    private GameObject originalPlayer; // To keep track of the original player

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            originalPlayer = collision.gameObject;
            StartCoroutine(TransformPlayer(collision.gameObject));
            gameObject.SetActive(false); // Deactivate the pizza power-up
        }
    }

    private IEnumerator TransformPlayer(GameObject player)
    {
        // Disable the original player
        player.SetActive(false);

        // Instantiate the transformed player prefab at the original player's position
        GameObject transformedPlayer = Instantiate(transformedPlayerPrefab, player.transform.position, Quaternion.identity);

        // Make the player invulnerable by disabling the HealthManager component or other relevant components
        HealthManager healthManager = transformedPlayer.GetComponent<HealthManager>();
        if (healthManager != null)
        {
            healthManager.enabled = false; // Disable taking damage
        }

        yield return new WaitForSeconds(duration);

        // After duration, revert back to the original player
        Destroy(transformedPlayer); // Destroy the transformed player object
        player.SetActive(true); // Re-enable the original player

        // Optionally, re-enable vulnerability
        if (healthManager != null)
        {
            healthManager.enabled = true;
        }
    }
}
