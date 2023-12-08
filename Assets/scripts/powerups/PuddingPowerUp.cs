using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddingPowerUp : MonoBehaviour
{
    public float duration = 10f; // Duration of the power-up effect

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().ActivatePuddingPower(duration);
           
             gameObject.SetActive(false);
        }
    }
}