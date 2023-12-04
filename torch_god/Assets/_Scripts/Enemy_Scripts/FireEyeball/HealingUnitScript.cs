using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingUnitScript : MonoBehaviour, IDamageable
{
    //setting stats here
    public EnemyStats enemyStats { get; protected set; }
    public virtual void SetEnemyStats(EnemyStats stats)
    {
        enemyStats = stats;
    }

    [Header("Stat Properties")]
    public float speed;
    [SerializeField] private float healPercentAmount;
    private float healAmount;
    protected int maxHealth;
    public int currentHealth { get; protected set; }
    //public static Action<int> onEnemyDealDamage;

    [Header("References")]
    //player reference
    public GameObject player;
    private PlayerBaseScript playerScript;
    [SerializeField] protected SpriteRenderer sprite;
    public EnemyAnimations enemyAnimations;


    [Header("Move Properties")]
    protected Vector3 moveDir;

    public static Action<int> onDeath;

    protected virtual void Start()
    {
        if (LevelManager.Instance != null)
        {
            player = LevelManager.Instance.player; 
        }
        playerScript = player.GetComponent<PlayerBaseScript>();
        maxHealth = enemyStats.health;
        currentHealth = maxHealth;
        speed = enemyStats.movementSpeed;

        if (transform.position.x < player.transform.position.x)
        {
            sprite.flipX = true;
            moveDir = transform.right;
        }
        else
        {
            sprite.flipX = false;
            moveDir = -transform.right;
        }
    }

    protected virtual void OnEnable()
    {
        maxHealth = enemyStats.health;
        currentHealth = maxHealth;
        speed = enemyStats.movementSpeed;
    }

    protected virtual void Update()
    {
        transform.position = transform.position + moveDir * speed * Time.deltaTime;
        if (currentHealth <= 0)
        {
            Death();
        }

    }

    protected virtual void Death()
    {
        //play death animation and heal player based on percent of max HP
        healAmount = playerScript.maxHealth * (healPercentAmount/100);
        onDeath?.Invoke((int)healAmount);
        Destroy(gameObject);

    }

    public virtual void TakeDamage(int damageAmount)
    {

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

}
