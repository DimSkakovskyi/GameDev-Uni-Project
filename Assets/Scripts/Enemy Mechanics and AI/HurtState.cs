using System.Collections;
using UnityEngine;

public class HurtState : IEnemyState
{
    private SimpleEnemyPatrol enemy;

    public void Enter(SimpleEnemyPatrol enemy)
    {
        this.enemy = enemy;
        enemy.StartCoroutine(HurtRoutine());
    }

    private IEnumerator HurtRoutine()
    {
        enemy.Animator.SetTrigger("Hurt");
        enemy.Rigidbody.velocity = Vector2.zero;

        yield return new WaitForSeconds(enemy.hurtPauseTime);

        if (!enemy.IsDead)
        {
            enemy.ChangeState(enemy.IsWaiting ? new IdleState() : new MovingState());
        }
    }

    public void Update() { }
    public void Exit() { }
}
