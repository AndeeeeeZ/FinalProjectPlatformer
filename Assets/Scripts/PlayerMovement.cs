using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float horizontalSpeed, jumpForce;
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
    }

    private void Update()
    {
        rb.velocity = new Vector2(horizontalMove * horizontalSpeed, rb.velocity.y); 
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Move.performed += OnMovePerformed;
        input.Player.Move.canceled += OnMoveCanceled;
        input.Player.Jump.performed += OnJump; 
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Move.performed -= OnMovePerformed;
        input.Player.Move.canceled -= OnMoveCanceled;
        input.Player.Jump.performed -= OnJump; 
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        horizontalMove = context.ReadValue<float>();
        Debug.Log(horizontalMove);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        horizontalMove = 0f;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); 
    }
}
