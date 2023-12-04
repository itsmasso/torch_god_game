using UnityEngine;

//Predictive aim script. Formula for predicting target movement and aims where target tries to move to. 
//Credit:
//Link to source: https://www.youtube.com/watch?v=2zVwug_agr0&t=316s
public static class PredictiveAimScript
{
    public static bool PredictMovement(Vector2 targetPosition, Vector2 currentPosition, Vector2 targetVelocity, float projectileSpeed, out Vector2 result)
    {
        //target = a
        //current = b
        Vector2 directionAB = currentPosition - targetPosition;
        float directionC = directionAB.magnitude;
        float alpha = Vector2.Angle(directionAB, targetVelocity) * Mathf.Deg2Rad;
        float targetSpeed = targetVelocity.magnitude;
        float r = targetSpeed / projectileSpeed;
        if (SolveQuadratic(1 - r * r, 2 * r * directionC * Mathf.Cos(alpha), -(directionC * directionC), out float root1, out float root2) == 0)
        {
            result = Vector2.zero;
            return false;
        }
        float directionA = Mathf.Max(root1, root2);
        float time = directionA / projectileSpeed;
        Vector2 c = targetPosition + targetVelocity * time;
        result = (c - currentPosition).normalized;
        return true;


    }

    /*
    Quadratic Formula:
    x = -b * SquareRoot(b^2 - 4ac)
         -------------------------
                     2a
    */
    public static int SolveQuadratic(float a, float b, float c, out float root1, out float root2)
    {
        //discriminant is (b^2 - 4ac)
        var discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            root1 = Mathf.Infinity;
            root2 = -root1;
            return 0;
        }
        root1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
        root2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);
        return discriminant > 0 ? 2 : 1;

    }
}
