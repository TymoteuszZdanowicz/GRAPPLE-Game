using UnityEngine;

/// <summary>
/// Handles grappling hook mechanics for both left and right hooks.
/// </summary>
public class GrapplingHookScript : MonoBehaviour
{
    [Header("Right Grapple Settings")]
    public Transform rightFirePoint;           // Fire point for right hook
    public LineRenderer rightLineRenderer;     // Line renderer for right hook

    [Header("Left Grapple Settings")]
    public Transform leftFirePoint;            // Fire point for left hook
    public LineRenderer leftLineRenderer;      // Line renderer for left hook

    [Header("Grapple Layer")]
    public LayerMask grappleLayer;             // Layer mask for grapple targets

    public float maxGrappleDistance = 15f;     // Max grapple distance
    public float pullForce = 200f;             // Force pulling player to grapple
    public float swingDamping = 0.5f;          // Damping for swinging motion

    private Vector2 rightGrapplePoint;         // Target point for right hook
    private Vector2 leftGrapplePoint;          // Target point for left hook
    private bool isRightGrappling = false;     // Is right hook active
    private bool isLeftGrappling = false;      // Is left hook active
    private bool hasRightGrappled = false;     // Has right hook been fired
    private bool hasLeftGrappled = false;      // Has left hook been fired
    private Rigidbody2D rb;                    // Player Rigidbody2D

    private bool isAirborne = false;           // Is player airborne

    // Initialize Rigidbody2D reference
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Handles input for firing and releasing grappling hooks
    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        if (Input.GetMouseButtonDown(1) && !hasRightGrappled)
        {
            FireGrapplingHook(rightFirePoint, ref rightGrapplePoint, ref isRightGrappling, rightLineRenderer);
            hasRightGrappled = true;
        }
        if (Input.GetMouseButtonDown(0) && !hasLeftGrappled)
        {
            FireGrapplingHook(leftFirePoint, ref leftGrapplePoint, ref isLeftGrappling, leftLineRenderer);
            hasLeftGrappled = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            ReleaseGrapplingHook(ref isRightGrappling, rightLineRenderer);
        }
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseGrapplingHook(ref isLeftGrappling, leftLineRenderer);
        }
    }

    // Handles physics-based swinging for active grappling hooks
    void FixedUpdate()
    {
        if (isRightGrappling)
        {
            SwingPlayer(rightGrapplePoint, rightLineRenderer, rightFirePoint, ref isRightGrappling);
        }
        if (isLeftGrappling)
        {
            SwingPlayer(leftGrapplePoint, leftLineRenderer, leftFirePoint, ref isLeftGrappling);
        }
    }

    // Fires a grappling hook towards the mouse position
    void FireGrapplingHook(Transform firePoint, ref Vector2 grapplePoint, ref bool isGrappling, LineRenderer lineRenderer)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)firePoint.position;

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, maxGrappleDistance, grappleLayer);
        if (hit.collider != null)
        {
            grapplePoint = hit.point;
            isGrappling = true;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
    }

    // Applies force to swing the player towards the grapple point
    void SwingPlayer(Vector2 grapplePoint, LineRenderer lineRenderer, Transform firePoint, ref bool isGrappling)
    {
        Vector2 directionToGrapple = (grapplePoint - (Vector2)transform.position).normalized;
        Vector2 pullForceVector = directionToGrapple * pullForce * Time.deltaTime;
        rb.AddForce(pullForceVector);

        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, swingDamping * Time.deltaTime);

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, grapplePoint);

        if (Vector2.Distance(transform.position, grapplePoint) > maxGrappleDistance)
        {
            ReleaseGrapplingHook(ref isGrappling, lineRenderer);
        }
    }

    // Releases the grappling hook and hides the line
    void ReleaseGrapplingHook(ref bool isGrappling, LineRenderer lineRenderer)
    {
        isGrappling = false;
        lineRenderer.positionCount = 0;
    }

    // Resets grapple usage flags (for both hooks)
    public void ResetGrappleUsage()
    {
        hasRightGrappled = false;
        hasLeftGrappled = false;
    }

    // Called when player lands, resets airborne and grapple usage
    public void OnLand()
    {
        isAirborne = false;
        ResetGrappleUsage();
    }

    // Called when player jumps, sets airborne state
    public void OnJump()
    {
        isAirborne = true;
    }
}