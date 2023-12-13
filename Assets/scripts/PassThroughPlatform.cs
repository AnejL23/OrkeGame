using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughPlatform : MonoBehaviour
{
    private Collider2D _collider;
    private bool _playerOnPlatform;
    private Rigidbody2D rb;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (_playerOnPlatform && Input.GetKeyDown(KeyCode.S))
        {
            _collider.enabled = false;
            if (rb != null)
            {
                rb.velocity = new Vector2(rb.velocity.x, -1f); // Force the player to move down
            }
            StartCoroutine(EnableCollider());
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(1f); // Increase the wait time
        _collider.enabled = true;
    }

    private void SetPlayerOnPlatform(Collision2D other, bool value)
    {
        var player = other.gameObject.GetComponent<PlayerMovement>(); // Replace 'Player' with your player component's name if it's different
        if (player != null)
        {
            _playerOnPlatform = value;
            Debug.Log("Player on platform: " + value);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        SetPlayerOnPlatform(other, true);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        SetPlayerOnPlatform(other, false);
    }
}

    

