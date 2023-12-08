using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestruction : MonoBehaviour
{
    public bool canDestroy = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only destroy blocks if we have the power-up
        if (canDestroy && collision.gameObject.CompareTag("Destructible"))
        {
            // Check if we hit the block from the side
            Vector2 normal = collision.contacts[0].normal;
            if (Mathf.Abs(normal.x) > 0.5f) // Adjust this value as necessary
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
