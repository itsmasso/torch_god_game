using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSkullEventTypeScript : MonoBehaviour
{
    public GameObject player;
    private Vector3 direction;
    [SerializeField] private float speed;
    [SerializeField] private float timeBeforeDestroy;
    [SerializeField] private float percentDamage;
    [SerializeField] private CircleCollider2D col;
    [SerializeField] private LayerMask targetLayer;
    private float time;
    private bool canDamage;
    void Start()
    {
        canDamage = true;
        if (LevelManager.Instance != null)
        {
            player = LevelManager.Instance.player;
        }
        direction = (player.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        
    }
    protected virtual void DamageHitBox()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, col.radius, transform.position.normalized, 0, targetLayer);
        if (hit.collider != null)
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage((int)(player.GetComponent<PlayerBaseScript>().maxHealth * (percentDamage/100)));
                canDamage = false;
            }

        }
    }

    void Update()
    {      
        transform.position = transform.position + transform.up * speed * Time.deltaTime;
        if (canDamage)
        {
            DamageHitBox();
        }
        time += Time.deltaTime;
        if(time >= timeBeforeDestroy)
        {
            Destroy(gameObject);
        }
    }
}
