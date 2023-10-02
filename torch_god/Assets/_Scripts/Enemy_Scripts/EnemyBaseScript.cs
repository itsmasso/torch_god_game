using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;


//All enemies will inherit this base logic script
public abstract class EnemyBaseScript : MonoBehaviour
{
    //setting stats here
    public EnemyStats enemyStats { get; protected set; }
    public virtual void SetEnemyStats(EnemyStats stats)
    {
        enemyStats = stats;
    }

    [Header("Stat Properties")]
    [SerializeField] protected float speed;
    [SerializeField] protected float attackCooldown = 1.5f;
    public int xpAmountDropped { get; private set; }
    protected int maxHealth;
    public int currentHealth { get; protected set; }

    [Header("References")]
    //player reference
    public GameObject player;
    [SerializeField] protected GameObject xpPrefab;
    [SerializeField] protected SpriteRenderer sprite;

    //its enemy object pool
    public EnemyUnitPool enemyPool;

    [Header("Move Properties")]
    [SerializeField] protected float stopDistance = 1;
    protected Vector2 moveDir;

    [Header("Animation Properties")]
    [SerializeField] protected Material flashMaterial;
    [SerializeField] protected Material originalMaterial;
    [SerializeField] protected float flashDuration;
    private Coroutine flashRoutine;

    //Attack Info
    public static Action<int> onDealDamage;
    protected bool canAttack;

    protected virtual void Start()
    {
        player = LevelManager.Instance.player;
        canAttack = true;
        xpAmountDropped = enemyStats.xpDropped;
        maxHealth = enemyStats.health;
        currentHealth = maxHealth;
        speed = enemyStats.movementSpeed;
        sprite.material = originalMaterial;

    }

    protected virtual void OnEnable()
    {
        canAttack = true;
        maxHealth = enemyStats.health;
        currentHealth = maxHealth;
        sprite.material = originalMaterial;
    }

    protected virtual void Update()
    {
        //making sure class has grabbed reference for player first
        if(player != null)
        {
            //if enemy has reached the player, deal damage, otherwise keep chasing player
            if (Vector2.Distance(player.transform.position, transform.position) >= stopDistance)
            {
                ChasePlayer();
            }
            else
            {

                BodyCollisionDamage(enemyStats.attack);
            }

            if (currentHealth <= 0)
            {
                Death();
            }
        }

    }

    protected virtual void ChasePlayer()
    {
        moveDir = (player.transform.position - transform.position).normalized;
        transform.position = (Vector2)transform.position + moveDir * speed * Time.deltaTime;
    }

    protected virtual void Death()
    {
        //play death animation and drop something
        GameObject xpObject = Instantiate(xpPrefab, transform.position, Quaternion.identity);
        xpObject.GetComponent<LightCrystalXP>().xpAmount = xpAmountDropped;
        enemyPool.pool.Release(gameObject);
    }

    protected IEnumerator FlashEffect()
    {
        //method for damage flash effect by changing materials for a brief second
        sprite.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        sprite.material = originalMaterial;
        flashRoutine = null;
    }

    public void TakeDamage(int amount)
    {
        //knockback and animation
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashEffect());

        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= amount;
        }
    }

    protected virtual void BodyCollisionDamage(int amount)
    {
        if (canAttack)
        {
            //call event to deal damage to player
            onDealDamage?.Invoke(DealDamage(amount));
            StartCoroutine(AttackCoolDown());
        }
   
    }

    protected virtual IEnumerator AttackCoolDown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private int DealDamage(int amount)
    {
        float randomDamageMultiplier = 0.3f; //randomizes damage. A higher multiplier creates a more random damage number and higher range
        int randomDamage = UnityEngine.Random.Range(amount - Mathf.RoundToInt(amount * randomDamageMultiplier), amount + Mathf.RoundToInt(amount * randomDamageMultiplier));
        return randomDamage;
    }


}
