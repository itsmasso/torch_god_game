using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Cam_Follow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothing;
    [SerializeField] private Vector2 maxPositions;
    [SerializeField] private Vector2 minPositions;
    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        targetPosition.x = Mathf.Clamp(targetPosition.x, minPositions.x, maxPositions.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minPositions.y, maxPositions.y);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothing);

    }
}
