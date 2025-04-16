using UnityEngine;

public class PoisonBullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f;
    public int damage = 10;
    private Vector2 moveDirection;
    public Transform Fonar;

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet touched: " + collision.name);

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Bullet hit PLAYER!");

            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Damage(damage);
                Debug.Log("Damage dealt: " + damage);
            }
        }

        if(gameObject != Fonar)
        Destroy(gameObject);
    }


}