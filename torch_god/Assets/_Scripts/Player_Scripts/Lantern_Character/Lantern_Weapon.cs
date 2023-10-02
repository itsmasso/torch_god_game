using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern_Weapon : Weapon
{
    [SerializeField]
    private Animator anim;
    protected override void Update()
    {
        if (player.velocity != Vector2.zero)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
        base.Update();
    }
}
