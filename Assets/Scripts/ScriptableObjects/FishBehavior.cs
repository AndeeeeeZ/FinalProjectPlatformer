using UnityEngine;

[CreateAssetMenu]
public class FishBehavior : ScriptableObject
{
    public bool idleSwims;
    public float timePerDirectionChange;
    public float detectionDistance;
    public float moveSpeed;
    public float directionChangeSpeed;
    public float horizontalFlipSpeed;
    public float verticalRotationSpeed;
    public float maxVerticalAngle;


    public virtual Vector2 GetNextDirection(Vector2 currentDirection, float deltaTime)
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public virtual bool ShouldChangeDirection(float timer, bool obstacleDetected)
    {
        return obstacleDetected || timer > timePerDirectionChange;
    }

    public virtual Vector2 CalculateMovement(Vector2 position, Vector2 currentDirection, Vector2 targetDirection, float deltaTime)
    {
        // Vector2 smoothDirection = Vector2.Lerp(currentDirection, targetDirection, directionChangeSpeed * deltaTime);
        // Vector2 targetPosition = position + smoothDirection * 100f;
        // return Vector2.MoveTowards(position, targetPosition, moveSpeed * deltaTime);
        Vector2 smoothDirection = Vector2.Lerp(currentDirection, targetDirection, directionChangeSpeed * deltaTime);
        return position + smoothDirection * moveSpeed * deltaTime;
    }

    public virtual float CalculateYRotation(Vector2 currentDirection, float currentYRotation, float deltaTime)
    {
        float targetYRotation = currentDirection.x < 0f ? 180f : 0f;

        // Handle 0/360 wraparound for smooth interpolation
        if (Mathf.Abs(targetYRotation - currentYRotation) > 180f)
        {
            if (currentYRotation > 180f) currentYRotation -= 360f;
            else currentYRotation += 360f;
        }
        return Mathf.Lerp(currentYRotation, targetYRotation, horizontalFlipSpeed * deltaTime);
    }

    public virtual float CalculateZRotation(Vector2 currentDirection, float currentZRotation, float deltaTime)
    {
        float verticalAngle = Mathf.Atan2(currentDirection.y, Mathf.Abs(currentDirection.x)) * Mathf.Rad2Deg;
        verticalAngle = Mathf.Clamp(verticalAngle, -maxVerticalAngle, maxVerticalAngle);

        if (currentZRotation > 180f) currentZRotation -= 360f;
        return Mathf.Lerp(currentZRotation, verticalAngle, verticalRotationSpeed * deltaTime);
    }
}
