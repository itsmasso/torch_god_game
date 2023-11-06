using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{   
    private bool canAttack;
    private float lastAttackTime = 0f;
    public override void EnterState(EnemyBaseScript enemy)
    {
        canAttack = true;
    }

    public override void UpdateState(EnemyBaseScript enemy)
    {
        if (canAttack && Vector2.Distance(enemy.transform.position, enemy.player.transform.position) < enemy.stopDistance)
        {
            if(Time.time - lastAttackTime >= enemy.attackCooldown)
            {
                //call event to deal damage to player
                enemy.enemyAnimations.PlayAttackAnimation();
                EnemyBaseScript.onDealDamage?.Invoke(enemy.DealDamage(enemy.enemyStats.attack));
                lastAttackTime = Time.time;
            }

        }
        else
        {
            enemy.SwitchState(enemy.enemyChaseState);
        }
    }

    
}
