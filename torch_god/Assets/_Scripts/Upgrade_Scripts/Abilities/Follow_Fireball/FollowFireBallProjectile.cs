using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//script for the second weapon upgrade. fire balls will shoot and follow the first target in its radius
public class FollowFireBallProjectile : ProjectileBase
{
    [SerializeField] protected float followRadius; //radius for how far the fire ball will look for a random target
    [SerializeField] protected AnimationCurve accelerationCurve; //acceleration towards target
    [SerializeField] protected float maxSpeed;
    private float time;
    private bool foundTarget;
    private GameObject target;

    
    protected override void Start()
    {
        base.Start();
        foundTarget = false;
    }


    protected override void Update()
    {
        DamageHitBox();
        DestroyObject();
        if (!foundTarget)
        {
            Shoot();
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, followRadius, transform.position.normalized, 0, 1 << 3);
            if (hit.collider != null)
            {
                target = hit.collider.gameObject;
                foundTarget = true;


            }
        } 
        else if (foundTarget && target != null) 
        {
            //if projectile has not been destroyed yet, then being at the target's position must mean the target has already been destroyed. 
            //we also check if target has been destroyed, because some enemies are either from object pooled or just instantiated and destroyed
            //set found target to false if thats the case in order to look for a new target
            if(target == null || Vector2.Distance(transform.position, target.transform.position) < 0.1f)
            {
                //Debug.Log("finding new target");
                foundTarget = false;
            }
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, accelerationCurve.Evaluate(time) * maxSpeed * Time.deltaTime);
            time += Time.deltaTime;
            
        }
    }

}
