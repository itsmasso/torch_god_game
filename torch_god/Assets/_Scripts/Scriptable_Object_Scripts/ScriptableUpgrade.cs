using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UpgradeType
{
    StatUpgrade,
    Ability,
    Items

}
[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrades")]
public class ScriptableUpgrade : ScriptableObject
{
    public UpgradeType upgradeType;
    public GameObject upgradePrefab;
    public bool needAim;
    public PlayerStats statModification;
    public string upgradeName;
    public string upgradeDescription;
    public int baseDamage;
    public int level;
}
