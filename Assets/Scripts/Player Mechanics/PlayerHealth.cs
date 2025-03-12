using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health = 100;

    private int MAX_HEALTH = 100;

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
        Debug.Log("I am Dead");
        Destroy(gameObject);
    }
}
