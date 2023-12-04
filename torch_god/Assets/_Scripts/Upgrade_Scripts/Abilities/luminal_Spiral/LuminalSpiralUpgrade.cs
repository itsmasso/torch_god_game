using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuminalSpiralUpgrade : AbilityUpgradeBase, IUpgradeable
{
    [SerializeField] protected float shootInterval;
    [SerializeField] private GameObject luminalSpiralPrefab;
    [SerializeField] private float angleChange;
    private float currentAngle = 0;
    [SerializeField] private float maxBounces;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(ShootLuminalSpiral());
    }
    public void LevelUpUpgrade()
    {
        currentLevel++;
        upgradeScriptable.level = currentLevel;
        //maybe add cool upgrades besides damage later
        damageAmount = upgradeScriptable.baseDamage + (currentLevel * additionalDamageAmount);
        if(currentLevel % 2 == 0)
        {
            maxBounces++;
        }
    }

    public GameObject ReturnGameObject()
    {
        return gameObject;
    }

    public ScriptableUpgrade ReturnScriptableObject()
    {
        return upgradeScriptable;
    }

    private IEnumerator ShootLuminalSpiral()
    {
        while (canAttack)
        {
            //we shoot this fire ball in a random direction depending on our clamps
            var direction = Quaternion.Euler(0, 0, currentAngle);
            currentAngle += angleChange;
            GameObject luminalSpiral = Instantiate(luminalSpiralPrefab, transform.position, Quaternion.identity * direction);
            LuminalSpiral luminalSpiralScript = luminalSpiral.GetComponent<LuminalSpiral>();
            luminalSpiralScript.maxBounces = maxBounces;
            luminalSpiralScript.damageAmount = GenerateDamageAmount(player.attack + damageAmount);

            yield return new WaitForSeconds(shootInterval / player.attackSpeed);
        }
    }
}
