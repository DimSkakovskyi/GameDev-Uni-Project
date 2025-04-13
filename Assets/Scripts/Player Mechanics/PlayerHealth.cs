using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int health = 100;

    public int MAX_HEALTH = 100;

    public HealthBar healthBar;

    private void Start()
    {
        health = MAX_HEALTH;
        healthBar.SetMaxHealth(MAX_HEALTH);
    }

    private void Update()
    {
        //if (Input.GetKeyUp(KeyCode.Q))
        //{
        //    Damage(20);
        //}
    }

    public void Damage(int damage)
    {
        if (damage < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }

        health -= damage;

        healthBar.SetHealth(health);

        if (health == 0)
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
            health = MAX_HEALTH;
            healthBar.SetHealth(MAX_HEALTH);
        }
        else
        {
            health += healing;
            healthBar.SetHealth(health);
        }

    }

    private void Die()
    {
        Debug.Log("I am Dead");
        Destroy(gameObject);
    }
}
