using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public float despawnSpeed;
    private void Update()
    {
        Invoke("Despawn", despawnSpeed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
    private void Despawn()
    {
        Destroy(gameObject);
    }
}
