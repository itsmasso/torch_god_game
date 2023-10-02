using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

enum ShadeStates
{
    Chasing,
    Teleporting,
    Knockedbacked,
    Attacking
}
public class Shade_Script : EnemyBaseScript
{
    public delegate void OnTeleport();
    public event OnTeleport onTeleport;

    [SerializeField]
    private float teleportRadiusThickness;

    private float teleportRadiusMin, teleportRadiusMax;

    [SerializeField]
    private float teleportCooldown = 1f;

    [SerializeField]
    private float teleportStopDistance;

    private ShadeStates currentState;
    private bool canTeleport;
    private float distanceBetweenPlayer;
    [SerializeField]
    private int teleportsBeforeFail = 40;
    protected override void Start()
    {
        base.Start();
        canTeleport = true;
        if(player != null)
        {
            UpdateShadeState(ShadeStates.Chasing);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        canTeleport = true;
        if (player != null)
        {
            UpdateShadeState(ShadeStates.Chasing);
        }
    }

    private Vector2 FindRandomPosition()
    {
        //This method finds a random position inside a radius which will generate a random direction.
        //we will also find a random distance to find a random point near the player
        Vector2 targetPosition;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(teleportRadiusMin, teleportRadiusMax);
        targetPosition = (Vector2)player.transform.position + randomDirection * randomDistance;
        return targetPosition;
    }
    protected override void Death()
    {
        //play animation
        base.Death();
        
    }
    private void UpdateShadeState(ShadeStates newState)
    {
        currentState = newState;
        switch (newState)
        {
            case ShadeStates.Chasing:
                HandleChasing();
                break;
            case ShadeStates.Teleporting:
                HandleTeleporting();
                break;
            case ShadeStates.Attacking:
                HandleAttacking();
                break;
            case ShadeStates.Knockedbacked:
                break;
            default:
                break;

        }
    }

    private void HandleChasing()
    {
        StartCoroutine(ChasingPlayer());  
    }

    private IEnumerator ChasingPlayer()
    {
        //this coroutine handles the states and when to switch depending on when shade reaches certain positions
        while(currentState == ShadeStates.Chasing)
        {
            ChasePlayer();
            if (distanceBetweenPlayer <= stopDistance)
            {
                UpdateShadeState(ShadeStates.Attacking);

            }
            else if (distanceBetweenPlayer > teleportStopDistance && canTeleport)
            {
                UpdateShadeState(ShadeStates.Teleporting);
            }
            yield return null;
        }
    }

    private void HandleTeleporting()
    {
        StartCoroutine(TeleportTowardsPlayer());
    }
    private IEnumerator TeleportTowardsPlayer()
    {
        speed = 0;
        //play animation and wait for however long animation takes
        yield return new WaitForSeconds(0.5f);
        speed = enemyStats.movementSpeed;
        transform.position = FindRandomPosition();
        StartCoroutine(TeleportCooldown());
        UpdateShadeState(ShadeStates.Chasing);
        //play animation    

    }
    private IEnumerator TeleportCooldown()
    {
        canTeleport = false;
        yield return new WaitForSeconds(teleportCooldown);
        canTeleport = true;
    }

    private void HandleAttacking()
    {
        StartCoroutine(AttackPlayer());
    }

    private IEnumerator AttackPlayer()
    {
        while (currentState == ShadeStates.Attacking)
        {
            BodyCollisionDamage(enemyStats.attack);
            if (distanceBetweenPlayer > stopDistance)
            {
                UpdateShadeState(ShadeStates.Chasing);
            }    
            yield return null;
        }
    }
    protected override void Update()
    {
        if(player != null)
        {
            distanceBetweenPlayer = Vector2.Distance(player.transform.position, transform.position);

            teleportRadiusMax = Vector2.Distance(player.transform.position, transform.position);
            teleportRadiusMin = teleportRadiusMax - teleportRadiusThickness;
        }

        
        if (currentHealth <= 0)
        {
            Death();
        }


    }
}
