using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawColliderBoxes : MonoBehaviour
{
    [SerializeField] 
    private BoxCollider2D boxCollider;

    private void OnDrawGizmos()
    {
        if (boxCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
        }
    }
}

