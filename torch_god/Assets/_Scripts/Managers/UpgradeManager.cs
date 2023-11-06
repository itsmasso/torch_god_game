using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UpgradeManager : MonoBehaviour
{
    public delegate void OnPickedUpgrade();
    public static event OnPickedUpgrade onPickedUpgrade;

    [SerializeField] private TMP_Text upgradeName1, upgradeDescription1, upgradeName2, upgradeDescription2, upgradeName3, upgradeDescription3;

    private List<ScriptableUpgrade> chosenUpgrades = new List<ScriptableUpgrade>();

    [SerializeField] private ScriptableSaveData playerData;

    private void OnEnable()
    {
        GenerateRandomUpgrades();
    }
    private void UpgradeStat(ScriptableUpgrade upgrade)
    {
        ScriptableSaveData playerData = LevelManager.Instance.saveData;
        playerData.playerData.attack += upgrade.statModification.attack;
        playerData.playerData.attackSpeed += upgrade.statModification.attackSpeed;
        playerData.playerData.health += upgrade.statModification.health;
        playerData.playerData.critChance += upgrade.statModification.critChance;
        playerData.playerData.movementSpeed += upgrade.statModification.movementSpeed;
        playerData.playerData.pickUpRange += upgrade.statModification.pickUpRange;
    }

    private void AttatchUpgradesToPlayer(GameObject upgrade)
    {
        GameObject parent = LevelManager.Instance.player;

        if (upgrade.GetComponent<IUpgradeable>().ReturnScriptableObject().needAim)
        {
            Weapon weaponReference = parent.GetComponentInChildren<Weapon>();
            GameObject upgradeObj = Instantiate(upgrade, weaponReference.transform.position, weaponReference.firePoint.rotation);
            upgradeObj.transform.parent = weaponReference.firePoint.transform;

        }
        else
        {
            
            GameObject upgradeObj = Instantiate(upgrade, parent.transform.position, Quaternion.identity);
            upgradeObj.transform.parent = parent.transform;
        }
    }

    private void Item()
    {
        //TODO
    }

    private void Ability(ScriptableUpgrade upgrade)
    {
        bool hasUpgrade = false;

        //we level up upgrades in scene and in player data list because the ones in scenes get destroyed after scene loads and we load in saved ones in player data.
        IUpgradeable[] upgradeableInSceneList = LevelManager.Instance.player.GetComponentsInChildren<IUpgradeable>();
        foreach (IUpgradeable upgradeableInScene in upgradeableInSceneList)
        {
            if (upgradeableInScene.ReturnScriptableObject() == upgrade)
            {
                //level up current upgrade in scene 
                upgradeableInScene.LevelUpUpgrade();
                hasUpgrade = true;
                break;
            }
        }

        if (!hasUpgrade)
        {
            playerData.playerData.currentUpgradeIDs.Add(ResourceSystem.Instance.GetUpgradeID(upgrade));
            AttatchUpgradesToPlayer(upgrade.upgradePrefab);
        }
    }


    private void GenerateRandomUpgrades()
    {
        //set all upgrade buttons,images, text here
        List<ScriptableUpgrade> scriptableUpgrades = ShuffleList(ResourceSystem.Instance.scriptableUpgrades);

        for (int i = 0; i < 3; i++)
        {       
            chosenUpgrades.Add(scriptableUpgrades[i]);
        }        
      
        upgradeName1.text = chosenUpgrades[0].upgradeName;
        upgradeDescription1.text = chosenUpgrades[0].upgradeDescription;

        upgradeName2.text = chosenUpgrades[1].upgradeName;
        upgradeDescription2.text = chosenUpgrades[1].upgradeDescription;

        upgradeName3.text = chosenUpgrades[2].upgradeName;
        upgradeDescription3.text = chosenUpgrades[2].upgradeDescription;
    }

    // Fisher–Yates shuffle algo
    private List<T> ShuffleList<T>(List<T> list)
    {       
       
        for (int i = 0; i < list.Count; i++)
        {
            // Generate a random index within the remaining unshuffled elements
            int randomIndex = Random.Range(0, list.Count);

            //swap random index and i
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
    private void DetermineUpgradeType(ScriptableUpgrade upgrade)
    {
        UpgradeType type = upgrade.upgradeType;
        switch (type)
        {
            case UpgradeType.StatUpgrade:
                UpgradeStat(upgrade);
                break;
            case UpgradeType.Ability:
                Ability(upgrade);
                break;
            case UpgradeType.Items:
                break;
            default:
                break;
        }
    }

    public void PickUpgrade1()
    {
        DetermineUpgradeType(chosenUpgrades[0]);
        LevelManager.Instance.UpdateGameState(GameState.SpawningWaves);
        onPickedUpgrade?.Invoke();
        Time.timeScale = 1;
        
    }

    public void PickUpgrade2()
    {
        DetermineUpgradeType(chosenUpgrades[1]);
        LevelManager.Instance.UpdateGameState(GameState.SpawningWaves);
        onPickedUpgrade?.Invoke();
        Time.timeScale = 1;
    }

    public void PickUpgrade3()
    {
        DetermineUpgradeType(chosenUpgrades[2]);
        LevelManager.Instance.UpdateGameState(GameState.SpawningWaves);
        onPickedUpgrade?.Invoke();
        Time.timeScale = 1;
    }

    private void OnDisable()
    {
        chosenUpgrades.Clear();
    }
}
