using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UpgradeManager : MonoBehaviour
{
    public delegate void OnPickedUpgrade();
    public static event OnPickedUpgrade onPickedUpgrade;

    [SerializeField] private TMP_Text upgradeName1, upgradeDescription1, upgradeName2, upgradeDescription2, upgradeName3, upgradeDescription3;

    private UpgradesScriptable upgrade1, upgrade2, upgrade3;

    [SerializeField] private List<UpgradesScriptable> scriptableUpgrades;

    private void OnEnable()
    {
        GenerateRandomUpgrades();
    }
    private void UpgradeStat(UpgradesScriptable upgradeScriptable)
    {
        LevelManager.Instance.playerData.attack += upgradeScriptable.statModification.attack;
        LevelManager.Instance.playerData.attackSpeed += upgradeScriptable.statModification.attackSpeed;
        LevelManager.Instance.playerData.health += upgradeScriptable.statModification.health;
        LevelManager.Instance.playerData.critChance += upgradeScriptable.statModification.critChance;
        LevelManager.Instance.playerData.movementSpeed += upgradeScriptable.statModification.movementSpeed;
        LevelManager.Instance.playerData.pickUpRange += upgradeScriptable.statModification.pickUpRange;
    }

    private void Item()
    {
        //TODO
    }

    private void WeaponEnhancement()
    {
        //TODO
    }

    private void Ability()
    {
        //TODO
    }


    private void GenerateRandomUpgrades()
    {
        //set all upgrade buttons,images, text here
        upgrade1 = scriptableUpgrades[Random.Range(0, scriptableUpgrades.Count)];
        upgrade2 = scriptableUpgrades[Random.Range(0, scriptableUpgrades.Count)];
        upgrade3 = scriptableUpgrades[Random.Range(0, scriptableUpgrades.Count)];

        upgradeName1.text = upgrade1.name;
        upgradeDescription1.text = upgrade1.upgradeDescription;

        upgradeName2.text = upgrade2.name;
        upgradeDescription2.text = upgrade2.upgradeDescription;

        upgradeName3.text = upgrade3.name;
        upgradeDescription3.text = upgrade3.upgradeDescription;
    }

    private void DetermineUpgradeType(UpgradesScriptable upgrade)
    {
        UpgradeType type = upgrade.upgradeType;
        switch (type)
        {
            case UpgradeType.StatUpgrade:
                UpgradeStat(upgrade);
                break;
            case UpgradeType.Ability:
                break;
            case UpgradeType.WeaponEnhancement:
                break;
            case UpgradeType.Items:
                break;
            default:
                break;
        }
    }

    public void PickUpgrade1()
    {
        DetermineUpgradeType(upgrade1);
        LevelManager.Instance.UpdateGameState(LevelManager.Instance.currentWave);
        onPickedUpgrade?.Invoke();
        Time.timeScale = 1;
    }

    public void PickUpgrade2()
    {
        DetermineUpgradeType(upgrade2);
        LevelManager.Instance.UpdateGameState(LevelManager.Instance.currentWave);
        onPickedUpgrade?.Invoke();
        Time.timeScale = 1;
    }

    public void PickUpgrade3()
    {
        DetermineUpgradeType(upgrade3);
        LevelManager.Instance.UpdateGameState(LevelManager.Instance.currentWave);
        onPickedUpgrade?.Invoke();
        Time.timeScale = 1;
    }

    private void Update()
    {
        
    }
}
