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
    public float knockbackSpeed = 5f;

    private int currentHealth;
    private Animator animator;
    public GameObject Poof;
    public GameObject Weapon;
    public float resetSpeed;
    private SpriteRenderer sr;

    private Transform playerTransform;
    private bool isKnockedBack = false;
    private Vector2 knockbackDirection;
    private float knockbackTimer = 0f;


    private void Awake()
    {
        sr= GetComponent<SpriteRenderer>();
        Poof.SetActive(false);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator.Play("Knight_Normal");
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
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Weapon"))
        {
            animator.Play("Knight_Damage");
            // Reduce health and apply knockback force to the enemy
            currentHealth -= 1;

            knockbackDirection = (transform.position - collision.transform.position).normalized;
            StartCoroutine(Knockback());
            Invoke("ResetDamage", 0.3f);
            isKnockedBack = true;
        }

        if (currentHealth <= 0)
        {
            Poof.SetActive(true);
            sr.enabled= false;
            Weapon.SetActive(false);
            Invoke("ResetPoof", resetSpeed);
        }
    }
    private void ResetPoof()
    {
        Destroy(gameObject);
    }
    private void ResetDamage()
    {
        animator.Play("Knight_Normal");
    }

    IEnumerator Knockback()
    {
        Vector2 originalPosition = transform.position;
        Vector2 targetPosition = originalPosition + knockbackDirection * knockbackForce;

        float timer = 0f;
        while (timer < knockbackDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / knockbackDuration;
            transform.position = Vector2.Lerp(originalPosition, targetPosition, progress);
            yield return null;
        }

        // Apply knockback speed to the remaining distance
        float remainingDistance = Vector2.Distance(transform.position, targetPosition);
        float remainingTime = remainingDistance / knockbackSpeed;

        while (remainingTime > 0f)
        {
            float step = knockbackSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
            remainingTime -= Time.deltaTime;
            yield return null;
        }
    }
}
