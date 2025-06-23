using UnityEngine;
using System.Collections;

/// <summary>
/// Handles automatic upright rotation and small jump if the player is not upright.
/// </summary>
public class UprightRotationHandler : MonoBehaviour
{
    private Rigidbody2D rb2D;                  // Reference to Rigidbody2D
    public float UprightThreshold = 1f;        // Time before checking upright
    public float UprightRotationThreshold = 5f;// Allowed angle from upright (degrees)
    public float SmallJumpForce = 5f;          // Force applied for small jump
    public float RotationSpeed = 200f;         // Speed of upright rotation

    private float uprightCheckTimer = 0f;      // Timer for upright check
    private bool isRotatingToUpright = false;  // Is currently rotating to upright

    // Initialize Rigidbody2D reference
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Checks if player should be rotated upright
    void Update()
    {
        if (rb2D.linearVelocity.magnitude < 0.1f && !isRotatingToUpright)
        {
            uprightCheckTimer += Time.deltaTime;
            if (uprightCheckTimer >= UprightThreshold && !IsUpright())
            {
                StartCoroutine(PerformUprightJump());
                uprightCheckTimer = 0f;
            }
        }
        else
        {
            uprightCheckTimer = 0f;
        }
    }

    // Performs a small jump and rotates the player upright
    private IEnumerator PerformUprightJump()
    {
        isRotatingToUpright = true;

        // Apply a small vertical jump force
        Vector2 jumpForce = new Vector2(0, SmallJumpForce);
        rb2D.AddForce(jumpForce, ForceMode2D.Impulse);

        // Gradually rotate the player to an upright position
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRotation;
        isRotatingToUpright = false;
    }

    // Checks if the player is upright within the allowed threshold
    private bool IsUpright()
    {
        float zRotation = transform.rotation.eulerAngles.z;
        zRotation = (zRotation > 180) ? zRotation - 360 : zRotation; // Normalize to -180 to 180
        return Mathf.Abs(zRotation) <= UprightRotationThreshold;
    }
}