using UnityEngine;
public class Fish : MonoBehaviour
{
    public FishData fishData;

    [SerializeField]
    private LayerMask floorLayer;

    private Rigidbody2D rb;
    private Vector2 targetDirection;
    private Vector2 currentDirection;
    private float targetYRotation;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GetRandomMoveDirection();
        timer = 0f;
    }

    private void FixedUpdate()
    {
        if (fishData.behavior.idleSwims)
        {
            timer += Time.fixedDeltaTime;

            if (DetectObstacle())
            {
                GetRandomMoveDirection();
                // Reset timer to avoid immediate direction changes
                timer = 0f;
            }

            currentDirection = Vector2.Lerp(currentDirection, targetDirection, fishData.directionChangeSpeed * Time.fixedDeltaTime);

            // Calculate target position in the current direction
            Vector2 targetPosition = rb.position + currentDirection * 100f; // Large distance for continuous movement
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, fishData.moveSpeed * Time.fixedDeltaTime));


            if (timer > fishData.behavior.timePerDirectionChange)
            {
                timer = 0f;
                GetRandomMoveDirection();
            }
        }
    }

    private void Update()
    {
        if (fishData.behavior.idleSwims)
        {
            if (currentDirection.magnitude > 0.1f)
            {
                float newYRotation = CalculateYRotation();
                float newZRotation = CalculateZRotation();

                transform.rotation = Quaternion.Euler(0f, newYRotation, newZRotation);
            }
        }
    }

    private float CalculateYRotation()
    {
        targetYRotation = currentDirection.x < 0f ? 180f : 0f;
        float currentYRotation = transform.eulerAngles.y;

        // Handle 0/360 wraparound for smooth interpolation
        if (Mathf.Abs(targetYRotation - currentYRotation) > 180f)
        {
            if (currentYRotation > 180f) currentYRotation -= 360f;
            else currentYRotation += 360f;
        }
        return Mathf.Lerp(currentYRotation, targetYRotation, fishData.horizontalFlipSpeed * Time.deltaTime);
    }

    private float CalculateZRotation()
    {
        float verticalAngle = Mathf.Atan2(currentDirection.y, Mathf.Abs(currentDirection.x)) * Mathf.Rad2Deg;
        verticalAngle = Mathf.Clamp(verticalAngle, -fishData.maxVerticalAngle, fishData.maxVerticalAngle);

        float currentZRotation = transform.eulerAngles.z;
        if (currentZRotation > 180f) currentZRotation -= 360f;
        return Mathf.Lerp(currentZRotation, verticalAngle, fishData.verticalRotationSpeed * Time.deltaTime);
    }

    private bool DetectObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, currentDirection, fishData.behavior.detectionDistance, floorLayer);

        if (fishData.debugging)
            Debug.DrawRay(rb.position, currentDirection * fishData.behavior.detectionDistance, hit.collider != null ? Color.red : Color.green);

        return hit.collider != null;
    }

    private void GetRandomMoveDirection()
    {
        targetDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public void Die()
    {
        gameObject.SetActive(false); 
    }
}
