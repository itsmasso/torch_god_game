using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 velocity;
    
    
    void Start()
    {
            
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * speed * Time.fixedDeltaTime);
    }

    private void OnMove(InputValue inputValue)
    {
        velocity = inputValue.Get<Vector2>();
    }

    void Update()
    {


    }
}
