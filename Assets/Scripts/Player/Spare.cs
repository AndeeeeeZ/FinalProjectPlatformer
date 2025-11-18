using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spare : MonoBehaviour
{
    [SerializeField]
    private Transform targetLocation;
    [SerializeField]
    private float maxHoldTime;
    [SerializeField]
    private float maxShootForce;
    [SerializeField]
    private float returningSpeed;

    private PlatformerActions input;
    private Rigidbody2D rb;

    private SpareState currentState;
    private float timer;

    private void Awake()
    {
        input = new PlatformerActions();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentState = SpareState.HOLDING;
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
        if (currentState == SpareState.AIMING)
            timer += Time.deltaTime;

        if (currentState == SpareState.SHOOTING && rb.velocity.magnitude < 0.2f)
            SwitchStateTo(SpareState.RETURNING);

        switch (currentState)
        {
            case SpareState.AIMING:
                timer += Time.deltaTime;
                break;

            case SpareState.SHOOTING:
                if (rb.velocity.magnitude < 0.1f)
                {
                    SwitchStateTo(SpareState.RETURNING);
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (currentState == SpareState.RETURNING)
        {
            if ((transform.position - targetLocation.position).magnitude < 0.3f)
            {
                SwitchStateTo(SpareState.HOLDING);
                return;
            }
            AlignEndWithTargetPosition();
            rb.MovePosition(Vector3.MoveTowards(rb.position, targetLocation.position, returningSpeed * Time.fixedDeltaTime));
        }
    }

    private void LateUpdate()
    {
        if (currentState == SpareState.HOLDING || currentState == SpareState.AIMING)
        {
            AlignWithMouse();
            MoveToHoldPosition();
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
        if (currentState == SpareState.HOLDING)
            SwitchStateTo(SpareState.AIMING);
        else
            Debug.LogWarning("Can't cast spare again until it gets back in hand");
    }

    private void OnHoldCanceled(InputAction.CallbackContext context)
    {
        if (currentState == SpareState.AIMING)
        {
            SwitchStateTo(SpareState.SHOOTING);
            ShootSpare();
        }
    }

    private void SwitchStateTo(SpareState newState)
    {
        if (newState == currentState)
            return;

        switch (newState)
        {
            case SpareState.AIMING:
                timer = 0f;
                break;
        }

        currentState = newState;
    }

    private void ShootSpare()
    {
        if (timer < 0.1f)
        {
            SwitchStateTo(SpareState.HOLDING);
            Debug.LogWarning("Hold time too short");
            return;
        }
        rb.AddForce(Mathf.Clamp(GetCurrentHoldPercentage(), 0.3f, 1f) * maxShootForce * transform.up, ForceMode2D.Impulse);
    }

    private float GetCurrentHoldPercentage()
    {
        return Mathf.Clamp01(timer / maxHoldTime); 
    }
}

public enum SpareState
{
    HOLDING,
    AIMING,
    SHOOTING,
    RETURNING
}