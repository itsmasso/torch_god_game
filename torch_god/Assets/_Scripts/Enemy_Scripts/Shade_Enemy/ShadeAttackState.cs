using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShadeAttackState : ShadeBaseState
{
    public static Action<int> onDealDamage;
    private bool canAttack;
    private float lastAttackTime = 0f;
    public override void EnterState(Shade_Script shade)
    {
        canAttack = true;
    }

    public override void UpdateState(Shade_Script shade)
    {
        if (canAttack && Vector2.Distance(shade.transform.position, shade.player.transform.position) < shade.stopDistance)
        {
            if (Time.time - lastAttackTime >= shade.attackCooldown)
            {
                //call event to deal damage to player
                shade.enemyAnimations.PlayAttackAnimation();
                onDealDamage?.Invoke(shade.GenerateDamageAmount(shade.enemyStats.attack));
                lastAttackTime = Time.time;
            }

        }
        else
        {
            shade.SwitchState(shade.enemyChaseState);
        }
    }

}
