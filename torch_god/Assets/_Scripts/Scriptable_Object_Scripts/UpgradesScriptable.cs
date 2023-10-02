using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UpgradeType
{
    StatUpgrade,
    Ability,
    Items,
    WeaponEnhancement
}
[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrades")]
public class UpgradesScriptable : ScriptableObject
{
    public UpgradeType upgradeType;

    public PlayerStats statModification;
    public string upgradeName;
    public string upgradeDescription;
    public int abilityDamage;
}
