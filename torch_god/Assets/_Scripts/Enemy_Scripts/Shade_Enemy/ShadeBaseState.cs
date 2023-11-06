using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShadeBaseState 
{
    public abstract void EnterState(Shade_Script shade);

    public abstract void UpdateState(Shade_Script shade);
}
