using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthBarSlider;


    private GameObject player;
    private Player_Stats playerStats;

    private int maxHealth;
    private int currentHealth;

    void Start()
    {
        player = GameManager.Instance.player;
        playerStats = player.GetComponent<Player_Stats>();
        maxHealth = playerStats.stats.health;
        currentHealth = maxHealth;

        PlayerBehavior.onTakeDamage += LoseHealth;
    }
    
    private void LoseHealth(int amount)
    {  
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= amount;
        }
        healthBarSlider.value = (float)currentHealth / maxHealth;
        
        Debug.Log("Hit with " + amount);
        Debug.Log("Current Health:" + currentHealth);
    }

    private void OnDestroy()
    {
        PlayerBehavior.onTakeDamage -= LoseHealth;
    }
}
