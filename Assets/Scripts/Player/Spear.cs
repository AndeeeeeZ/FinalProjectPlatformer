using UnityEngine;
using UnityEngine.InputSystem;

public class Spear : MonoBehaviour
{
    [SerializeField] private FishDataEvent OnFishKilled;
    [SerializeField] private VoidEvent OnAFishDied; 
    [SerializeField] private Transform targetLocation;
    [SerializeField] private Rope rope; 
    [SerializeField] private float maxHoldTime;
    [SerializeField] private float maxShootForce;
    [SerializeField] private float returningSpeed;
    private PlatformerActions input;
    private Rigidbody2D rb;
    private SpearState currentState;
    private float timer;

    private void Awake()
    {
        input = new PlatformerActions();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentState = SpearState.HOLDING;
        timer = 0f;
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Hold.performed += OnHoldPerformed;
        input.Player.Hold.canceled += OnHoldCanceled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Hold.performed -= OnHoldPerformed;
        input.Player.Hold.canceled -= OnHoldCanceled;
    }

    private void Update()
    {
        if (currentState == SpearState.AIMING)
            timer += Time.deltaTime;

        if (currentState == SpearState.SHOOTING && rb.velocity.magnitude < 0.2f)
            SwitchStateTo(SpearState.RETURNING);

        switch (currentState)
        {
            case SpearState.AIMING:
                timer += Time.deltaTime;
                break;

            case SpearState.SHOOTING:
                if (rb.velocity.magnitude < 0.1f)
                {
                    SwitchStateTo(SpearState.RETURNING);
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (currentState == SpearState.RETURNING)
        {
            if ((transform.position - targetLocation.position).magnitude < 0.3f)
            {
                SwitchStateTo(SpearState.HOLDING);
                return;
            }
            AlignEndWithTargetPosition();
            rb.MovePosition(Vector3.MoveTowards(rb.position, targetLocation.position, returningSpeed * Time.fixedDeltaTime));
        }
    }

    private void LateUpdate()
    {
        if (currentState == SpearState.HOLDING || currentState == SpearState.AIMING)
        {
            AlignWithMouse();
            MoveToHoldPosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fish fish = collision.gameObject.GetComponent<Fish>();
        if (fish != null)
        {
            DecreaseSpeed();
            OnFishKilled.Raise(fish.fishData);
            OnAFishDied.Raise(); 
            fish.DestroySelf(); 
        }
    }

    private void AlignWithMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 direction = mousePosition - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
    }

    private void AlignEndWithTargetPosition()
    {
        Vector3 position = targetLocation.position;
        position.z = 0f;
        Vector3 direction = position - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

        transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
    }

    private void MoveToHoldPosition()
    {
        transform.position = targetLocation.position;
    }

    private void OnHoldPerformed(InputAction.CallbackContext context)
    {
        if (currentState == SpearState.HOLDING)
            SwitchStateTo(SpearState.AIMING);
        else
            Debug.LogWarning("Can't cast Spear again until it gets back in hand");
    }

    private void OnHoldCanceled(InputAction.CallbackContext context)
    {
        if (currentState == SpearState.AIMING)
        {
            SwitchStateTo(SpearState.SHOOTING);
            ShootSpear();
        }
    }

    private void SwitchStateTo(SpearState newState)
    {
        if (newState == currentState)
            return;

        switch (newState)
        {
            case SpearState.AIMING:
                timer = 0f;
                break;
            case SpearState.HOLDING:
                rope.HideRope(); 
                break; 
        }

        currentState = newState;
    }

    private void ShootSpear()
    {
        if (timer < 0.1f)
        {
            SwitchStateTo(SpearState.HOLDING);
            Debug.LogWarning("Hold time too short");
            return;
        }
        // Mathf.Clamp(GetCurrentHoldPercentage(), 0.3f, 1f) *
        rb.AddForce(maxShootForce * transform.up, ForceMode2D.Impulse);
        rope.ShowRope(); 
    }

    private float GetCurrentHoldPercentage()
    {
        return Mathf.Clamp01(timer / maxHoldTime);
    }

    private void DecreaseSpeed()
    {
        rb.velocity *= 0.5f;
    }
}

public enum SpearState
{
    HOLDING,
    AIMING,
    SHOOTING,
    RETURNING
}