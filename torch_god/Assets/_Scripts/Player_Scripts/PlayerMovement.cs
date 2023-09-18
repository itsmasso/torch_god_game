using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] 
    private float speed;

    [SerializeField] 
    private Player_Stats playerStats;

    [SerializeField] 
    private Rigidbody2D rb;

    private Vector2 velocity;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private SpriteRenderer sprite;

    private bool facingLeft;
    
    void Start()
    {
        facingLeft = false;
        speed = playerStats.stats.movementSpeed;
    }

    private void FixedUpdate()
    {
        if(velocity != Vector2.zero)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
        rb.MovePosition(rb.position + velocity * speed * Time.fixedDeltaTime);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        velocity = ctx.ReadValue<Vector2>();
        
    }

    private void Update()
    {
        if(velocity.x < 0)
        {
            //face left
            facingLeft = true;
            sprite.flipX = facingLeft;
        }
        else if(velocity.x > 0)
        {
            facingLeft = false;
            sprite.flipX = facingLeft;
        }
        else
        {
            sprite.flipX = facingLeft;
        }
    }

}
