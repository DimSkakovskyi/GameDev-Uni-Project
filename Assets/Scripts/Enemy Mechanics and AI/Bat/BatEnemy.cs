using UnityEngine;

public class FlyingChasingEnemy : MonoBehaviour
{
    [Header("General")]
    public float speed = 3f;

    [Header("Patrol Area")]
    public Vector2 patrolSize = new Vector2(5f, 3f);

    [Header("Aggro Area")]
    public Vector2 aggroSize = new Vector2(8f, 6f);

    [Header("Target")]
    public Transform player;

    [Header("Shooting")]
    [SerializeField] private GameObject poisonBulletPrefab;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float shootCooldown = 1.5f;

    private float shootTimer = 0f;
    private Vector3 origin;
    private bool isChasing = false;
    private float seedX, seedY;

    void Start()
    {
        origin = transform.position;
        seedX = Random.Range(0f, 100f);
        seedY = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (player == null) return;

        Vector3 toPlayer = player.position - origin;

        isChasing = Mathf.Abs(toPlayer.x) <= aggroSize.x && Mathf.Abs(toPlayer.y) <= aggroSize.y;

        if (isChasing)
        {
            ChasePlayer();
            ShootAtPlayer(); // Стріляє з власної позиції
        }
        else
        {
            Patrol();
            shootTimer = 0f;
        }
    }

    void Patrol()
    {
        float x = (Mathf.PerlinNoise(seedX, Time.time * 0.5f) - 0.5f) * 2;
        float y = (Mathf.PerlinNoise(seedY, Time.time * 0.5f) - 0.5f) * 2;

        Vector3 direction = new Vector3(x, y, 0).normalized;
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, origin.x - patrolSize.x, origin.x + patrolSize.x);
        newPosition.y = Mathf.Clamp(newPosition.y, origin.y - patrolSize.y, origin.y + patrolSize.y);

        transform.position = newPosition;
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, origin.x - aggroSize.x, origin.x + aggroSize.x);
        newPosition.y = Mathf.Clamp(newPosition.y, origin.y - aggroSize.y, origin.y + aggroSize.y);

        transform.position = newPosition;
    }

    void ShootAtPlayer()
{
    shootTimer += Time.deltaTime;

    if (shootTimer >= shootCooldown)
    {
        shootTimer = 0f;

        if (poisonBulletPrefab == null || player == null) return;

        // Створення кулі в позиції ворога
        GameObject bullet = Instantiate(poisonBulletPrefab, transform.position, Quaternion.identity);

        Vector2 direction = (player.position - transform.position).normalized;

        PoisonBullet poison = bullet.GetComponent<PoisonBullet>();
        
        
            poison.SetDirection(direction);
        
    }
}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Application.isPlaying ? origin : transform.position, patrolSize * 2);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Application.isPlaying ? origin : transform.position, aggroSize * 2);
    }
}
