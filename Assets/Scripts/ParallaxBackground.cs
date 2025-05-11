using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform;
        public float parallaxEffectMultiplier;
    }

    public ParallaxLayer[] layers;
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
            Vector3 newPosition = layer.layerTransform.position;
            //newPosition.x += cameraDelta.x * layer.parallaxEffectMultiplier;
            newPosition.y += cameraDelta.y * layer.parallaxEffectMultiplier;
            layer.layerTransform.position = newPosition;
        }

        previousCameraPosition = Camera.main.transform.position;
    }
}
