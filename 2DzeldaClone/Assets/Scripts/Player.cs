using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public GameObject attackObject;
    public float attackRotationStart = 0f;
    public float attackRotationEnd = 90f;
    public float attackDuration = 0.2f;
    public float health = 6;
    public bool Damaged =false;
    public float resetDamage;
    public Image healthImg;
    public Sprite[] healthSprite;

    private Rigidbody2D rb;
    private Quaternion attackRotation;
    private bool isAttacking;

    void Start()
    {
        attackObject.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        attackRotation = Quaternion.Euler(0, 0, attackRotationStart);
    }

    void Update()
    {
        if (health == 6f)
        {
            healthImg.sprite = healthSprite[0];
        }
        if (health == 5f)
        {
            healthImg.sprite = healthSprite[1];
        }
        if (health == 4f)
        {
            healthImg.sprite = healthSprite[2];
        }
        if (health == 3f)
        {
            healthImg.sprite = healthSprite[3];
        }
        if (health == 2f)
        {
            healthImg.sprite = healthSprite[4];
        }
        if (health == 1f)
        {
            healthImg.sprite = healthSprite[5];
        }
        if (health == 0f)
        {
            healthImg.sprite = healthSprite[6];
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);

        rb.velocity = movement * speed;

        if (movement.magnitude > 0)
        {
            transform.up = movement;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackCoroutine());
        }
    }

    IEnumerator AttackCoroutine()
    {
        // Activate the attack object
        attackObject.SetActive(true);

        float timeElapsed = 0f;

        while (timeElapsed < attackDuration)
        {
            // Lerp the rotation of the attack object
            float t = timeElapsed / attackDuration;
            attackObject.transform.localRotation = Quaternion.Lerp(attackRotation, Quaternion.Euler(0, 0, attackRotationEnd), t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Deactivate the attack object
        attackObject.SetActive(false);
        attackObject.transform.localRotation = Quaternion.Euler(0, 0, attackRotationStart);
        isAttacking = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage") && Damaged == false)
        {
            health--;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            Damaged = true;
            Invoke("ResetDamage", resetDamage);
        }
    }

    private void ResetDamage()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        Damaged = false;
    }
}
