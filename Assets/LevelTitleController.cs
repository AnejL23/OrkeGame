using System.Collections;
using System.Collections.Generic;
using TMPro; // Include the TextMeshPro namespace
using UnityEngine;

public class LevelTitleController : MonoBehaviour
{
    public TextMeshProUGUI levelTitle; // Change the type to TextMeshProUGUI
    public float displayTime = 3.0f;
    public float fadeTime = 1.0f;
    public AudioSource audioSource;

    void Start()
    {
        if (levelTitle != null)
        {
            // Play the sound effect
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }

            StartCoroutine(ShowAndHideTitle());
        }
    }

    IEnumerator ShowAndHideTitle()
    {
        // Display the title for the designated time
        yield return new WaitForSeconds(displayTime);

        // Fade out the title
        float elapsed = 0.0f;
        Color originalColor = levelTitle.color;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1.0f - (elapsed / fadeTime));
            levelTitle.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Optionally deactivate the GameObject or disable the component
        levelTitle.gameObject.SetActive(false);
    }
}