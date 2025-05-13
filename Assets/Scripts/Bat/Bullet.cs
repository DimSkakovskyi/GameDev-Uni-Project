using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The class is responsible for the behavior of the ball in the game.
/// It moves in a given direction, has a speed, lifetime, and deals damage on impact.
/// </summary>
public class Bullet : MonoBehaviour
{
    /// <summary>
    /// The speed of bullet
    /// </summary>
    public float speed = 10f;

    /// <summary>
    /// time of existence of the bullet
    /// </summary>
    public float lifetime = 5f;

    /// <summary>
    /// damage our bullet
    /// </summary>
    public int damage = 15;

    /// <summary>
    /// Rigidbody2D to control the physics of a ball.
    /// </summary>
    public Rigidbody2D rb;

    /// <summary>
    /// The direction in which the ball is moving.
    /// </summary>
    private Vector2 direction;

    /// <summary>
    /// A collider of an object that has released a bullet to avoid collision with it.
    /// </summary>
    private Collider2D ownerCollider;

    /// <summary>
    ///  Called when the object is launched. Destroys the ball after the specified lifetime.
    /// </summary>
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    
    /// <summary>
    /// Sets the direction of the bullet to the target position.
    /// </summary>
    /// <param name=“targetPosition”>The position to which the bullet should go.
    public void SetDirection(Vector2 targetPosition)
    {
        direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;
    }

     /// <summary>
    /// Sets the balloon owner to avoid collisions with it.
    /// </summary>
    /// <param name=“owner”>Collider of the owner object.</param
    public void SetOwner(Collider2D owner)
    {
        ownerCollider = owner;
        Collider2D bulletCollider = GetComponent<Collider2D>();
        if (bulletCollider != null && ownerCollider != null)
        {
            Physics2D.IgnoreCollision(bulletCollider, ownerCollider);
        }
    }

    /// <summary>
    /// Handles collisions between the ball and other objects. If it is a player, it deals damage.
    /// </summary>
    /// <param name=“hitInfo”>Information about the hit.</param
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
