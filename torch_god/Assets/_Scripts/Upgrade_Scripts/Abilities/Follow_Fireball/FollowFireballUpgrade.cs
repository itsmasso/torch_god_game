using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FollowFireBall : AbilityUpgradeBase, IUpgradeable
{
    [Header("FollowShot Properties")]
    [SerializeField] private GameObject followFireball;
    [SerializeField] private float angleOffset;
    [SerializeField] private float followFireBallShootInterval;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(ShootFollowFireBall());
    }

    public void LevelUpUpgrade()
    {       
        currentLevel++;
        upgradeScriptable.level = currentLevel;
        Debug.Log("Follow Fireball Level: " + currentLevel);
        //maybe add cool upgrades besides damage later
        damageAmount = upgradeScriptable.baseDamage + (currentLevel * additionalDamageAmount);
    }

    public ScriptableUpgrade ReturnScriptableObject()
    {
        return upgradeScriptable;
    }

    public GameObject ReturnGameObject()
    {
        return gameObject;
    }

    private IEnumerator ShootFollowFireBall()
    {
        while (canAttack)
        {
            //we shoot this fire ball in a random direction depending on our clamps
            var direction = Quaternion.Euler(0, 0, Random.Range(-angleOffset, angleOffset));
            GameObject followFireBall = Instantiate(followFireball, transform.position, Quaternion.identity * direction);
            FollowFireBallProjectile followFireBallScript = followFireBall.GetComponent<FollowFireBallProjectile>();
            followFireBallScript.damageAmount = GenerateDamageAmount(player.attack + damageAmount);

            yield return new WaitForSeconds(followFireBallShootInterval / player.attackSpeed);
        }
    }

    protected void Update()
    {
        
    }

}
