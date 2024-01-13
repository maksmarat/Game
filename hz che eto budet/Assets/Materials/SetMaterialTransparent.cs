using UnityEngine;

public class SetMaterialTransparent : MonoBehaviour
{
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            Material material = renderer.material;

            material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        }
    }
}
