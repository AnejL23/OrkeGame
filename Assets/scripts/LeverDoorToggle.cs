using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverDoorToggle : MonoBehaviour
{
    public Animator leverAnimator;
    public Animator doorAnimator;
    public Collider2D doorCollider; // Assign this in the inspector

    private bool isOpen = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the 'other' collider is your player
        if (other.CompareTag("Player"))
        {
            ToggleDoor();
        }
    }

    private void ToggleDoor()
    {
        // Toggle the isOpen state
        isOpen = !isOpen;

        // Set the Animator parameter to the state of isOpen
        leverAnimator.SetBool("isOpen", isOpen);
        doorAnimator.SetBool("isOpen", isOpen);

        // Update the door's collider state
        doorCollider.enabled = !isOpen;
    }
}
