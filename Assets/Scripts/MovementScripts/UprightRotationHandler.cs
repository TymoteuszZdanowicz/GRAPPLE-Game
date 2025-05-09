using UnityEngine;
using System.Collections;

public class UprightRotationHandler : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float UprightThreshold = 1f;
    public float UprightRotationThreshold = 5f;
    public float SmallJumpForce = 5f;
    public float RotationSpeed = 200f;

    private float uprightCheckTimer = 0f;
    private bool isRotatingToUpright = false;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

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

    private bool IsUpright()
    {
        float zRotation = transform.rotation.eulerAngles.z;
        zRotation = (zRotation > 180) ? zRotation - 360 : zRotation; // Normalize to -180 to 180
        return Mathf.Abs(zRotation) <= UprightRotationThreshold;
    }
}
