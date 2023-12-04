using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Brute_Script : EnemyBaseScript
{
    protected override void Start()
    {
        currentState = enemyChaseState;
        base.Start();
    }


    protected override void Update()
    {
        base.Update();
    }

    protected override void Death()
    {
        //play death animation and drop something
        GameObject xpObject = Instantiate(xpPrefab, transform.position, Quaternion.identity);
        xpObject.GetComponent<LightCrystalXP>().xpAmount = xpAmountDropped;
        Destroy(gameObject);
    }
}
