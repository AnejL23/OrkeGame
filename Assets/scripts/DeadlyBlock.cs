using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadlyBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is tagged as "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has collided with a deadly block.");

            // Insert any additional game over logic here
            // For example, you might want to play a death animation or sound

            // After handling the game over logic, you can reload the scene or load a game over scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GameManager.instance.GameOver();
        }
    }
}


