using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeChaseState : ShadeBaseState
{
    private RaycastHit2D hit;
    private Vector2 moveDir;
    public override void EnterState(Shade_Script shade)
    {
        
    }

    public override void UpdateState(Shade_Script shade)
    {
        hit = Physics2D.CircleCast(shade.transform.position, 0.5f, shade.transform.position.normalized, 0, 1 << 8);
        if (hit.collider != null)
        {
            shade.SwitchState(shade.enemyAttackState);
        }
        else
        {
            if (shade.useContextSteering && shade.contextSteeringAI.playerTransform != null)
            {
                moveDir = shade.contextSteeringAI.GetDirectionToMove();
            }
            else
            {
                moveDir = (shade.player.transform.position - shade.transform.position).normalized;
            }
            shade.transform.position = (Vector2)shade.transform.position + moveDir * shade.speed * Time.deltaTime;
        }

        if (shade.distanceBetweenPlayer > shade.teleportStopDistance && shade.canTeleport)
        {
            shade.SwitchShadeState(shade.shadeTeleportState);
        }
    }
}
