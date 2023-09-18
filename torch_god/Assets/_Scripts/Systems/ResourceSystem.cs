using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSystem : Singleton<ResourceSystem>
{
    public List<ScriptableEnemyUnit> scriptableEnemyUnits { get; private set; }
    private Dictionary<EnemyType, ScriptableEnemyUnit> enemyDict;
    protected override void Awake()
    {
        base.Awake();
        AssembleResources();
       
    }

    private void AssembleResources()
    {
        scriptableEnemyUnits = Resources.LoadAll<ScriptableEnemyUnit>("LevelOne_EnemyUnits").ToList();
        enemyDict = scriptableEnemyUnits.ToDictionary(e => e.enemyType, e => e);
    }

    public ScriptableEnemyUnit GetEnemyUnit(EnemyType type)
    {
        return enemyDict[type];
    }
}
