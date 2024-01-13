using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GK
{
    public class PowerBox : MonoBehaviour
    {

        public string powerLinesTag = "PowerLines";

        [Header("Prefabs")]
        public GameObject boundingBox;
        private GameObject rock;

        [Header("Variables")]

        public float fixedRadius = 5f;
        public int numberOfPolygons = 200;
        public float fillDistance = 0.1f;


        void Start()
        {
        }

        public void CalculateBoundingBox()
        {
            if (rock != null)
                Destroy(rock);



            // Calculate center and size of the bounding box


            var calc = new ConvexHullCalculator();
            var verts = new List<Vector3>();
            var tris = new List<int>();
            var normals = new List<Vector3>();
            var points = new List<Vector3>();

            GameObject[] powerLines = GameObject.FindGameObjectsWithTag(powerLinesTag);
            Vector3 minBounds = powerLines[0].transform.position;
            Vector3 maxBounds = powerLines[0].transform.position;

            foreach (GameObject powerLine in powerLines)
            {
                Vector3 position = powerLine.transform.position;

                for (int i = 0; i < numberOfPolygons; i++)
                {
                    points.Add(Random.insideUnitSphere);
                }

                // Update min and max bounds for each dimension
                minBounds.x = Mathf.Min(minBounds.x, position.x);
                minBounds.y = Mathf.Min(minBounds.y, position.y);
                minBounds.z = Mathf.Min(minBounds.z, position.z);

                maxBounds.x = Mathf.Max(maxBounds.x, position.x);
                maxBounds.y = Mathf.Max(maxBounds.y, position.y);
                maxBounds.z = Mathf.Max(maxBounds.z, position.z);

            }

            Vector3 center = (minBounds + maxBounds) / 2f;
            Vector3 size = maxBounds - minBounds;
            size += Vector3.one * fixedRadius;
            size.y -= fixedRadius / 2f;
            calc.GenerateHull(points, true, ref verts, ref tris, ref normals);

            rock = Instantiate(boundingBox);
            //rock.transform.SetParent(transform, false);
            rock.transform.position = center;
            rock.transform.localScale = size;

            var mesh = new Mesh();
            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.SetNormals(normals);

            rock.GetComponent<MeshFilter>().sharedMesh = mesh;
            rock.GetComponent<MeshCollider>().sharedMesh = mesh;
        }
    }
}