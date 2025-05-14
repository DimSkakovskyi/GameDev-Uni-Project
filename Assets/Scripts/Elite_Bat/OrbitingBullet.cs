using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingBullet : MonoBehaviour
{
    public int damage = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.Damage(damage);
                Destroy(gameObject); // зникає після завдання шкоди
            }
        }
    }
}

