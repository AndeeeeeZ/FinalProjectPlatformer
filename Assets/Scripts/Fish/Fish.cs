using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private bool debugging;
    public FishData fishData;
    public FishBehavior behavior;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private SpriteRenderer sr;

    // Private fields
    private Rigidbody2D rb;
    private Vector2 targetDirection;
    private Vector2 currentDirection;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (behavior != null && behavior.idleSwims)
        {
            targetDirection = behavior.GetNextDirection(currentDirection, 0f);
            timer = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (behavior == null || !behavior.idleSwims)
            return;

        timer += Time.fixedDeltaTime;

        bool obstacleDetected = DetectObstacle();

        if (behavior.ShouldChangeDirection(timer, obstacleDetected))
        {
            targetDirection = behavior.GetNextDirection(currentDirection, Time.fixedDeltaTime);
            timer = 0f;
        }

        currentDirection = Vector2.Lerp(currentDirection, targetDirection, behavior.directionChangeSpeed * Time.fixedDeltaTime);

        Vector2 newPosition = behavior.CalculateMovement(rb.position, currentDirection, targetDirection, Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }

    private void Update()
    {
        if (behavior == null || !behavior.idleSwims)
            return;

        if (currentDirection.magnitude > 0.1f)
        {
            float newYRotation = behavior.CalculateYRotation(currentDirection, transform.eulerAngles.y, Time.deltaTime);
            float newZRotation = behavior.CalculateZRotation(currentDirection, transform.eulerAngles.z, Time.deltaTime);

            transform.rotation = Quaternion.Euler(0f, newYRotation, newZRotation);
        }
    }

    private bool DetectObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, currentDirection, behavior.detectionDistance, floorLayer);

        if (debugging)
            Debug.DrawRay(rb.position, currentDirection * behavior.detectionDistance, hit.collider != null ? Color.red : Color.green);

        return hit.collider != null;
    }

    public void SetFishDataAndBehavior(FishData data, FishBehavior b)
    {
        fishData = data;
        behavior = b;
        sr.sprite = fishData.fishSprite;

        if (behavior != null && behavior.idleSwims)
        {
            currentDirection = behavior.GetNextDirection(Vector2.zero, 0f);
            targetDirection = currentDirection;
            timer = 0f;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}