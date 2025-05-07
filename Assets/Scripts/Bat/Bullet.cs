using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 10f;
    public float lifetime = 5f;
    public int damage = 15;

    public Rigidbody2D rb;
    private Vector2 direction;


    private Collider2D ownerCollider;

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

    public void SetOwner(Collider2D owner)
    {
        ownerCollider = owner;
        Collider2D bulletCollider = GetComponent<Collider2D>();
        if (bulletCollider != null && ownerCollider != null)
        {
            Physics2D.IgnoreCollision(bulletCollider, ownerCollider);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        PlayerHealth playerhp = hitInfo.GetComponent<PlayerHealth>();
        if (playerhp != null)
        {
            playerhp.Damage(damage);
        }

        Destroy(gameObject);
    }

}
