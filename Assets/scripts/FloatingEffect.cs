using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    public float amplitude = 0.09f; // The height of the floating effect
    public float frequency = 1f;   // The speed of the floating effect

    private Vector2 startPos;
    private float tempVal;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        tempVal = Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = new Vector2(startPos.x, startPos.y + tempVal);
    }
}