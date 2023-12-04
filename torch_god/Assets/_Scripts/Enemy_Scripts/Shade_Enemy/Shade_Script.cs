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
    [System.NonSerialized] public float teleportRadiusMin;
    public float teleportRadiusMax;

    public float teleportRadiusThickness;

    public float teleportCooldown = 1f;

    public float teleportStopDistance;

    public ShadeBaseState currentShadeState;
    public ShadeChaseState shadeChaseState = new ShadeChaseState();
    public ShadeAttackState shadeAttackState = new ShadeAttackState();
    public ShadeHurtState shadeHurtState = new ShadeHurtState();
    public ShadeTeleportState shadeTeleportState = new ShadeTeleportState();

    public bool canTeleport;
    public float distanceBetweenPlayer;
    [SerializeField]
    private int teleportsBeforeFail = 40;
    protected override void Start()
    {
        canTeleport = true;
        currentState = enemyChaseState;
        currentState.EnterState(this);
        player = LevelManager.Instance.player;
        xpAmountDropped = enemyStats.xpDropped;
        maxHealth = enemyStats.health;
        currentHealth = maxHealth;
        speed = enemyStats.movementSpeed;

    }

    protected override void OnEnable()
    {
        canTeleport = true;
        currentShadeState = shadeChaseState;
        currentShadeState.EnterState(this);

        maxHealth = enemyStats.health;
        currentHealth = maxHealth;
        speed = enemyStats.movementSpeed;

    }

    protected override void Death()
    {
        //play animation
        base.Death();
        
    }

    public void SwitchShadeState(ShadeBaseState state)
    {
        currentShadeState = state;
        state.EnterState(this);
    }

    public void TriggerTeleportCooldown()
    {
        StartCoroutine(TeleportCooldown());
    }

    private IEnumerator TeleportCooldown()
    {
        canTeleport = false;
        yield return new WaitForSeconds(teleportCooldown);
        canTeleport = true;
    }

    protected override void Update()
    {
        currentShadeState.UpdateState(this);
        if (player != null)
        {
            distanceBetweenPlayer = Vector2.Distance(player.transform.position, transform.position);
            teleportRadiusMax = Vector2.Distance(player.transform.position, transform.position);
        }

        teleportRadiusMin = teleportRadiusMax - teleportRadiusThickness;

        if (currentHealth <= 0)
        {
            Death();
        }

    }
}
