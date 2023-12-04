using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthBarSlider;

    [SerializeField] private TMP_Text healthText;

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

        PlayerBaseScript.onTakeDamage += LoseHealth;
        HealingUnitScript.onDeath += GainHealth;
        //EnemyBaseScript.onEnemyDealDamage += LoseHealth;
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

    private void GainHealth(int amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
            
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        healthBarSlider.value = (float)currentHealth / maxHealth;

        //Debug.Log("Hit with " + amount);
        //Debug.Log("Current Health:" + currentHealth);
    }
    private void Update()
    {
        healthText.text = string.Format("{0}/{1}", currentHealth, maxHealth);
    }
    private void OnDestroy()
    {
        PlayerBaseScript.onTakeDamage -= LoseHealth;
        HealingUnitScript.onDeath -= GainHealth;
        //EnemyBaseScript.onEnemyDealDamage -= LoseHealth;
    }
}
