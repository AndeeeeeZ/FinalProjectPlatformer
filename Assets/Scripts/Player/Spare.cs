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

    private void LateUpdate()
    {
        //if (currentState == SpareState.HOLDING || currentState == SpareState.AIMING)
        AlignWithMouse();
        MoveToHoldPosition(); 
    }

    private void AlignWithMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 direction = mousePosition - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
    }

    private void MoveToHoldPosition()
    {
        transform.position = targetLocation.position;
    }

    private void OnHoldPerformed(InputAction.CallbackContext context)
    {
        SwitchStateTo(SpareState.AIMING);
    }

    private void OnHoldCanceled(InputAction.CallbackContext context)
    {
        SwitchStateTo(SpareState.SHOOTING);
    }

    private void SwitchStateTo(SpareState newState)
    {
        if (newState == currentState)
            return;

        currentState = newState;
    }
}

public enum SpareState
{
    HOLDING,
    AIMING,
    SHOOTING,
    RETURNING
}