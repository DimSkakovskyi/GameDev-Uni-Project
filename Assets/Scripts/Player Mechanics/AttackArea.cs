using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int damage = 3;

    private void OnTriggerEnter2D(Collider2D collider)
    {
       Enemy enemy = collider.GetComponent<Enemy>();

        if (enemy != null)
        {
            Debug.Log("AttackArea triggered with " + collider.name);
            enemy.Damage(damage);
        }
    }
}
