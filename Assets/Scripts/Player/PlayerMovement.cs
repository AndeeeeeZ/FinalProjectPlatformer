using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private bool debugging;

    [SerializeField] private ParticleSystem particles;

    [SerializeField] private float horizontalSpeed, jumpForce;

    [SerializeField] private float regularGravity, fallGravity;

    [SerializeField, Min(0)] private int maxExtraJumpAmount;
    [SerializeField] private Animator animator;

    private PlatformerActions input;
    private Rigidbody2D rb;
    private float horizontalMove;
    private int currentJumpAmount;
    private PlayerState currentState;

    private void Awake()
    {
        input = new PlatformerActions();
    }

    private void Start()
    {
        horizontalMove = 0f;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = regularGravity;
        SwitchStateTo(PlayerState.FALLING);
        ResetJumpAmount();
    }

    private void Update()
    {
        rb.velocity = new Vector2(horizontalMove * horizontalSpeed, rb.velocity.y);

        if (currentState == PlayerState.JUMPING && rb.velocity.y < -0.01f)
            SwitchStateTo(PlayerState.FALLING);
        Debug.Log(currentState); 
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
        animator.SetFloat("Walk", horizontalMove); 
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        horizontalMove = 0f;
        animator.SetFloat("Walk", horizontalMove); 
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (currentState == PlayerState.GROUNDED || currentJumpAmount > 0)
        {
            rb.gravityScale = regularGravity;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            if (currentState != PlayerState.GROUNDED)
                currentJumpAmount--;
            SwitchStateTo(PlayerState.JUMPING);
        }
        else
        {
            if (debugging)
                Debug.LogWarning("Player have already jumped");
        }
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        if (currentState == PlayerState.JUMPING)
            SwitchStateTo(PlayerState.FALLING);
    }

    public void OnTouchingFloor()
    {
        if (currentState != PlayerState.GROUNDED)
        {
            SwitchStateTo(PlayerState.GROUNDED);
            ResetJumpAmount();
        }
    }

    private void ResetJumpAmount()
    {
        currentJumpAmount = maxExtraJumpAmount;
    }

    private void SwitchStateTo(PlayerState newState)
    {
        if (newState == currentState)
            return;

        switch (newState)
        {
            case PlayerState.JUMPING:
                particles.Play();
                animator.SetTrigger("Jumping");
                break;
            case PlayerState.FALLING:
                rb.gravityScale = fallGravity;
                particles.Stop();
                animator.SetTrigger("Falling");
                break;
            case PlayerState.GROUNDED:
                rb.gravityScale = regularGravity;
                animator.SetTrigger("Grounded"); 
                break;
        }
        currentState = newState;
    }
}

public enum PlayerState
{
    JUMPING,
    FALLING,
    GROUNDED
}