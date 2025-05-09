using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpHandler : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float Thrust = 15f;
    public float MaxHoldTime = 1f;

    private float jumpHoldTime = 0f;
    private bool isGrounded = true;

    private float lastHorizontalInput = 0f;
    private float horizontalInputBufferTime = 0.2f;
    private float horizontalInputBufferTimer = 0f;

    public bool IsGrounded => isGrounded; // Expose isGrounded for other scripts
    public float LastHorizontalInput => lastHorizontalInput; // Expose buffered input

    // Dodano referencjê do GrapplingHookScript
    private GrapplingHookScript grapplingHookScript;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        grapplingHookScript = GetComponent<GrapplingHookScript>(); // Pobierz referencjê do GrapplingHookScript
    }

    public void HandleJump(bool jumpPressed, float horizontalInput)
    {
        // Update horizontal input buffer
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

    private void PerformJump()
    {
        float jumpStrength = (jumpHoldTime / MaxHoldTime) * Thrust;
        Vector2 jumpForce = new Vector2(lastHorizontalInput, 1).normalized * jumpStrength;
        rb2D.AddForce(jumpForce, ForceMode2D.Impulse);
        isGrounded = false;

        // Wywo³aj OnJump z GrapplingHookScript
        if (grapplingHookScript != null)
        {
            grapplingHookScript.OnJump();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            // Wywo³aj OnLand z GrapplingHookScript
            if (grapplingHookScript != null)
            {
                grapplingHookScript.OnLand();
            }
        }
    }
}
