using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform;           // Reference to the background layer
        public float parallaxEffectMultiplier;     // Parallax movement strength
        public float startParallaxAtY = float.NegativeInfinity; // Y threshold to start parallax
    }

    public ParallaxLayer[] layers;                 // Array of parallax layers
    public Transform playerTransform;              // Reference to player (for Y threshold)

    private Vector3 previousCameraPosition;        // Last camera position

    // Initialize previous camera position
    void Start()
    {
        previousCameraPosition = Camera.main.transform.position;
    }

    // Move layers based on camera movement and player Y position
    void LateUpdate()
    {
        Vector3 cameraDelta = Camera.main.transform.position - previousCameraPosition;

        foreach (var layer in layers)
        {
            if (playerTransform != null && playerTransform.position.y >= layer.startParallaxAtY)
            {
                Vector3 newPosition = layer.layerTransform.position;
                newPosition.y += cameraDelta.y * layer.parallaxEffectMultiplier;
                layer.layerTransform.position = newPosition;
            }
        }

        previousCameraPosition = Camera.main.transform.position;
    }
}