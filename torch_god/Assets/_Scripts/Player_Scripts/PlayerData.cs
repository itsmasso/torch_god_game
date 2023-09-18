using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Player Data")]
public class PlayerData : ScriptableObject
{
    public PlayerStats baseStats;
}


[Serializable]
public struct PlayerStats
{
    public int health;
    public int movementSpeed;
    public int attack;
    public float attackSpeed;
    public float critChance;
    //public int CritDamage;

}