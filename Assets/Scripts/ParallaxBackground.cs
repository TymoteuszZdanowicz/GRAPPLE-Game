using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform;
        public float parallaxEffectMultiplier;
        public float startParallaxAtY = float.NegativeInfinity; // domyœlnie zawsze aktywne
    }

    public ParallaxLayer[] layers;
    public Transform playerTransform; // Przypisz gracza w Inspectorze

    private Vector3 previousCameraPosition;

    void Start()
    {
        previousCameraPosition = Camera.main.transform.position;
    }

    void LateUpdate()
    {
        Vector3 cameraDelta = Camera.main.transform.position - previousCameraPosition;

        foreach (var layer in layers)
        {
            // SprawdŸ, czy gracz przekroczy³ próg Y dla tej warstwy
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