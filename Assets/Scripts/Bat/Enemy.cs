using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //LootTable
    [Header("Loot")]
    public List<LootItem> LootTable = new List<LootItem>();

    [SerializeField] private int health = 9;

    private Dictionary<string, ItemFactoryBase> LootFactories = new Dictionary<string, ItemFactoryBase>();

    private int MAX_HEALTH = 9;
    public bool isCollidingWithPlayer = false;
    private bool attacking = false;
    private int damage = 20;

    private float timeToAttack = 0.25f;
    private float timer = 0f;

    private void Awake()
    {
        LootFactories.Add("healing", new HealingPotionFactory());
        // LootFactories.Add("coin", new CoinFactory());
    }


    private void Update()
    {
        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
            }
        }
    }

    // Detect when player enters colliding zone
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Ensure it's the player
        {
            isCollidingWithPlayer = true;
            attacking = true;

            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null && attacking)
            {
                Debug.Log("Player hit by enemy!");
                playerHealth.Damage(damage);
            }
        }
    }

    // Detect when player exits colliding zone
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
    }


    public void Damage(int damage)
    {
        if (damage < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }

        this.health -= damage;

        if (this.health < 0)
        {
            Die();
        }
    }

    public void Healing(int healing)
    {
        if (healing < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Healing");
        }

        if (health + healing > MAX_HEALTH)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += healing;
        }

    }

    private void Die()
    {
        Debug.Log("I am Dead");
        Destroy(gameObject);
    }
}
