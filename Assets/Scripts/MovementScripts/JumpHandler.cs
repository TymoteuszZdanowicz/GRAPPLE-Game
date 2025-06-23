using UnityEngine;

/// <summary>
/// Handles jumping logic, including jump buffering and interaction with grappling hook.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class JumpHandler : MonoBehaviour
{
    private Rigidbody2D rb2D;                  // Reference to Rigidbody2D

    // Jump settings
    public float Thrust = 15f;                 // Jump force
    public float MaxHoldTime = 1f;             // Max time jump can be held

    private float jumpHoldTime = 0f;           // Current jump hold time
    private bool isGrounded = true;            // Is player on the ground

    private float lastHorizontalInput = 0f;    // Last horizontal input for jump direction
    private float horizontalInputBufferTime = 0.2f; // Buffer time for horizontal input
    private float horizontalInputBufferTimer = 0f;  // Timer for input buffering

    private GrapplingHookScript grapplingHookScript; // Reference to grappling hook

    // Initialize references
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        grapplingHookScript = GetComponent<GrapplingHookScript>();
    }

    // Handles jump input and buffering.
    public void HandleJump(bool jumpPressed, float horizontalInput)
    {
        // Handle horizontal input buffering
        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            lastHorizontalInput = horizontalInput;
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

        // Handle jump input
        if (jumpPressed && isGrounded)
        {
            jumpHoldTime += Time.deltaTime;

            if (jumpHoldTime >= MaxHoldTime)
            {
                PerformJump();
                jumpHoldTime = 0f;
            }
        }
        else if (jumpHoldTime > 0)
        {
            PerformJump();
            jumpHoldTime = 0f;
        }
    }

    // Performs the jump and notifies grappling hook.
    private void PerformJump()
    {
        float jumpStrength = (jumpHoldTime / MaxHoldTime) * Thrust;
        Vector2 jumpForce = new Vector2(lastHorizontalInput, 1).normalized * jumpStrength;
        rb2D.AddForce(jumpForce, ForceMode2D.Impulse);
        isGrounded = false;

        if (grapplingHookScript != null)
        {
            grapplingHookScript.OnJump();
        }
    }

    // Detects landing and notifies grappling hook
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            if (grapplingHookScript != null)
            {
                grapplingHookScript.OnLand();
            }
        }
    }
}