using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public HealthManager healthManager;
    public int damage = 1;
    private bool canDamage = true;

    private void ResetDamage()
    {
        canDamage = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canDamage && healthManager != null)
        {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            healthManager.TakeDamage(damage, knockbackDirection);
            canDamage = false;
            Invoke("ResetDamage", 2f);
        }
    }
}