using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public abstract class Weapon : MonoBehaviour
{
    public Transform firePoint;

    protected PlayerBaseScript player;

    protected Vector2 mousePosition; //regular mouse position on screen
    protected Vector2 projectedMousePosition;

    protected bool canAttack;
    protected virtual void Start()
    {
        player = GetComponentInParent<PlayerBaseScript>();
        canAttack = true;
    }
    public virtual void OnAim(InputAction.CallbackContext ctx)
    {
        mousePosition = ctx.ReadValue<Vector2>();
    }

    public virtual void OnFire(InputAction.CallbackContext ctx)
    {
        //stuff
    }

    protected virtual int GenerateDamageAmount(int amount)
    {
        float randomDamageMultiplier = 0.1f; 
        int randomDamage = Random.Range(amount - Mathf.RoundToInt(amount * randomDamageMultiplier), amount + Mathf.RoundToInt(amount * randomDamageMultiplier));
        return randomDamage;
    }
    protected virtual void Update()
    {
        projectedMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 aimDirection = new Vector2(projectedMousePosition.x - transform.position.x, projectedMousePosition.y - transform.position.y);
        transform.up = aimDirection;


    }

    protected virtual void OnDestroy()
    {
        //stuff
    }

}
