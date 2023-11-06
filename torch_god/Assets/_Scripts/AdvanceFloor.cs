using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceFloor : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    private Vector2 size;
    private void Start()
    {
        size = boxCollider.size;
    }
    void Update()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, size, 0, transform.position.normalized, 0, playerLayer);
        if(hit.collider != null)
        {
            LevelManager.Instance.IncrementFloors();          
            
        }
    }

}
