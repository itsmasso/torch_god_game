using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4_HurtState : Enemy4_BaseState
{
    public override void EnterState(Enemy4_Script enemy)
    {
        //flash animation
        enemy.enemyAnimations.PlayFlashEffect();
    }

    public override void UpdateState(Enemy4_Script enemy)
    {
        //knockback here
        enemy.SwitchEnemy4State(enemy.enemy4ChaseState);
    }
}
