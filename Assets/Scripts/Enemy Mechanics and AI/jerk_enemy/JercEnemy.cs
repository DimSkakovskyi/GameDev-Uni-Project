using System.Collections;
using UnityEngine;

public class JerkEnemy : MonoBehaviour
{
    public float patrolRadius = 5f; // ����� ������������
    public float dashRadius = 3f;   // �����, � ����� ����� ������ �����
    public float dashSpeed = 10f;   // �������� �����
    public float patrolSpeed = 2f;  // �������� ������������
    public float dashCooldown = 2f; // ��� �� �������
    public float dashDistance = 3f; // ������� �����

    private Vector2 startPosition;
    private Rigidbody2D rb;
    private Transform player;
    private bool canDash = true; // �� ����� ������� �����
    private Vector2 patrolTarget;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
        SetNewPatrolTarget();
    }

    private void Update()
    {
        float distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x);

        if (distanceToPlayer <= dashRadius && canDash)
        {
            StartCoroutine(DashTowardsPlayer());
        }
        else if (distanceToPlayer > dashRadius)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (Mathf.Abs(transform.position.x - patrolTarget.x) < 0.5f)
        {
            SetNewPatrolTarget();
        }
        MoveTowards(patrolTarget, patrolSpeed);
    }

    private void SetNewPatrolTarget()
    {
        float randomX = Random.Range(startPosition.x - patrolRadius, startPosition.x + patrolRadius);
        patrolTarget = new Vector2(randomX, transform.position.y);
    }

    private IEnumerator DashTowardsPlayer()
    {
        canDash = false;
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        Vector2 targetPosition = new Vector2(transform.position.x + direction * dashDistance, transform.position.y);

        float elapsedTime = 0f;
        Vector2 startPosition = transform.position;

        while (elapsedTime < dashDistance / dashSpeed)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / (dashDistance / dashSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // ��������� ����� ���������
        rb.velocity = Vector2.zero; // ��������� ���
        yield return new WaitForSeconds(dashCooldown); // ������� ����� ��������� ������
        canDash = true;
    }

    private void MoveTowards(Vector2 target, float speed)
    {
        float direction = Mathf.Sign(target.x - transform.position.x);
        rb.velocity = new Vector2(direction * speed, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(startPosition.x, transform.position.y, 0), patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(startPosition.x, transform.position.y, 0), dashRadius);
    }
}

