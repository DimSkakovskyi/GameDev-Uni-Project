using System.Collections;
using UnityEngine;

public class SimpleEnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    public GameObject PointA;
    public GameObject PointB;

    [Header("Movement Settings")]
    public float speed = 2f;
    public float waitTime = 2f;
    public float hurtPauseTime = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform currentPoint;

    private bool isWaiting = false;
    private float waitCounter = 0f;
    private bool isDead = false;

    private enum State { Idle, Moving, Hurt, Dead }
    private State currentState = State.Moving;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = PointA.transform;
    }

    void Update()
    {
        if (isDead) return;

        switch (currentState)
        {
            case State.Moving:
                MoveToPoint();
                break;
            case State.Idle:
                WaitAtPoint();
                break;
            case State.Hurt:
                rb.velocity = Vector2.zero;
                break;
        }

        UpdateAnimator();
    }

    private void MoveToPoint()
    {
        if (isWaiting) return;

        float moveDir = (currentPoint == PointB.transform) ? 1f : -1f;
        rb.velocity = new Vector2(moveDir * speed, 0);

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            currentState = State.Idle;
            isWaiting = true;
            waitCounter = waitTime;
            rb.velocity = Vector2.zero;
        }
    }

    private void WaitAtPoint()
    {
        waitCounter -= Time.deltaTime;
        if (waitCounter <= 0f)
        {
            isWaiting = false;
            Flip();
            currentPoint = (currentPoint == PointA.transform) ? PointB.transform : PointA.transform;
            currentState = State.Moving;
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    /// <summary>
    /// Call this when the enemy is hurt
    /// </summary>
    public void OnHurt()
    {
        if (isDead) return;

        Debug.Log("Enemy got hurt!");
        StopAllCoroutines();
        StartCoroutine(HurtRoutine());
    }

    private IEnumerator HurtRoutine()
    {
        currentState = State.Hurt;
        animator.SetTrigger("Hurt");

        yield return new WaitForSeconds(hurtPauseTime);

        if (!isDead)
        {
            currentState = isWaiting ? State.Idle : State.Moving;
        }
    }

    /// <summary>
    /// Call this when the enemy dies
    /// </summary>
    public void OnDeath()
    {
        if (isDead) return;

        Debug.Log("Enemy is dying...");
        isDead = true;
        currentState = State.Dead;
        rb.velocity = Vector2.zero;

        animator.SetBool("isDead", true);

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        // Wait until transition starts
        yield return null;

        // Wait until animator is playing "Death"
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            yield return null;
        }

        float deathLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log("Playing Death for " + deathLength + " seconds");

        // Wait for full length of death animation
        yield return new WaitForSeconds(deathLength);

        Destroy(gameObject);
    }


    private void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetBool("isMoving", currentState == State.Moving);
    }

    private void OnDrawGizmos()
    {
        if (PointA != null && PointB != null)
        {
            Gizmos.DrawWireSphere(PointA.transform.position, 0.5f);
            Gizmos.DrawWireSphere(PointB.transform.position, 0.5f);
            Gizmos.DrawLine(PointA.transform.position, PointB.transform.position);
        }
    }
}
