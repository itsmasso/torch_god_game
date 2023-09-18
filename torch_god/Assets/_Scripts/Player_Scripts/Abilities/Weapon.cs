using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;


public class Weapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private Player_Stats playerStats;

    private Vector2 mousePosition; //regular mouse position on screen
    private Vector2 projectedMousePosition;

    public void OnAim(InputAction.CallbackContext ctx)
    {
        mousePosition = ctx.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.damageAmount = DealDamage(playerStats.stats.attack);
        }
    }

    private int DealDamage(int amount)
    {
        float randomDamageMultiplier = 0.3f; //randomizes damage. A higher multiplier creates a more random damage number and higher range
        int randomDamage = UnityEngine.Random.Range(amount - Mathf.RoundToInt(amount * randomDamageMultiplier), amount + Mathf.RoundToInt(amount * randomDamageMultiplier));
        return randomDamage;
    }
    private void Update()
    {
        projectedMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 aimDirection = new Vector2(projectedMousePosition.x - transform.position.x, projectedMousePosition.y - transform.position.y);
        transform.up = aimDirection;


    }

}
