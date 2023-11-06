using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ShadeTeleportState : ShadeBaseState
{

    public override void EnterState(Shade_Script shade)
    {
        shade.speed = 0;
        //play animation and wait for however long animation takes

        shade.speed = shade.enemyStats.movementSpeed;

        //finds a random position inside a radius which will generate a random direction.
        //we will also find a random distance to find a random point near the player
        Vector2 targetPosition;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(shade.teleportRadiusMin, shade.teleportRadiusMax);
        targetPosition = (Vector2)shade.player.transform.position + randomDirection * randomDistance;

        shade.transform.position = targetPosition;
        shade.TriggerTeleportCooldown();      
        shade.SwitchShadeState(shade.shadeChaseState);
        //play animation 
    }



    public override void UpdateState(Shade_Script shade)
    {      

       

    }




}
