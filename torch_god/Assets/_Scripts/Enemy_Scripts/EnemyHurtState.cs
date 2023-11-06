using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtState : EnemyBaseState
{
    public override void EnterState(EnemyBaseScript enemy)
    {
        //flash animation
        enemy.enemyAnimations.PlayFlashEffect();
    }

    public override void UpdateState(EnemyBaseScript enemy)
    {
        //knockback here
        enemy.SwitchState(enemy.enemyChaseState);
    }
}
