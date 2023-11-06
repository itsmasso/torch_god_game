using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityUpgradeBase : MonoBehaviour
{
    public ScriptableUpgrade upgradeScriptable;

    protected PlayerBaseScript player;

    public bool canAttack;
    protected int damageAmount;
    public int currentLevel;
    [SerializeField] protected int additionalDamageAmount;
    protected virtual void Start()
    {
        if (currentLevel == 0)
            currentLevel = 1;
        damageAmount = upgradeScriptable.baseDamage;
        currentLevel = upgradeScriptable.level;
        damageAmount = upgradeScriptable.baseDamage + (currentLevel * additionalDamageAmount);
        canAttack = true;
        player = GetComponentInParent<PlayerBaseScript>();
    }

    protected int GenerateDamageAmount(int amount)
    {
        float randomDamageMultiplier = 0.3f; //randomizes damage. A higher multiplier creates a more random damage number and higher range
        int randomDamage = UnityEngine.Random.Range(amount - Mathf.RoundToInt(amount * randomDamageMultiplier), amount + Mathf.RoundToInt(amount * randomDamageMultiplier));
        return randomDamage;
    }

}
