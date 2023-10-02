using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicProjectileScript : MonoBehaviour
{
    [Header("Default Properties")]
    [SerializeField] protected float radius = 0.5f;
    [SerializeField] protected float timeBeforeDestroy = 2;
    public float projectileSpeed = 10;
    public int damageAmount;
    protected Vector2 startPosition;

    protected virtual void Start()
    {
        startPosition = transform.position;
        StartCoroutine(DestroyObject());
    }

    protected virtual void Update()
    {
        Shoot();
        DealDamage();      
    }

    protected virtual void DealDamage()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, transform.position.normalized, 0, 1 << 3);
        if (hit.collider != null)
        {
            //Debug.Log("Hit");
            hit.collider.gameObject.GetComponent<EnemyBaseScript>().TakeDamage(damageAmount);
            Destroy(gameObject);

        }
    }

    protected virtual void Shoot()
    {
        transform.position = transform.position + transform.up * projectileSpeed * Time.deltaTime;
    }

    protected virtual IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(gameObject);
    }

}
