using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private SimpleEnemyPatrol enemy;

    public void Enter(SimpleEnemyPatrol enemy)
    {
        this.enemy = enemy;
        enemy.WaitCounter = enemy.WaitTime;
        enemy.Animator.SetBool("isMoving", false); // Optional animation control
    }

    public void Update()
    {
        if (enemy.isCollidingWithPlayer)
        {
            enemy.ChangeState(new AttackState());
            return;
        }

        enemy.WaitCounter -= Time.deltaTime;

        if (enemy.WaitCounter <= 0f)
        {
            enemy.SetNextPatrolPoint();
            enemy.ChangeState(new MovingState());
        }
    }

    public void Exit() { }
}



