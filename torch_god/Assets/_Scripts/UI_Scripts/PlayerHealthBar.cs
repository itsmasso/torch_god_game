using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthBarSlider;


    private GameObject player;
    private PlayerBaseScript playerStats;

    private int maxHealth;
    private int currentHealth;

    void Start()
    {
        player = LevelManager.Instance.player;
        playerStats = player.GetComponent<PlayerBaseScript>();
        maxHealth = playerStats.stats.health;
        currentHealth = maxHealth;

        EnemyBaseScript.onDealDamage += LoseHealth;
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
        
        //Debug.Log("Hit with " + amount);
        //Debug.Log("Current Health:" + currentHealth);
    }

    private void OnDestroy()
    {
        EnemyBaseScript.onDealDamage -= LoseHealth;
    }
}
