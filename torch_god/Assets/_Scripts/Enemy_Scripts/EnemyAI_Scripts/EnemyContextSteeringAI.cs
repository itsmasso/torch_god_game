using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContextSteeringAI : MonoBehaviour
{
    [SerializeField]
    private bool showObstacleDetectorGizmos = true, showObstacleAvoidanceGizmo = true, showPlayerFollowGizmos = true, showContextSolverGizmos;

    [SerializeField]
    private float detectionDelay = 0.05f;

    [Header("AI Data")]
    public Collider2D[] obstacles = null;
    public Transform playerTransform;

    [Header("Obstacle Detection Properties")]
    [SerializeField]
    private float detectionRadius = 2;
    [SerializeField]
    private LayerMask layerMask;

    [Header("Obstacle Avoidance Properties")]
    [SerializeField]
    private float radius = 2f, agentColliderSize = 0.6f;
    //gizmo parameters
    float[] dangersResultTemp = null;

    [Header("Player Follow Properties")]
    //gizmo parameters
    private float[] interestsTemp;

    [Header("Context Solver Properties")]
    //gizmo parameters
    Vector2 resultDirection = Vector2.zero;
    private float rayLength = 1;

    Collider2D[] colliders;

    private void Start()
    {
        playerTransform = GetComponent<EnemyBaseScript>().player.transform;
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void PerformDetection()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, layerMask);
        obstacles = colliders;

    }
    public (float[] danger, float[] interest) PlayerFollowSteering(float[] danger, float[] interest)
    {
        Vector2 directionToTarget = (playerTransform.transform.position - transform.position);
        for (int i = 0; i < interest.Length; i++)
        {
            float result = Vector2.Dot(directionToTarget.normalized, Directions.eightDirections[i]);

            //accept only directions at the less than 90 degrees to the target direction
            if (result > 0)
            {
                float valueToPutIn = result;
                if (valueToPutIn > interest[i])
                {
                    interest[i] = valueToPutIn;
                }

            }
        }
        interestsTemp = interest;
        return (danger, interest);
    }

    public (float[] danger, float[] interest) ObstacleAvoidanceSteering(float[] danger, float[] interest)
    {
        foreach (Collider2D obstacleCollider in obstacles)
        {
            if(obstacleCollider != null)
            {
                Vector2 directionToObstacle = obstacleCollider.ClosestPoint(transform.position) - (Vector2)transform.position;
                float distanceToObstacle = directionToObstacle.magnitude;

                float weight = distanceToObstacle <= agentColliderSize ? 1 : (radius - distanceToObstacle) / radius;

                Vector2 directionToObstacleNormalized = directionToObstacle.normalized;

                for (int i = 0; i < Directions.eightDirections.Count; i++)
                {
                    float result = Vector2.Dot(directionToObstacleNormalized, Directions.eightDirections[i]);
                    float valueInput = result * weight;
                    if (valueInput > danger[i])
                    {
                        danger[i] = valueInput;
                    }
                }
            }
        }
        dangersResultTemp = danger;
        return (danger, interest);
    }
    public Vector2 GetDirectionToMove()
    {
        float[] danger = new float[8];
        float[] interest = new float[8];

        (danger, interest) = ObstacleAvoidanceSteering(danger, interest);
        (danger, interest) = PlayerFollowSteering(danger, interest);

        //subtract interest values by danger values
        for (int i = 0; i < 8; i++)
        {
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }

        //get avg direction
        Vector2 outputDirection = Vector2.zero;
        for (int i = 0; i < 8; i++)
        {
            outputDirection += Directions.eightDirections[i] * interest[i];
        }
        outputDirection.Normalize();

        resultDirection = outputDirection;

        //return selected movement direction
        return resultDirection;
    }

    private void OnDrawGizmos()
    {
        //gizmos for obstacle detector
        if (showObstacleDetectorGizmos)
        {
            if (Application.isPlaying && colliders != null)
            {
                Gizmos.color = Color.red;
                foreach (Collider2D obstacleCollider in colliders)
                {
                    Gizmos.DrawSphere(obstacleCollider.transform.position, 0.2f);
                }
            }
        }

        if (showObstacleAvoidanceGizmo)
        {
            if (Application.isPlaying && dangersResultTemp != null)
            {
                if (dangersResultTemp != null)
                {
                    Gizmos.color = Color.red;
                    for (int i = 0; i < dangersResultTemp.Length; i++)
                    {
                        Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * dangersResultTemp[i]);
                    }
                }
            }
            else
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, radius);
            }
        }

        if (showPlayerFollowGizmos && playerTransform != null)
        {
            Gizmos.DrawSphere(playerTransform.position, 0.2f);

            if (Application.isPlaying && interestsTemp != null)
            {
                if (interestsTemp != null)
                {
                    Gizmos.color = Color.green;
                    for (int i = 0; i < interestsTemp.Length; i++)
                    {
                        Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestsTemp[i] * 2);
                    }
                }
            }
        }

        if (showContextSolverGizmos)
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(transform.position, resultDirection * rayLength);
            }
        }

    }
}

public static class Directions
{
    public static List<Vector2> eightDirections = new List<Vector2>
    {
        new Vector2(0,1).normalized,
        new Vector2(1,1).normalized,
        new Vector2(1,0).normalized,
        new Vector2(1,-1).normalized,
        new Vector2(0,-1).normalized,
        new Vector2(-1,-1).normalized,
        new Vector2(-1,0).normalized,
        new Vector2(-1,1).normalized
    };
}
