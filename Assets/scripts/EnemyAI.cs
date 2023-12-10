using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float activationRange = 5f;
    public float chaseSpeed = 2f;
    public float attackCooldown = 2f;
    public int damage = 1;
    public Vector2 knockbackForce = new Vector2(2f, 2f);

    private GameObject player;
    private Rigidbody2D rb;
    private float lastAttackTime = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= activationRange)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * chaseSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            if (Time.time - lastAttackTime > attackCooldown)
            {
                lastAttackTime = Time.time;
                AttackPlayer();
            }
        }
    }

    void AttackPlayer()
    {
        HealthManager healthManager = player.GetComponent<HealthManager>();
        if (healthManager != null)
        {
            Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
            healthManager.TakeDamage(damage, knockbackDirection * knockbackForce);
        }
    }

    public void DefeatEnemy()
    {
        Debug.Log("Enemy defeated!");
        Destroy(gameObject);
    }

}
