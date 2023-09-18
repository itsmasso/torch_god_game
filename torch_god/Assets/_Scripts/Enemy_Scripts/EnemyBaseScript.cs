using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

//this will be on all enemy units as default logic
public abstract class EnemyBaseScript : MonoBehaviour
{
    private bool canAttack;
    [SerializeField] private float attackCooldown = 1.5f;
    public EnemyStats enemyStats { get; private set; }
    private int maxHealth;
    public int currentHealth { get; private set; }
    public virtual void SetEnemyStats(EnemyStats stats)
    {
        enemyStats = stats;
    }

    private Vector2 moveDir;

    protected virtual void Start()
    {
        canAttack = true;
        maxHealth = enemyStats.health;
        currentHealth = maxHealth;

    }

    protected virtual void Update()
    {
        Vector3 playerPosition = GameManager.Instance.player.transform.position;
        moveDir = (playerPosition - transform.position).normalized;
        if(Vector2.Distance(playerPosition, transform.position) >= 0.2f)
        {
            transform.position = (Vector2)transform.position + moveDir * enemyStats.movementSpeed * Time.deltaTime;
        }

        if(currentHealth <= 0)
        {
            //play death animation and drop something
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= amount;
        }
    }

    protected virtual IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    protected virtual void BodyCollisionDamage(int amount)
    {
        if (canAttack)
        {
            PlayerBehavior.onTakeDamage?.Invoke(DealDamage(amount));
            StartCoroutine(AttackCooldown());
        }
        
    }
    private int DealDamage(int amount)
    {
        float randomDamageMultiplier = 0.3f; //randomizes damage. A higher multiplier creates a more random damage number and higher range
        int randomDamage = UnityEngine.Random.Range(amount - Mathf.RoundToInt(amount * randomDamageMultiplier), amount + Mathf.RoundToInt(amount * randomDamageMultiplier));
        return randomDamage;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        PlayerBehavior playerScript = collider.gameObject.GetComponent<PlayerBehavior>();
        if(playerScript != null)
        {
            BodyCollisionDamage(enemyStats.attack);
        }
    }

}
