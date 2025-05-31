using System.Collections;
using System.Collections.Generic;
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
    public Animator Animator;
    private Transform currentPoint;

    private bool isWaiting = false;
    private float waitCounter = 0f;
    public bool isDead = false;

    public bool isCollidingWithPlayer = false;

    private IEnemyState currentState;

    [Header("Loot")]
    public List<LootItem> LootTable = new List<LootItem>();

    [SerializeField] private int health = 9;
    private int MAX_HEALTH = 9;
    public bool IsAlive => health > 0;

    private Dictionary<string, ItemFactoryBase> LootFactories = new Dictionary<string, ItemFactoryBase>();


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        currentPoint = PointA.transform;

        ChangeState(new MovingState());
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        LootFactories.Add("healing", new HealingPotionFactory());
    }

    private void Update()
    {
        if (!isDead)
            currentState?.Update();
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter(this);
    }

    public void SetNextPatrolPoint()
    {
        Flip();
        currentPoint = (currentPoint == PointA.transform) ? PointB.transform : PointA.transform;
    }
    public void Damage(int damage)
    {
        if (damage < 0)
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");

        health -= damage;

        if (health < 0)
            Die();
    }

    public void Healing(int healing)
    {
        if (healing < 0)
            throw new System.ArgumentOutOfRangeException("Cannot have negative Healing");

        health = Mathf.Min(health + healing, MAX_HEALTH);
    }
    public PlayerHealth GetPlayerHealth()
    {
        GameObject player = GameObject.FindWithTag("Player");
        return player?.GetComponent<PlayerHealth>();
    }


    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void OnHurt()
    {
        if (isDead) return;

        Debug.Log("Enemy got hurt!");
        StopAllCoroutines();
        StartCoroutine(HurtRoutine());
    }

    private IEnumerator HurtRoutine()
    {
        ChangeState(new HurtState());
        Animator.SetTrigger("Hurt");

        yield return new WaitForSeconds(hurtPauseTime);

        if (!isDead)
        {
            ChangeState(isWaiting ? new IdleState() : new MovingState());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
    }


    public void OnDeath()
    {
        if (isDead) return;

        Debug.Log("Enemy is dying...");
        isDead = true;
        rb.velocity = Vector2.zero;

        Animator.SetBool("isDead", true);
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return null;

        while (!Animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            yield return null;
        }

        float deathLength = Animator.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log("Playing Death for " + deathLength + " seconds");

        yield return new WaitForSeconds(deathLength);
        Destroy(gameObject);
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
    private void Die()
    {
        if (LootFactories.Count > 0)
        {
            List<string> keys = new List<string>(LootFactories.Keys);
            string randomKey = keys[Random.Range(0, keys.Count)];

            ItemFactoryBase factory = LootFactories[randomKey];

            if (factory != null)
            {
                GameObject loot = factory.SpawnItem();
                loot.transform.position = transform.position;
                Debug.Log($"Dropped loot: {loot.name}");
            }
        }

        Debug.Log("I am Dead");
        Destroy(gameObject);
    }


    // Public getters for state classes to access necessary data
    public Rigidbody2D Rigidbody => rb;
    public Transform CurrentPoint => currentPoint;
    public Transform PointATransform => PointA.transform;
    public Transform PointBTransform => PointB.transform;
    public float Speed => speed;
    public float WaitTime => waitTime;
    public bool IsDead => isDead;
    public bool IsWaiting { get => isWaiting; set => isWaiting = value; }
    public float WaitCounter { get => waitCounter; set => waitCounter = value; }
}
