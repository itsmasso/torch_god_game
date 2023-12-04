using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4_Script : EnemyBaseScript
{
    //[Header("State Properties")]
    public Enemy4_BaseState enemy4currentState;
    public Enemy4_ChaseState enemy4ChaseState = new Enemy4_ChaseState();
    public Enemy4_AttackState enemy4AttackState = new Enemy4_AttackState();
    public Enemy4_HurtState enemy4HurtState = new Enemy4_HurtState();

    [Header("Projectile Properties")]
    public GameObject projectilePrefab;
    public float attackDistance = 8f;
    protected Rigidbody2D playerRb;
    protected float projectileSpeed;
    protected override void Start()
    {
        
        projectileSpeed = projectilePrefab.GetComponent<ProjectileBase>().projectileSpeed;
        enemy4currentState = enemy4ChaseState;
        enemy4currentState.EnterState(this);
        if (LevelManager.Instance != null)
        {
            player = LevelManager.Instance.player;
        }
        playerRb = player.GetComponent<Rigidbody2D>();
        xpAmountDropped = enemyStats.xpDropped;
        maxHealth = enemyStats.health;
        currentHealth = maxHealth;
        speed = enemyStats.movementSpeed;
    }

    protected override void OnEnable()
    {
        enemy4currentState = enemy4ChaseState;
        enemy4currentState.EnterState(this);
        maxHealth = enemyStats.health;
        currentHealth = maxHealth;
        speed = enemyStats.movementSpeed;
    }


    protected override void Update()
    {        
        enemy4currentState.UpdateState(this);
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    //add predictive movement 
    public void ShootProjectile()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;      
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        GameObject projectile = Instantiate(projectilePrefab, (Vector2)transform.position, rotation);
        Enemy4_ProjectileScript projectileScript = projectile.GetComponent<Enemy4_ProjectileScript>();
        projectileScript.damageAmount = GenerateDamageAmount(enemyStats.attack);
    }

    public void SwitchEnemy4State(Enemy4_BaseState state)
    {
        enemy4currentState = state;
        state.EnterState(this);
    }

    public override void TakeDamage(int damageAmount)
    {
        SwitchEnemy4State(enemy4HurtState);
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
