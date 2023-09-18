using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;
    public PlayerStats stats { get; private set; } //own struct of stats independent of this class. can modify without changing base stats

    //add implementation for stat modification here
    private void Awake()
    {
        stats = playerData.baseStats;
    }
}
