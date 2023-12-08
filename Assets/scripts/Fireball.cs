using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            HealthManager healthManager = hitInfo.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                // Calculate the direction from the fireball to the player
                Vector2 knockbackDirection = (hitInfo.transform.position - transform.position).normalized;
                // Pass the damage and the knockback direction to the TakeDamage method
                healthManager.TakeDamage(damage, knockbackDirection);
            }
            Destroy(gameObject); // Destroy the fireball on impact
        }
    }
}