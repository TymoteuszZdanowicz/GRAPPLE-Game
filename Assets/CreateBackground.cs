using UnityEngine;

public class CreateBackground : MonoBehaviour
{
    public Material backgroundMaterial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

        MeshRenderer meshRenderer = quad.GetComponent<MeshRenderer>();
        meshRenderer.material = backgroundMaterial;

        meshRenderer.sortingLayerName = "Background";
        meshRenderer.sortingOrder = 0;

        quad.transform.localScale = new Vector3(10f, 10f, 1f);

        quad.transform.position = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
