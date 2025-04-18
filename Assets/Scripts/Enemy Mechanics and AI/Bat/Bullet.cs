using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 10f;
    public float lifetime = 5f;

    public Rigidbody2D rb;
    private Vector2 direction;

    void Start()
    {
        // Автоматично знищити кулю через N секунд
        Destroy(gameObject, lifetime);
    }

    // Встановлення напряму
    public void SetDirection(Vector2 targetPosition)
    {
        direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void OntriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        Destroy(gameObject); 
    }

}
