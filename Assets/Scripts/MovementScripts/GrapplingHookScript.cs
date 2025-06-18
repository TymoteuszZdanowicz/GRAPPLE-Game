using UnityEngine;

public class GrapplingHookScript : MonoBehaviour
{
    [Header("Right Grapple Settings")]
    public Transform rightFirePoint;
    public LineRenderer rightLineRenderer;
    public float maxGrappleDistance = 15f;
    public float pullForce = 200f;
    public float swingDamping = 0.5f;

    [Header("Left Grapple Settings")]
    public Transform leftFirePoint;
    public LineRenderer leftLineRenderer;

    [Header("Grapple Layer")]
    public LayerMask grappleLayer;

    private Vector2 rightGrapplePoint;
    private Vector2 leftGrapplePoint;
    private bool isRightGrappling = false;
    private bool isLeftGrappling = false;
    private bool hasRightGrappled = false;
    private bool hasLeftGrappled = false;
    private Rigidbody2D rb;

    private bool isAirborne = false;
    //private float groundedBufferTime = 0f;
    //private float maxGroundedBufferTime = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Fire the right grappling hook
        if (Input.GetMouseButtonDown(1) && !hasRightGrappled)
        {
            FireGrapplingHook(rightFirePoint, ref rightGrapplePoint, ref isRightGrappling, rightLineRenderer);
            hasRightGrappled = true;
        }

        // Fire the left grappling hook
        if (Input.GetMouseButtonDown(0) && !hasLeftGrappled)
        {
            FireGrapplingHook(leftFirePoint, ref leftGrapplePoint, ref isLeftGrappling, leftLineRenderer);
            hasLeftGrappled = true;
        }

        // Release the right grappling hook
        if (Input.GetMouseButtonUp(1))
        {
            ReleaseGrapplingHook(ref isRightGrappling, rightLineRenderer);
        }

        // Release the left grappling hook
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseGrapplingHook(ref isLeftGrappling, leftLineRenderer);
        }

        // Handle swinging for the right grappling hook
        if (isRightGrappling)
        {
            SwingPlayer(rightGrapplePoint, rightLineRenderer, rightFirePoint, ref isRightGrappling);
        }

        // Handle swinging for the left grappling hook
        if (isLeftGrappling)
        {
            SwingPlayer(leftGrapplePoint, leftLineRenderer, leftFirePoint, ref isLeftGrappling);
        }
    }


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

    void ReleaseGrapplingHook(ref bool isGrappling, LineRenderer lineRenderer)
    {
        isGrappling = false;
        lineRenderer.positionCount = 0;
    }

    public void ResetGrappleUsage()
    {
        hasRightGrappled = false;
        hasLeftGrappled = false;
    }

    public void OnLand()
    {
        isAirborne = false;
        ResetGrappleUsage();
    }

    public void OnJump()
    {
        isAirborne = true;
    }
}
