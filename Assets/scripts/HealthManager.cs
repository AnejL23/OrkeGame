using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public Image[] fullHearts;
    public Image[] emptyHearts;
    public AudioClip damageSound;
    public float knockbackForce = 0.5f;
    private bool isInvincible = false;
    private Rigidbody2D playerRigidbody;
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        playerRigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        UpdateHeartUI();
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        UpdateHeartUI();
        audioSource.PlayOneShot(damageSound);

        // Apply knockback force and make the player invincible for a short time
        if (playerRigidbody)
        {
            playerRigidbody.velocity = Vector2.zero; // Reset the player's velocity before applying knockback force
            playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead");
            // Handle player death here (disable player control, trigger death animation, etc.)
        }
        else
        {
            StartCoroutine(BecomeTemporarilyInvincible());
            StartCoroutine(StunPlayer(1f)); // This coroutine will handle the stun effect
        }
    }

    private IEnumerator StunPlayer(float stunDuration)
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false; // Disable player movement to simulate stun
            yield return new WaitForSeconds(stunDuration);
            playerMovement.enabled = true; // Re-enable player movement
        }
    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        Debug.Log("Player is now invincible.");
        isInvincible = true;
        // Here you might want to add a visual effect or animation to indicate invincibility
        yield return new WaitForSeconds(2f); // Duration of invincibility
        isInvincible = false;
        Debug.Log("Player is no longer invincible.");
        // Here you would remove the invincibility visual effect or animation
    }

    public void UpdateHeartUI()
    {
        for (int i = 0; i < fullHearts.Length; i++)
        {
            if (i < currentHealth)
            {
                fullHearts[i].gameObject.SetActive(true);
                emptyHearts[i].gameObject.SetActive(false);
            }
            else
            {
                fullHearts[i].gameObject.SetActive(false);
                emptyHearts[i].gameObject.SetActive(true);
            }
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHeartUI();
    }
}