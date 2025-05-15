using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlyingChasingEnemy : MonoBehaviour
{
    [Header("General")]
    public float speed = 3f;

    [Header("Patrol Area")]
    public Vector2 patrolSize = new Vector2(5f, 3f);

    [Header("Aggro Area")]
    public Vector2 aggroSize = new Vector2(8f, 6f);

    [Header("Target (auto-found)")]
    private Transform player;

    private Vector3 origin;
    public bool isChasing { get; private set; } = false;
    private float seedX, seedY;

    private Rigidbody2D rb;

    [Header("Bat Animation Settings")]

    public Animator animator;

    protected virtual void Start()
    {
        origin = transform.position;
        seedX = Random.Range(0f, 100f);
        seedY = Random.Range(0f, 100f);

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            player = playerGO.transform;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (player == null) return;

        Vector3 toPlayer = player.position - origin;
        isChasing = Mathf.Abs(toPlayer.x) <= aggroSize.x && Mathf.Abs(toPlayer.y) <= aggroSize.y;

        if (isChasing)
        {
            float currentSpeed = rb.velocity.magnitude;
            if (animator == null)
                animator = GetComponent<Animator>();
            animator.SetFloat("speed", currentSpeed);
            MoveTo(ChaseTarget());
        }   
        else
            MoveTo(PatrolTarget());
    }

    Vector3 ChaseTarget()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        Vector3 target = transform.position + dir * speed * Time.fixedDeltaTime;

        target.x = Mathf.Clamp(target.x, origin.x - aggroSize.x, origin.x + aggroSize.x);
        target.y = Mathf.Clamp(target.y, origin.y - aggroSize.y, origin.y + aggroSize.y);

        return target;
    }

    Vector3 PatrolTarget()
    {
        float x = (Mathf.PerlinNoise(seedX, Time.time * 0.5f) - 0.5f) * 2;
        float y = (Mathf.PerlinNoise(seedY, Time.time * 0.5f) - 0.5f) * 2;

        Vector3 dir = new Vector3(x, y, 0).normalized;
        Vector3 target = transform.position + dir * speed * Time.fixedDeltaTime;

        target.x = Mathf.Clamp(target.x, origin.x - patrolSize.x, origin.x + patrolSize.x);
        target.y = Mathf.Clamp(target.y, origin.y - patrolSize.y, origin.y + patrolSize.y);

        return target;
    }

   void MoveTo(Vector3 target)
{
    Vector2 direction = (target - transform.position).normalized;
    float distance = Vector2.Distance(transform.position, target);

    // Основна перевірка на зіткнення
    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, LayerMask.GetMask("Obstacles"));

    if (hit.collider == null)
    {
        rb.MovePosition(transform.position + (Vector3)(direction * speed * Time.deltaTime));
    }
    else
    {
        // ❗ Перешкода! Пробуємо обхід:
        Vector2[] alternateDirs = new Vector2[]
        {
            new Vector2(-direction.y, direction.x),  // Вліво відносно напрямку
            new Vector2(direction.y, -direction.x),  // Вправо
            -direction                                // Назад (останній варіант)
        };

        foreach (Vector2 altDir in alternateDirs)
        {
            RaycastHit2D altHit = Physics2D.Raycast(transform.position, altDir, speed * Time.deltaTime * 1.5f, LayerMask.GetMask("Obstacles"));
            if (altHit.collider == null)
            {
                rb.MovePosition(transform.position + (Vector3)(altDir.normalized * speed * Time.deltaTime));
                return;
            }
        }

        // Якщо всі варіанти зайняті — стоїмо на місці
    }
}

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Application.isPlaying ? origin : transform.position, patrolSize * 2);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Application.isPlaying ? origin : transform.position, aggroSize * 2);
    }
}


