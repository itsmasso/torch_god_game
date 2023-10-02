using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//this singleton class will load all resources from a folder. resources included will be essential scriptable objects that we need to retrieve in bunches
public class ResourceSystem : Singleton<ResourceSystem>
{
    public List<ScriptableEnemyUnit> scriptableEnemyUnits { get; private set; }
    private Dictionary<EnemyType, ScriptableEnemyUnit> enemyDict;
    public List<ScriptablePlayerUnit> scriptablePlayerCharacters { get; private set; }
    private Dictionary<Character, ScriptablePlayerUnit> playerCharacterDict;

    public ScriptablePlayerData persistentPlayerData { get; private set; }

    
    protected override void Awake()
    {
        base.Awake();
        AssembleResources();
       
    }

    private void AssembleResources()
    {
        scriptableEnemyUnits = Resources.LoadAll<ScriptableEnemyUnit>("Enemies").ToList();
        enemyDict = scriptableEnemyUnits.ToDictionary(e => e.enemyType, e => e);

        scriptablePlayerCharacters = Resources.LoadAll<ScriptablePlayerUnit>("Player_Characters").ToList();
        playerCharacterDict = scriptablePlayerCharacters.ToDictionary(c => c.character, c => c);


        persistentPlayerData = Resources.Load<ScriptablePlayerData>("PlayerData/PlayerData");
    }
    public ScriptablePlayerUnit GetPlayerCharacter(Character character)
    {
        return playerCharacterDict[character];
    }
    public ScriptableEnemyUnit GetEnemyUnit(EnemyType type)
    {
        return enemyDict[type];
    }

}
