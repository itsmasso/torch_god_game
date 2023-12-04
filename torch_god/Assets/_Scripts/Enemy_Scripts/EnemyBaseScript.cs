using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



//All enemies will inherit this base logic script
public abstract class EnemyBaseScript : MonoBehaviour, IDamageable
{
    //setting stats here
    public EnemyStats enemyStats { get; protected set; }
    public virtual void SetEnemyStats(EnemyStats stats)
    {
        enemyStats = stats;
    }

    [Header("Stat Properties")]
    public float speed;
    public float attackCooldown = 1.5f;
    public int xpAmountDropped { get; protected set; }
    protected int maxHealth;
    public int currentHealth { get; protected set; }
    //public static Action<int> onEnemyDealDamage;

    [Header("References")]
    //player reference
    public GameObject player;
    [SerializeField] protected GameObject xpPrefab;
    [SerializeField] protected SpriteRenderer sprite;
    public EnemyAnimations enemyAnimations;
    public EnemyContextSteeringAI contextSteeringAI;
    public bool useContextSteering = true;

    //its enemy object pool
    public EnemyUnitPool enemyPool;

    [Header("Move Properties")]
    public float stopDistance = 1;
    protected Vector2 moveDir;

    public static Action onDeath;

    //[Header("State Properties")]
    public EnemyBaseState currentState;
    public EnemyChaseState enemyChaseState = new EnemyChaseState();
    public EnemyAttackState enemyAttackState = new EnemyAttackState();
    public EnemyHurtState enemyHurtState = new EnemyHurtState();

    protected virtual void Awake()
    {
        if (LevelManager.Instance != null)
        {
            player = LevelManager.Instance.player;

        }
    }
    protected virtual void Start()
    {
        currentState = enemyChaseState;
        currentState.EnterState(this);
        if(LevelManager.Instance != null)
        {
            LevelManager.Instance.onEndOfFloor += EndofFloorDeath;
        }
        xpAmountDropped = enemyStats.xpDropped;
        maxHealth = enemyStats.health;
        currentHealth = maxHealth;
        speed = enemyStats.movementSpeed;
       
    }

    protected virtual void OnEnable()
    {
        currentState = enemyChaseState;
        currentState.EnterState(this);
        maxHealth = enemyStats.health;
        currentHealth = maxHealth;
        speed = enemyStats.movementSpeed;
    }

    protected virtual void Update()
    {
        currentState.UpdateState(this);
        if (currentHealth <= 0)
        {
            Death();
        }

    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    protected virtual void Death()
    {
        //play death animation and drop something
        GameObject xpObject = Instantiate(xpPrefab, transform.position, Quaternion.identity);
        xpObject.GetComponent<LightCrystalXP>().xpAmount = xpAmountDropped;
        onDeath?.Invoke();
        if(enemyPool != null)
        {
            enemyPool.pool.Release(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    protected virtual void EndofFloorDeath()
    {
        //play death animation
        Destroy(gameObject);
        
    }

    public int GenerateDamageAmount(int amount)
    {
        float randomDamageMultiplier = 0.1f; //randomizes damage. A higher multiplier creates a more random damage number and higher range
        int randomDamage = UnityEngine.Random.Range(amount - Mathf.RoundToInt(amount * randomDamageMultiplier), amount + Mathf.RoundToInt(amount * randomDamageMultiplier));
        return randomDamage;
    }

    public virtual void TakeDamage(int damageAmount)
    {
        SwitchState(enemyHurtState);
        DamagePopup.Create(transform.position, damageAmount, false);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= damageAmount;
        }
    }

    protected void OnDestroy()
    {
        onDeath?.Invoke();
        LevelManager.Instance.onEndOfFloor -= EndofFloorDeath;
    }

}
