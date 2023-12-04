using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4_AttackState : Enemy4_BaseState
{
    protected Vector2 moveDir;
    private bool canAttack;
    private float lastAttackTime = 0f;
    public override void EnterState(Enemy4_Script enemy)
    {
        canAttack = true;
    }

    //add predictive movement later
    public override void UpdateState(Enemy4_Script enemy)
    {
        if (canAttack && Time.time - lastAttackTime >= enemy.attackCooldown)
        {
            //call event to deal damage to player
            enemy.enemyAnimations.PlayAttackAnimation();
            enemy.ShootProjectile();
            lastAttackTime = Time.time;
        }
        if (Vector2.Distance(enemy.transform.position, enemy.player.transform.position) >= enemy.stopDistance)
        {
            //Debug.Log("moving");
            moveDir = enemy.contextSteeringAI.GetDirectionToMove();
            enemy.transform.position = (Vector2)enemy.transform.position + moveDir * enemy.speed * Time.deltaTime;

        }

        if (Vector2.Distance(enemy.transform.position, enemy.player.transform.position) > enemy.attackDistance)
        {
            enemy.SwitchEnemy4State(enemy.enemy4ChaseState);
        }
    }
}
