
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 10;
    public int damageAmount;
    private Vector2 startPosition;

    [SerializeField] 
    private float distanceBeforeDestroy = 10;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position = transform.position + transform.up * projectileSpeed * Time.deltaTime;
        if(Vector2.Distance(startPosition, transform.position) >= distanceBeforeDestroy)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyBaseScript enemyScript = collider.gameObject.GetComponent<EnemyBaseScript>();
        if(enemyScript != null)
        {
            Debug.Log("Hit Enemy");
            enemyScript.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }
}
