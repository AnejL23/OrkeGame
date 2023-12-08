using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorTeleport : MonoBehaviour
{
    public Transform teleportDestination;
    private float disableDuration = 1f; // Time for which the collider is disabled

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collider2D destinationCollider = teleportDestination.GetComponent<Collider2D>();
            if (destinationCollider != null)
            {
                StartCoroutine(DisableColliderTemporarily(destinationCollider));
            }

            other.transform.position = teleportDestination.position;
        }
    }

    private IEnumerator DisableColliderTemporarily(Collider2D collider)
    {
        collider.enabled = false;
        yield return new WaitForSeconds(disableDuration);
        collider.enabled = true;
    }
}