using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Fast_Script : EnemyBaseScript
{
    protected override void Death()
    {
        //play death animation and drop something
        GameObject xpObject = Instantiate(xpPrefab, transform.position, Quaternion.identity);
        xpObject.GetComponent<LightCrystalXP>().xpAmount = xpAmountDropped;
        Destroy(gameObject);
    }
}
