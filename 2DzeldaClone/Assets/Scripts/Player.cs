using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public GameObject attackObject;
    public float attackRotationStart = 0f;
    public float attackRotationEnd = 90f;
    public float attackDuration = 0.2f;

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
}
