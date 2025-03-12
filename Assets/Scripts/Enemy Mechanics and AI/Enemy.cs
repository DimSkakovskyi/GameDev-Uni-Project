using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //LootTable
    [Header("Loot")]
    public List<LootItem> LootTable = new List<LootItem>();

    [SerializeField] private int health = 9;

    private int MAX_HEALTH = 9;
    public bool isCollidingWithPlayer = false;
    private bool attacking = false;
    private int damage = 100;

    private float timeToAttack = 0.25f;
    private float timer = 0f;


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

    // Detect when player enters the trigger zone
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure collider belongs to the player
        {
            isCollidingWithPlayer = true;
            attacking = true;
            if (other.GetComponent<Enemy>() != null && (attacking == true))
            {
                PlayerHealth health = other.GetComponent<PlayerHealth>();
                health.Damage(damage);
            }
        }
    }

    // Detect when player exits the trigger zone
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
        //Go around LootTable
        //Spawn items
        foreach (LootItem item in LootTable)
        {
            InitiateLoot(item.itemPrefab);
        }
        Debug.Log("I am Dead");
        Destroy(gameObject);
    }

    void InitiateLoot(GameObject loot)
    {
        if (loot)
        {
            GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);

            droppedLoot.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }
}
