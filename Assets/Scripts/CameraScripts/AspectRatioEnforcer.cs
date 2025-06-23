using UnityEngine;

/// <summary>
/// Enforces a specific aspect ratio by adding letterboxing or pillarboxing to the camera view.
/// </summary>
public class AspectRatioEnforcer : MonoBehaviour
{
    public float targetAspect = 16f / 9f; // Desired aspect ratio (width/height)

    // Sets the camera viewport to maintain the target aspect ratio
    void Start()
    {
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera cam = GetComponent<Camera>();

        if (scaleHeight < 1.0f)
        {
            // Add letterbox (black bars top and bottom)
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            cam.rect = rect;
        }
        else
        {
            // Add pillarbox (black bars left and right)
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            cam.rect = rect;
        }
    }
}