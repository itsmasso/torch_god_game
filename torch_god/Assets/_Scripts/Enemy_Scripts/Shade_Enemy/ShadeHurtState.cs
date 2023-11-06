using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeHurtState : ShadeBaseState
{
    public override void EnterState(Shade_Script shade)
    {
        //flash animation
        shade.enemyAnimations.PlayFlashEffect();
    }

    public override void UpdateState(Shade_Script shade)
    {
        //knockback here
        shade.SwitchShadeState(shade.shadeChaseState);
    }
}
