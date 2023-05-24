using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject arrowPrefab;  // Prefab for the arrow object
    public Transform[] arrowSpawnPoints;  // Array of spawn points for the arrows
    public float arrowSpeed = 10f;  // Speed of the arrow
    public bool throwingKnife;
    public bool knifeBomb;
    public float knifeCooldown = 1f;  // Cooldown time for the throwing knife
    private bool canThrowKnife = true;  // Flag to track if the knife can be thrown

    void Update()
    {
        if (throwingKnife)
        {
            ThrowingKnife();
        }
        if (knifeBomb)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (canThrowKnife)
                {
                    KnifeBomb();
                }
            }
        }
    }

    private void KnifeBomb()
    {
        if (canThrowKnife)
        {
            foreach (Transform spawnPoint in arrowSpawnPoints)
            {
                // Instantiate the arrow prefab
                GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, spawnPoint.rotation);

                // Get the Rigidbody2D component of the arrow
                Rigidbody2D arrowRigidbody = arrow.GetComponent<Rigidbody2D>();

                // Set the arrow's velocity to shoot it forward
                arrowRigidbody.velocity = spawnPoint.up * arrowSpeed;
            }

            StartCoroutine(KnifeCooldownCoroutine());
        }
    }

    private void ThrowingKnife()
    {
        if (canThrowKnife)
        {
            foreach (Transform spawnPoint in arrowSpawnPoints)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    // Instantiate the arrow prefab
                    GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, spawnPoint.rotation);

                    // Get the Rigidbody2D component of the arrow
                    Rigidbody2D arrowRigidbody = arrow.GetComponent<Rigidbody2D>();

                    // Set the arrow's velocity to shoot it forward
                    arrowRigidbody.velocity = spawnPoint.up * arrowSpeed;

                    StartCoroutine(KnifeCooldownCoroutine());
                }
            }
        }
    }

    private IEnumerator KnifeCooldownCoroutine()
    {
        canThrowKnife = false;

        // Wait for the cooldown duration
        yield return new WaitForSeconds(knifeCooldown);

        canThrowKnife = true;
    }
}
