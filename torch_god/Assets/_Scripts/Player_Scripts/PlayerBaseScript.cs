using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

//Base script for player class. All playable characters inherit this class
public abstract class PlayerBaseScript : MonoBehaviour
{
    public static event Action<int> onLevelUp;

    [Header("Movement Properties")]
    public Vector2 velocity;
    [SerializeField] protected Rigidbody2D rb;

    [Header("Sprite Properties")]
    [SerializeField] protected SpriteRenderer sprite;
    protected bool facingLeft;

    [Header("Animation Properties")]
    [SerializeField] protected Animator anim;
    [SerializeField] protected Material flashMaterial;
    [SerializeField] protected Material originalMaterial;
    [SerializeField] protected float flashDuration;
    private Coroutine flashRoutine;

    [Header("Player Stats")]
    [SerializeField] protected ScriptablePlayerUnit playerScriptable;
    public ScriptableSaveData playerData;
    [SerializeField] protected float xpNeededDivider; //lower values means more xp required per level
    [SerializeField] protected float xpLevelGapMultiplier; //higher values means larger gaps between levels
    public PlayerStats stats { get; private set; }
    //own struct of stats independent of this class. can modify without changing base stats 
    //add implementation for stat modification here
    protected int maxHealth;
    public int currentHealth { get; private set; }
    public float speed { get; private set; }
    public int attack { get; private set; }
    public float attackSpeed { get; private set; }
    public float critChance { get; private set; }
    public float pickUpRange { get; private set; }

    protected virtual void Awake()
    {
        stats = playerScriptable.baseStats;
        
    }
    protected virtual void Start()
    {
        playerData = ResourceSystem.Instance.saveData;

        //settings stats locally
        UpdateStats();
        currentHealth = maxHealth;

        facingLeft = false;
        sprite.material = originalMaterial;

        //Subscribing to events
        EnemyBaseScript.onDealDamage += TakeDamage;
        LightCrystalXP.onReceiveXP += HandleXP;
        UpgradeManager.onPickedUpgrade += UpdateStats;
    }

    //UpdateStats will update the stats according to recent stat modifications. 
    //Method is called when player picks an upgrade which fires the event.
    protected void UpdateStats()
    {       
        pickUpRange = stats.pickUpRange + playerData.playerData.pickUpRange;
        critChance = stats.critChance + playerData.playerData.critChance;
        attackSpeed = stats.attackSpeed + playerData.playerData.attackSpeed;
        attack = stats.attack + playerData.playerData.attack;
        speed = stats.movementSpeed + playerData.playerData.movementSpeed;
        maxHealth = stats.health + playerData.playerData.health;
        
    }

    //This coroutine will show a damage flash by changing the material for a brief second.
    protected IEnumerator FlashEffect()
    {        
        sprite.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        sprite.material = originalMaterial;
        flashRoutine = null;
    }

    //This if statement ensures that we can restart the flash animation upon getting hit again
    public void TakeDamage(int amount)
    {       
        if(flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashEffect());

        //This if statement handles player death
        if (currentHealth <= 0)
        {
            //player died
            //player death animation
            LevelManager.Instance.UpdateGameState(GameState.PlayerDied);
            currentHealth = 0;
        }
        else
        {
            currentHealth -= amount;
        }
    }

    protected virtual void Update()
    {
        FlipSprite();
    }

    protected virtual void FixedUpdate()
    {
        //Here we can tell the animator whether we are moving or not by checking our user inputs (in the form of velocity variable)
        if (velocity != Vector2.zero)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
        rb.MovePosition(rb.position + velocity * speed * Time.fixedDeltaTime);
    }

    public virtual void OnMove(InputAction.CallbackContext ctx)
    {
        velocity = ctx.ReadValue<Vector2>();

    }

    //This method determines whether the player is moving right or left and will flip the sprite according to direction of player
    protected virtual void FlipSprite()
    {       
        if (velocity.x < 0)
        {
            //face left
            facingLeft = true;
            sprite.flipX = facingLeft;
        }
        else if (velocity.x > 0)
        {
            facingLeft = false;
            sprite.flipX = facingLeft;
        }
        else
        {
            sprite.flipX = facingLeft;
        }
    }

    //This method adds xp gains to the player data and will call a level up method upon reaching an xp threshhold
    protected void HandleXP(int xpGained)
    {
        playerData.playerData.currentExperience += xpGained;
        
        if (playerData.playerData.currentExperience >= playerData.playerData.maxExperience)
        {
            LevelUp();
        }
    } 

    //This method handles leveling up logic and calculates new xp thresh hold for next level
    protected void LevelUp()
    {       
        //maybe add some stat level up modifications
        //maybe make current health = full health
        playerData.playerData.currentLevel++;
        playerData.playerData.currentExperience = 0;
        playerData.playerData.maxExperience = (int)Mathf.Pow((playerData.playerData.currentLevel / xpNeededDivider), xpLevelGapMultiplier);
        AudioManager.Instance.PlaySFX("LevelUp");
        LevelManager.Instance.UpdateGameState(GameState.LevelUp);
        onLevelUp?.Invoke(playerData.playerData.currentLevel);
    }
    protected virtual void OnDestroy()
    {
        //unsubscribe to events
        EnemyBaseScript.onDealDamage -= TakeDamage;
        LightCrystalXP.onReceiveXP -= HandleXP;
        UpgradeManager.onPickedUpgrade -= UpdateStats;

    }
}
