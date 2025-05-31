using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void Enter(SimpleEnemyPatrol enemy);
    void Update();
    void Exit();
}

