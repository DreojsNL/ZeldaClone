using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public float detectionRange = 5f;
    public float knockbackForce = 10f;
    public int maxHealth = 10;

    private int currentHealth;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < detectionRange)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            // Rotate towards the player
            transform.up = direction;
        }

        if (isKnockedBack)
        {
            knockbackTimer += Time.deltaTime;
            if (knockbackTimer >= 1f)
            {
                isKnockedBack = false;
                rb.velocity = Vector2.zero;
                knockbackTimer = 0f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            // Reduce health and apply knockback force to the enemy
            currentHealth -= 1;

            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            isKnockedBack = true;
        }

        if (currentHealth <= 0)
        {
            // Enemy is defeated, do something (e.g. play death animation, drop item, etc.)
            Destroy(gameObject);
        }
    }
}
