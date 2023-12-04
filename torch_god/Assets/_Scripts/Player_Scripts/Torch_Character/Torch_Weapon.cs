using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Torch_Weapon : Weapon
{

    [Header("Fireball Prefabs")]
    [SerializeField]
    protected GameObject basicFireballPrefab;
    protected Coroutine basicFireballRoutine;

    [Header("Attack Properties")]
    [SerializeField] protected float shootInterval;
    

    protected override void Start()
    {
        base.Start();
        PlayerBaseScript.onLevelUp += UpdateAttackStyle;
        UpdateAttackStyle(LevelManager.Instance.saveData.playerData.currentLevel);
    }
    protected override void Update()
    {
        base.Update();
        
    }

    private void UpdateAttackStyle(int currentLevel)
    {
        //We can add our weapon's upgrade through here based on our current level.
        switch (currentLevel)
        {
            case <= 4:
                if(basicFireballRoutine != null)
                    StopCoroutine(basicFireballRoutine);
                basicFireballRoutine = StartCoroutine(ShootFireBall());
                break;
            case >= 5:
                if (basicFireballRoutine != null)
                    StopCoroutine(basicFireballRoutine);
                basicFireballRoutine = StartCoroutine(ShootFireBall());
                break;
            default:
                break;
        }
    }

    private IEnumerator ShootFireBall()
    {
        //change to pooling later
        
        while (canAttack)
        {
            //we shoot a fire ball and how fast we shoot depends on our attack speed which is divided by the interval.
            //a higher attack speed divided into the base interval will make the interval smaller thus making the fire ball shoot faster.
            GameObject projectile = Instantiate(basicFireballPrefab, firePoint.position, firePoint.rotation);
            TorchLevel1Projectile projectileScript = projectile.GetComponent<TorchLevel1Projectile>();
            projectileScript.damageAmount = GenerateDamageAmount(player.attack);
            //AudioManager.Instance.PlaySFX("BasicFireBall");
            yield return new WaitForSeconds(shootInterval / player.attackSpeed);
        }
        
    }



    protected override void OnDestroy()
    {
        PlayerBaseScript.onLevelUp -= UpdateAttackStyle;
        base.OnDestroy();
        
    }
}
