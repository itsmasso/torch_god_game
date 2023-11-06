using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    [Header("Default Properties")]
    [SerializeField] protected float radius = 0.5f;
    [SerializeField] protected float timeBeforeDestroy = 2;
    public float projectileSpeed = 10;
    [System.NonSerialized] public int damageAmount;

    [SerializeField]
    protected Animator anim;

    [SerializeField]
    protected bool hasDestroyAnimation;

    protected bool currentlyBeingDestroyed;
    [SerializeField] protected LayerMask enemyLayer;

    protected virtual void Start()
    {
        currentlyBeingDestroyed = false;
        StartCoroutine(DestroyObject());
    }

    protected virtual void Update()
    {
        
        if (!currentlyBeingDestroyed)
        {
            Shoot();
            DamageHitBox();
        }   
    }

    protected virtual void DamageHitBox()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, transform.position.normalized, 0, enemyLayer);
        if (hit.collider != null)
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(damageAmount);
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

    protected virtual IEnumerator PlayDestroyedAnimation()
    {
        anim.SetBool("IsDestroyed", true);
        currentlyBeingDestroyed = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        Destroy(gameObject);
    }
    protected virtual void Shoot()
    {
        transform.position = transform.position + transform.up * projectileSpeed * Time.deltaTime;
    }

    protected virtual IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        if(anim != null && hasDestroyAnimation)
        {
            StartCoroutine(PlayDestroyedAnimation());
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
