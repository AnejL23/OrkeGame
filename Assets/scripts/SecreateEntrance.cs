using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecreateEntrance : MonoBehaviour
{
    public SpriteRenderer wallSprite; // Assign this in the inspector

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Make the wall semi-transparent to reveal the secret entrance
            wallSprite.color = new Color(wallSprite.color.r, wallSprite.color.g, wallSprite.color.b, 0.0f);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Return the wall to full opacity when the player exits
            wallSprite.color = new Color(wallSprite.color.r, wallSprite.color.g, wallSprite.color.b, 1f);
        }
    }
}