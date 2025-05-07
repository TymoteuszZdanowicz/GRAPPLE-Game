using UnityEngine;

public class GrapplingHookScript : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public LayerMask grappleLayer;
    public Transform firePoint;
    public float maxGrappleDistance = 15f;
    public float pullForce = 20f;
    public float swingDamping = 0.5f;

    private Vector2 grapplePoint;
    private bool isGrappling = false;
    private bool hasGrappled = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hasGrappled)
        {
            FireGrapplingHook();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseGrapplingHook();
        }

        if (isGrappling)
        {
            SwingPlayer();
        }
    }

    void FireGrapplingHook()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)firePoint.position;

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, maxGrappleDistance, grappleLayer);
        if (hit.collider != null)
        {
            grapplePoint = hit.point;
            isGrappling = true;
            hasGrappled = true;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
    }

    void SwingPlayer()
    {
        Vector2 directionToGrapple = (grapplePoint - (Vector2)transform.position).normalized;

        Vector2 pullForceVector = directionToGrapple * pullForce;
        rb.AddForce(pullForceVector);

        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, swingDamping * Time.deltaTime);

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, grapplePoint);

        if (Vector2.Distance(transform.position, grapplePoint) < 0.5f)
        {
            ReleaseGrapplingHook();
        }
    }

    void ReleaseGrapplingHook()
    {
        isGrappling = false;
        lineRenderer.positionCount = 0;
    }

    public void ResetGrappleUsage()
    {
        hasGrappled = false;
    }
}
