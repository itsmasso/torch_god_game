using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuminalSpiral : ProjectileBase
{
    protected Vector3 currentDirection;
    private float amountOfBounces;
    [System.NonSerialized] public float maxBounces;
    protected override void Start()
    {
        currentDirection = transform.up;
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if(amountOfBounces >= maxBounces)
        {
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
    protected override void Shoot()
    {
        transform.position = transform.position + currentDirection * projectileSpeed * Time.deltaTime;

    }

    protected override void DamageHitBox()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, transform.position.normalized, 0, targetLayer);
        if (hit.collider != null)
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damageAmount);
                currentDirection = Vector2.Reflect(currentDirection.normalized, hit.normal);
                amountOfBounces++;
            }
            

        }
    }
}
