using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Torch_Weapon : Weapon
{
    [Header("Fireball Prefabs")]
    [SerializeField]
    protected GameObject basicFireballPrefab, followFireball;

    [Header("Attack Properties")]
    [SerializeField] protected float shootInterval;
    private Coroutine shootFireBall;

    [Header("FollowShot Properties")]
    [SerializeField] protected float angleOffset;
    [SerializeField] protected float followFireBallShootInterval;
    private Coroutine shootFollowFireBall;
 

    protected override void Start()
    {
        base.Start();

        player.onLevelUp += UpdateAttackStyle;
        UpdateAttackStyle(LevelManager.Instance.playerData.currentLevel);
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
            case 1:
                if(shootFireBall != null)
                    StopCoroutine(shootFireBall);
                shootFireBall = StartCoroutine(ShootFireBall()); 
                break;
            case >= 4:
                if (shootFireBall != null)
                    StopCoroutine(shootFireBall);
                shootFireBall = StartCoroutine(ShootFireBall());

                if (shootFollowFireBall != null)
                    StopCoroutine(shootFollowFireBall);
                shootFollowFireBall = StartCoroutine(ShootFollowFireBall());
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
            BasicAttackFireball projectileScript = projectile.GetComponent<BasicAttackFireball>();
            projectileScript.damageAmount = DealDamage(player.attack);
            yield return new WaitForSeconds(shootInterval / player.attackSpeed);
        }
    }

    private IEnumerator ShootFollowFireBall()
    {
        while (canAttack)
        {
            //we shoot this fire ball in a random direction depending on our clamps
            var direction = Quaternion.Euler(0, 0, Random.Range(-angleOffset, angleOffset));
            GameObject followFireBall = Instantiate(followFireball, firePoint.position, firePoint.rotation * direction);
            BasicAttackFollowFireball followFireBallScript = followFireBall.GetComponent<BasicAttackFollowFireball>();
            followFireBallScript.damageAmount = DealDamage(player.attack);

            yield return new WaitForSeconds(followFireBallShootInterval / player.attackSpeed);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        player.onLevelUp -= UpdateAttackStyle;
    }
}
