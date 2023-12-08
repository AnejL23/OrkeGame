using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float sightRange = 5f;
    public LayerMask playerLayer;
    public float attackRange = 1f;
    public int attackDamage = 1;
    public int damage = 1;

    private Transform player;
    private int waypointIndex = 0;
    private bool isChasing = false; // Initially set to false to start with patrolling

    private Rigidbody2D rb;
    private Vector2 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = rb.position;
        MoveToNextWaypoint();
    }

    private void Update()
    {
        CheckForPlayer(); // Check for the player every frame

        if (isChasing)
        {
            ChasePlayer();
        }
        else if (!isChasing && Vector2.Distance(rb.position, waypoints[waypointIndex].position) < 0.1f)
        {
            MoveToNextWaypoint(); // Move to next waypoint when we reach the current one
        }
    }

    void FixedUpdate()
    {
        if (!isChasing)
        {
            Patrol(); // Call patrol method in FixedUpdate for consistent movement
        }

        // Lock the Y position of the enemy to prevent vertical movement
        rb.position = new Vector2(rb.position.x, startPosition.y);
    }

    void Patrol()
    {
        Vector2 nextPosition = waypoints[waypointIndex].position;
        rb.MovePosition(Vector2.MoveTowards(rb.position, nextPosition, patrolSpeed * Time.fixedDeltaTime));
    }

    void MoveToNextWaypoint()
    {
        waypointIndex = (waypointIndex + 1) % waypoints.Length;
    }

    void ChasePlayer()
    {
        Vector2 newPosition = Vector2.MoveTowards(rb.position, new Vector2(player.position.x, startPosition.y), chaseSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }

    void CheckForPlayer()
    {
        Vector2 directionToPlayer = ((Vector2)player.position - (Vector2)rb.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)rb.position, directionToPlayer, sightRange, ~playerLayer);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }
    }

    public void DefeatEnemy()
    {
        // Add logic to handle the enemy's defeat here
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthManager healthManager = collision.gameObject.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                healthManager.TakeDamage(damage, knockbackDirection);

                StartCoroutine(KnockbackRoutine(1f));
                StartCoroutine(IgnorePlayerRoutine(1.5f));
            }
        }
    }

    private IEnumerator KnockbackRoutine(float duration)
    {
        float knockbackSpeed = 2f; // Adjust as needed
        Vector2 knockbackDirection = (player.position - transform.position).normalized * -1; // Knockback in opposite direction

        float timer = 0;
        while (timer < duration)
        {
            rb.velocity = new Vector2(knockbackDirection.x * knockbackSpeed, rb.velocity.y);
            timer += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero; // Reset velocity
    }

    private IEnumerator IgnorePlayerRoutine(float duration)
    {
        isChasing = false; // Stop chasing

        Vector2 moveDirection = (player.position - transform.position).normalized * -1; // Move in opposite direction
        float ignoreSpeed = 1f; // Adjust as needed

        float timer = 0f;
        while (timer < duration)
        {
            rb.velocity = new Vector2(moveDirection.x * ignoreSpeed, rb.velocity.y);
            timer += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero; // Reset velocity
        isChasing = true; // Resume chasing
    }
}

