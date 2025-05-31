using System.Collections;
using UnityEngine;

public class DeadState : IEnemyState
{
    private SimpleEnemyPatrol enemy;

    public void Enter(SimpleEnemyPatrol enemy)
    {
        this.enemy = enemy;
        enemy.isDead = true;
        enemy.Rigidbody.velocity = Vector2.zero;
        enemy.Animator.SetBool("isDead", true);
        enemy.StartCoroutine(DeathRoutine());
    }

    public void Update() { }
    public void Exit() { }

    private IEnumerator DeathRoutine()
    {
        yield return null;

        while (!enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            yield return null;

        float length = enemy.Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        GameObject.Destroy(enemy.gameObject);
    }
}
