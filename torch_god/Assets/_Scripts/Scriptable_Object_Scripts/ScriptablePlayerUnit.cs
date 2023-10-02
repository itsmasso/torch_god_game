using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Player Character", menuName = "Player Character")]
public class ScriptablePlayerUnit : ScriptableObject
{
    public Character character;
    public PlayerStats baseStats;
    public string characterName;
    public GameObject characterPrefab;
}


[Serializable]
public struct PlayerStats
{
    public int health;
    public int movementSpeed;
    public int attack;
    public float attackSpeed;
    public float critChance;
    public float pickUpRange;
    //public int CritDamage;

}

[Serializable]
public enum Character
{
    TorchCharacter,
    LanternCharacter
}