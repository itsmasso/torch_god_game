using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This scriptable object keeps track of any player stat modifications, level data, and current upgrade data
[CreateAssetMenu(fileName = "New Player Data", menuName = "Player Data")]
public class ScriptableSaveData : ScriptableObject
{
    public SaveData playerData;  
    
}

public class ReturnSaveData
{
    public SaveData playerData { get; set; }
}


[System.Serializable]
public class SaveData
{
    //player modifications
    public int health;
    public int movementSpeed;
    public int attack;
    public float attackSpeed;
    public float critChance;
    public float pickUpRange;
    public int currentExperience;
    public int maxExperience;
    public int currentLevel;
    public List<int> currentUpgradeIDs = new List<int>();
    public Character character;
    public int currentGameLevel;
    public int currentFloor;
}