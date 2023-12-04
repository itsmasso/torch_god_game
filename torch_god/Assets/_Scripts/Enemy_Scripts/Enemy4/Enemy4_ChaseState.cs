using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4_ChaseState : Enemy4_BaseState
{
    protected Vector2 moveDir;
    public override void EnterState(Enemy4_Script enemy)
    {
        //entering chase state
    }

    public override void UpdateState(Enemy4_Script enemy)
    {
        //add an update delay for using context steering to maybe improve performance
        if (enemy.useContextSteering && enemy.contextSteeringAI.playerTransform != null)
        {
            moveDir = enemy.contextSteeringAI.GetDirectionToMove();
        }
        else
        {
            moveDir = (enemy.player.transform.position - enemy.transform.position).normalized;
        }
        enemy.transform.position = (Vector2)enemy.transform.position + moveDir * enemy.speed * Time.deltaTime;
        if (Vector2.Distance(enemy.transform.position, enemy.player.transform.position) <= enemy.attackDistance)
        {
            enemy.SwitchEnemy4State(enemy.enemy4AttackState);
        }



    }
}
