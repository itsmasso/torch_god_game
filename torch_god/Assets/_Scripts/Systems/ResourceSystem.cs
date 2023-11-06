using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//this singleton class will load all resources from a folder. resources included will be essential scriptable objects that we need to retrieve in bunches
public class ResourceSystem : Singleton<ResourceSystem>
{
    public List<ScriptablePlayerUnit> scriptablePlayerCharacters { get; private set; }
    private Dictionary<Character, ScriptablePlayerUnit> playerCharacterDict;

    public ScriptableSaveData saveData { get; private set; }

    public List<ScriptableEnemyUnit> tier1Enemies { get; private set; }

    public List<ScriptableUpgrade> scriptableUpgrades = new List<ScriptableUpgrade>();
    private Dictionary<int, ScriptableUpgrade> scriptableUpgradesDict = new Dictionary<int, ScriptableUpgrade>();

    public GameObject damagePopup { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        AssembleResources();
       
    }

    private void AssembleResources()
    {
        string[] upgradePaths = new string[] { "Upgrades/Ability", "Upgrades/Items", "Upgrades/StatUpgrades", "Upgrades/WeaponEnhancement" };
        foreach(string path in upgradePaths)
        {
            scriptableUpgrades.AddRange(Resources.LoadAll<ScriptableUpgrade>(path));
        }
        for (int i = 0; i < scriptableUpgrades.Count; i++)
        {
            scriptableUpgradesDict.Add(i, scriptableUpgrades[i]);
        }
        tier1Enemies = Resources.LoadAll<ScriptableEnemyUnit>("Tier1_Enemies").ToList();

        damagePopup = Resources.Load<GameObject>("Game_Assets/DamagePopUp");
        scriptablePlayerCharacters = Resources.LoadAll<ScriptablePlayerUnit>("Player_Characters").ToList();
        playerCharacterDict = scriptablePlayerCharacters.ToDictionary(c => c.character, c => c);


        saveData = Resources.Load<ScriptableSaveData>("SaveData/PlayerData");

    }
    public ScriptablePlayerUnit GetPlayerCharacter(Character character)
    {
        return playerCharacterDict[character];
    }

    public int GetUpgradeID(ScriptableUpgrade upgrade)
    {
        return scriptableUpgradesDict.FirstOrDefault(x => x.Value == upgrade).Key;
    }

    public ScriptableUpgrade GetUpgradeByID(int id)
    {
        return scriptableUpgradesDict[id];
    }

    public void ResetPlayerData()
    {
        saveData.playerData.health = 0;
        saveData.playerData.attack = 0;
        saveData.playerData.attackSpeed = 0;
        saveData.playerData.critChance = 0;
        saveData.playerData.currentLevel = 1;
        saveData.playerData.pickUpRange = 0;
        saveData.playerData.movementSpeed = 0;
        saveData.playerData.currentExperience = 0;
        saveData.playerData.maxExperience = 0;
        if(saveData.playerData.currentUpgradeIDs.Count != 0)
        {
            foreach (int id in saveData.playerData.currentUpgradeIDs)
            {
                GetUpgradeByID(id).level = 0;
            }
        }
        saveData.playerData.currentUpgradeIDs.Clear();
        saveData.playerData.currentGameLevel = 1;
        saveData.playerData.currentFloor = 1;

    }




}
