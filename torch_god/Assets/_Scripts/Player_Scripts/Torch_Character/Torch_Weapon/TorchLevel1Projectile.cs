
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//turn into object pooling later on for better performance
//Script for fireball that is shot by the weapon. basic attack that passively levels up. NOT an ability/upgrade
public class TorchLevel1Projectile : ProjectileBase
{
    protected RaycastHit2D[] results = new RaycastHit2D[4];
    
    [SerializeField] protected ContactFilter2D contactFilter = new ContactFilter2D();
    [SerializeField] protected float aoeRadius;
    protected override void Start()
    {
        base.Start();
    }
    protected override void DamageHitBox()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, transform.position.normalized, 0, enemyLayer);
        if (hit.collider != null)
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(damageAmount);
            }
            AoeHitBox();
            if (anim != null && hasDestroyAnimation)
            {
                StartCoroutine(PlayDestroyedAnimation());
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }
    protected void AoeHitBox()
    {
        int objectsHit = Physics2D.CircleCast(transform.position, aoeRadius, transform.position.normalized, contactFilter, results, 0);
        if(objectsHit > 0)
        {
            for (int i = 0; i < objectsHit; i++)
            {
                IDamageable damageable = results[i].collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.Damage(damageAmount);
                }


            }
        }


    }

    protected override void Update()
    {
        base.Update();
    }
    
}
