using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    private SimpleEnemyPatrol enemy;
    private float attackCooldown = 1f;
    private float attackTimer = 0f;
    private int damage = 10;

    public void Enter(SimpleEnemyPatrol enemy)
    {
        this.enemy = enemy;
        attackTimer = 0f;
        Debug.Log("Entered Attack State");
    }

    public void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown && enemy.isCollidingWithPlayer)
        {
            PlayerHealth hp = enemy.GetPlayerHealth();
            if (hp != null)
            {
                hp.Damage(damage);
                Debug.Log("Enemy dealt damage to player");
            }

            attackTimer = 0f;
        }

        // Optionally return to idle if player is no longer in contact
        if (!enemy.isCollidingWithPlayer)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Attack State");
    }
}


