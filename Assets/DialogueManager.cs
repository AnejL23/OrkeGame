using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this namespace for TextMeshPro

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Changed from Text to TextMeshProUGUI

    public void DisplayText(string text)
    {
        dialogueText.text = text;
    }
}

