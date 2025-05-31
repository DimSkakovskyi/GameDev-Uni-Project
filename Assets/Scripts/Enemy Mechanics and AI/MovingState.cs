using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : IEnemyState
{
    private SimpleEnemyPatrol enemy;

    public void Enter(SimpleEnemyPatrol enemy)
    {
        this.enemy = enemy;
        enemy.Animator.SetBool("isMoving", true);
    }

    public void Update()
    {
        if (enemy.isCollidingWithPlayer)
        {
            enemy.Rigidbody.velocity = Vector2.zero;
            enemy.ChangeState(new AttackState());
            return;
        }

        Vector2 direction = enemy.CurrentPoint.position - enemy.transform.position;
        float moveDir = (enemy.CurrentPoint == enemy.PointB.transform) ? 1f : -1f;
        enemy.Rigidbody.velocity = new Vector2(moveDir * enemy.Speed, 0);

        if (Vector2.Distance(enemy.transform.position, enemy.CurrentPoint.position) < 0.5f)
        {
            enemy.Rigidbody.velocity = Vector2.zero;
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
        enemy.Animator.SetBool("isMoving", false);
    }
}


