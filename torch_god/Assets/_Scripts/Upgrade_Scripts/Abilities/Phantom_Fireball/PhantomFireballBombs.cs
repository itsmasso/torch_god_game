using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomFireballBombs : ProjectileBase
{
    protected RaycastHit2D[] results = new RaycastHit2D[10];

    [SerializeField] protected ContactFilter2D enemyContactFilter = new ContactFilter2D();
    [SerializeField] protected bool showGizmos;
    private bool drawGizmos;

    private float currentTime;
    [SerializeField] private float fieldTime;

    protected override void DamageHitBox()
    {
        int objectsHit = Physics2D.CircleCast(transform.position, radius, transform.position.normalized, enemyContactFilter, results, 0);
        drawGizmos = true;
        if (objectsHit > 0)
        {
            for (int i = 0; i < objectsHit; i++)
            {
                IDamageable damageable = results[i].collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damageAmount);
                }
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


    }

    protected override void Update()
    {
        if(currentTime > fieldTime && !currentlyBeingDestroyed)
        {
            DamageHitBox();
        }
        
        currentTime += Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            if (Application.isPlaying && drawGizmos)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(transform.position, radius);
            }
        }
    }
}