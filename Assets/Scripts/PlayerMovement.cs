using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float horizontalSpeed, jumpForce;

    [SerializeField]
    private float regularGravity, fallGravity;


    private PlatformerActions input;
    private Rigidbody2D rb;
    private float horizontalMove;

    private void Awake()
    {
        input = new PlatformerActions();
    }

    private void Start()
    {
        horizontalMove = 0f;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = regularGravity;
    }

    private void Update()
    {
        rb.velocity = new Vector2(horizontalMove * horizontalSpeed, rb.velocity.y);
        if (rb.velocity.y < 0f)
            rb.gravityScale = fallGravity;
        else
            rb.gravityScale = regularGravity;
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Move.performed += OnMovePerformed;
        input.Player.Move.canceled += OnMoveCanceled;
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpCanceled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Move.performed -= OnMovePerformed;
        input.Player.Move.canceled -= OnMoveCanceled;
        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Jump.canceled -= OnJumpCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        horizontalMove = context.ReadValue<float>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        horizontalMove = 0f;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        rb.gravityScale = regularGravity;
        // Prevent falling velocity from reducing jump height
        if (rb.velocity.y < 0f)
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        // Variable jump height
        if (rb.velocity.y > 0f)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
    }
}
