using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public float detectionRange = 5f;
    public float knockbackForce = 10f;
    public float knockbackDuration = 1f;
    public float rotationSpeed = 5f;
    public int maxHealth = 10;

    private int currentHealth;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isKnockedBack = false;
    private Vector2 knockbackDirection;
    private float knockbackTimer = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Set the rigidbody to be kinematic initially
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < detectionRange)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            // Rotate towards the player with rotationSpeed
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (isKnockedBack)
        {
            knockbackTimer += Time.deltaTime;
            if (knockbackTimer >= knockbackDuration)
            {
                isKnockedBack = false;
                knockbackTimer = 0f;
                rb.isKinematic = true; // Set the rigidbody back to kinematic
                rb.freezeRotation = false; // Enable rotation
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            // Reduce health and apply knockback force to the enemy
            currentHealth -= 1;

            knockbackDirection = (transform.position - collision.transform.position).normalized;
            rb.isKinematic = false; // Set the rigidbody to be non-kinematic during knockback
            knockbackTimer = 0f;
            rb.velocity = Vector2.zero;
            rb.freezeRotation = true; // Disable rotation
            StartCoroutine(Knockback());

            isKnockedBack = true;
        }

        if (currentHealth <= 0)
        {
            // Enemy is defeated, do something (e.g. play death animation, drop item, etc.)
            Destroy(gameObject);
        }
    }

    IEnumerator Knockback()
    {
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        rb.drag = 0f; // Disable drag to allow knockback movement

        yield return new WaitForSeconds(knockbackDuration);

        rb.velocity = Vector2.zero;
        rb.drag = 5f; // Apply linear drag to reduce sliding
        rb.isKinematic = true; // Set the rigidbody back to kinematic
        rb.freezeRotation = false; // Enable rotation
    }
}
