using UnityEngine;
using System.Collections.Generic;

public class PowerLineBoundingBox : MonoBehaviour
{
    public string powerLinesTag = "PowerLines";
    public float boundingBoxRadius = 5f;

    public void CalculateBoundingBox()
    {
        GameObject[] powerLines = GameObject.FindGameObjectsWithTag(powerLinesTag);
        List<MeshFilter> meshFilters = new List<MeshFilter>();

        foreach (GameObject powerLine in powerLines)
        {
            MeshFilter meshFilter = powerLine.GetComponent<MeshFilter>();

            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                meshFilters.Add(meshFilter);
            }
        }

        if (meshFilters.Count > 0)
        {
            CombineMeshes(meshFilters);
        }
    }

    void CombineMeshes(List<MeshFilter> meshFilters)
    {
        List<CombineInstance> combineInstances = new List<CombineInstance>();

        foreach (MeshFilter meshFilter in meshFilters)
        {
            Vector3 powerLinePosition = meshFilter.transform.position;
            Vector3 powerLineSize = meshFilter.sharedMesh.bounds.size;
            Vector3 boundingBoxSize = new Vector3(powerLineSize.x + boundingBoxRadius * 2f,
                                                  powerLineSize.y + boundingBoxRadius * 2f,
                                                  powerLineSize.z + boundingBoxRadius * 2f);

            CombineInstance combineInstance = new CombineInstance();
            combineInstance.mesh = CreateBoundingBoxMesh(boundingBoxSize);
            combineInstance.transform = Matrix4x4.TRS(powerLinePosition, meshFilter.transform.rotation, Vector3.one);

            combineInstances.Add(combineInstance);
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineInstances.ToArray(), true, true);

        GameObject combinedObject = new GameObject("CombinedMesh");
        combinedObject.transform.position = Vector3.zero;
        combinedObject.transform.rotation = Quaternion.identity;
        combinedObject.transform.localScale = Vector3.one;

        MeshFilter combinedMeshFilter = combinedObject.AddComponent<MeshFilter>();
        combinedMeshFilter.mesh = combinedMesh;

        MeshRenderer combinedMeshRenderer = combinedObject.AddComponent<MeshRenderer>();
        combinedMeshRenderer.material = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial; // Assuming all original objects have the same material
    }

    Mesh CreateBoundingBoxMesh(Vector3 size)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-size.x / 2f, -size.y / 2f, -size.z / 2f),
            new Vector3(-size.x / 2f, size.y / 2f, -size.z / 2f),
            new Vector3(size.x / 2f, size.y / 2f, -size.z / 2f),
            new Vector3(size.x / 2f, -size.y / 2f, -size.z / 2f),
            new Vector3(-size.x / 2f, -size.y / 2f, size.z / 2f),
            new Vector3(-size.x / 2f, size.y / 2f, size.z / 2f),
            new Vector3(size.x / 2f, size.y / 2f, size.z / 2f),
            new Vector3(size.x / 2f, -size.y / 2f, size.z / 2f),
        };

        int[] triangles = new int[]
        {
            0, 1, 2, 0, 2, 3, // Front face
            4, 5, 6, 4, 6, 7, // Back face
            0, 3, 7, 0, 7, 4, // Left face
            1, 2, 6, 1, 6, 5, // Right face
            0, 1, 5, 0, 5, 4, // Top face
            2, 3, 7, 2, 7, 6  // Bottom face
        };

        Vector3[] normals = new Vector3[]
        {
            Vector3.back, Vector3.back, Vector3.back, Vector3.back, // Front face
            Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward, // Back face
            Vector3.left, Vector3.left, Vector3.left, Vector3.left, // Left face
            Vector3.right, Vector3.right, Vector3.right, Vector3.right, // Right face
            Vector3.up, Vector3.up, Vector3.up, Vector3.up, // Top face
            Vector3.down, Vector3.down, Vector3.down, Vector3.down  // Bottom face
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        return mesh;
    }
}
