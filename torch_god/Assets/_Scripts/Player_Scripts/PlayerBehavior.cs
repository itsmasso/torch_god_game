using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField] 
    private Player_Stats playerStats;

    public delegate void OnTakeDamage(int amount); 
    public static OnTakeDamage onTakeDamage;

    private int maxHealth;
    public int currentHealth { get; private set; }

    private void Start()
    {
        maxHealth = playerStats.stats.health;
        currentHealth = maxHealth;

        onTakeDamage += TakeDamage;
    }
    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= amount;
        }
        //Debug.Log("Hit with " + amount);
        //Debug.Log("Current Health:" + currentHealth);
    }

    private void Update()
    {
      //TODO  
    }

    private void OnDestroy()
    {
        onTakeDamage -= TakeDamage;
    }
}
