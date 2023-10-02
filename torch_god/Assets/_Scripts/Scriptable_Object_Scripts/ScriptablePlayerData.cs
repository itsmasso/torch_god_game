using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This scriptable object keeps track of any player stat modifications, level data, and current upgrade data
[CreateAssetMenu(fileName = "New Player Data", menuName = "Player Data")]
public class ScriptablePlayerData : ScriptableObject
{
    public int health, movementSpeed, attack;
    public float attackSpeed, critChance, pickUpRange;
    public int currentExperience, maxExperience;
    public int currentLevel;
    public List<UpgradesScriptable> currentUpgrades = new List<UpgradesScriptable>();
}
