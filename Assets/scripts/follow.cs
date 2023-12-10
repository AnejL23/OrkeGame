using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign your player's transform here in the inspector
    public float smoothSpeed = 0.125f; // Adjust this if you want to smooth the camera movement
    public Vector3 offset; // Use this if you want the camera to be offset from the player's position

    public float leftLimit;
    public float rightLimit;

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Clamping the X position
        float cameraHalfWidth = Camera.main.orthographicSize * ((float)Screen.width / Screen.height);
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, leftLimit + cameraHalfWidth, rightLimit - cameraHalfWidth);

        // The Y position follows the player without clamping
        smoothedPosition.y = desiredPosition.y;

        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
