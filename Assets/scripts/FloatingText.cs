using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Remove this line if you are not using TextMeshPro

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float fadeSpeed = 1f;
    public float lifetime = 1f;

    private TextMeshProUGUI textMesh; // Change to 'private Text text;' if you're using the standard Text UI
    private float timer;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>(); // Change to 'GetComponent<Text>();' if you're using the standard Text UI
    }

    private void Update()
    {
        // Move the text upward
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Fade out the text over time
        timer += Time.deltaTime;
        if (textMesh != null)
        {
            textMesh.alpha = Mathf.Clamp01(1 - (timer / fadeSpeed));
        }

        // Destroy the text after it has existed for its lifetime
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}