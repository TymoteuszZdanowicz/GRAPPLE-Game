using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour
{
    private PlayerInput keyPress;
    private InputAction jump;
    private InputAction direction;

    private Rigidbody2D rb2D;
    private float thrust = 15f;
    private float jumpHoldTime = 0f;
    private float maxHoldTime = 1f;
    private bool isGrounded = true; // Assume grounded initially
    private bool isJumping = false;

    private float lastHorizontalInput = 0f;
    private float horizontalInputBufferTime = 0.2f;
    private float horizontalInputBufferTimer = 0f; 

    void Awake()
    {
        keyPress = new PlayerInput();
        jump = keyPress.Input.Jump;
        direction = keyPress.Input.Direction;
    }

    void OnEnable()
    {
        jump.Enable();
        direction.Enable();
    }

    void OnDisable()
    {
        jump.Disable();
        direction.Disable();
    }

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Update horizontal input buffer
        float currentHorizontalInput = direction.ReadValue<Vector2>().x;
        if (Mathf.Abs(currentHorizontalInput) > 0.01f)
        {
            lastHorizontalInput = currentHorizontalInput;
            horizontalInputBufferTimer = horizontalInputBufferTime;
        }
        else
        {
            horizontalInputBufferTimer -= Time.deltaTime;
            if (horizontalInputBufferTimer <= 0f)
            {
                lastHorizontalInput = 0f;
            }
        }

        if (jump.ReadValue<float>() > 0 && isGrounded)
        {
            isJumping = true;
            jumpHoldTime += Time.deltaTime;

            if (jumpHoldTime >= maxHoldTime)
            {
                jumpHoldTime = maxHoldTime;
                PerformJump();
                isJumping = false;
                jumpHoldTime = 0f;
            }
        }
        else if (isJumping)
        {
            PerformJump();
            isJumping = false;
            jumpHoldTime = 0f;
        }
    }

    private void PerformJump()
    {
        if (isGrounded)
        {
            float jumpStrength = (jumpHoldTime / maxHoldTime) * thrust;

            Vector2 jumpForce = new Vector2(lastHorizontalInput, 1).normalized * jumpStrength;

            rb2D.AddForce(jumpForce, ForceMode2D.Impulse);
            Debug.Log($"Jump with strength: {jumpStrength}, direction: {lastHorizontalInput}");

            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            // Reset grappling hook usage when grounded
            GrapplingHookScript grapplingHook = GetComponent<GrapplingHookScript>();
            if (grapplingHook != null)
            {
                grapplingHook.ResetGrappleUsage();
            }
        }
    }

}
