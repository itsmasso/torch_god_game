using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(EnemyBaseScript enemy);

    public abstract void UpdateState(EnemyBaseScript enemy);
}
