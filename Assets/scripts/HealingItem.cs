using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    public int healAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthManager healthManager = collision.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.Heal(healAmount);
                Destroy(gameObject); // Destroy the healing item after it's used
            }
        }
    }
}