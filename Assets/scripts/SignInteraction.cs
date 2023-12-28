using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInteraction : MonoBehaviour
{
    public GameObject textBubble; // Assign your text bubble GameObject here
    public float displayDuration = 3.0f; // Duration for which the text bubble will be displayed

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure your player has the tag "Player"
        {
            ShowTextBubble();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HideTextBubble(); // Hide the text bubble immediately when player leaves
        }
    }

    private void ShowTextBubble()
    {
        textBubble.SetActive(true);
        StopAllCoroutines(); // Stop any existing coroutines to reset the timer
        StartCoroutine(HideTextBubbleAfterDelay());
    }

    private IEnumerator HideTextBubbleAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        HideTextBubble();
    }

    private void HideTextBubble()
    {
        textBubble.SetActive(false);
    }
}
