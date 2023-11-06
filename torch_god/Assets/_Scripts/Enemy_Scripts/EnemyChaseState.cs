using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class EnemyChaseState : EnemyBaseState
{
    protected Vector2 moveDir;
    
    public override void EnterState(EnemyBaseScript enemy)
    {
        //entering chase state
    }

    public override void UpdateState(EnemyBaseScript enemy)
    {
        //add an update delay for using context steering to maybe improve performance
        if (Vector2.Distance(enemy.transform.position, enemy.player.transform.position) >= enemy.stopDistance)
        {
            moveDir = enemy.contextSteeringAI.GetDirectionToMove();
            enemy.transform.position = (Vector2)enemy.transform.position + moveDir * enemy.speed * Time.deltaTime;
        }
        else
        {
            enemy.SwitchState(enemy.enemyAttackState);
        }



    }
}
