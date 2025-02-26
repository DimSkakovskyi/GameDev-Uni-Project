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



    private void Update()
    {

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
