using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int damage = 3;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        SimpleEnemyPatrol enemy = collider.GetComponent<SimpleEnemyPatrol>();
        if (enemy != null)
        {
            enemy.Damage(damage);
        }
    }
}
