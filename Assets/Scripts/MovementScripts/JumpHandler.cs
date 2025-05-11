using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpHandler : MonoBehaviour
{
    private Rigidbody2D rb2D;

    /// Jump settings
    public float Thrust = 15f;
    public float MaxHoldTime = 1f;

    private float jumpHoldTime = 0f;
    private bool isGrounded = true;

    private float lastHorizontalInput = 0f;
    private float horizontalInputBufferTime = 0.2f;
    private float horizontalInputBufferTimer = 0f;


    private GrapplingHookScript grapplingHookScript;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        grapplingHookScript = GetComponent<GrapplingHookScript>();
    }

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

    /// Perform the jump
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
