using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LodGroupadd : MonoBehaviour
{
    void Start()
    {
        if (GetComponent<LODGroup>() == null)
        {
            LODGroup lodGroup = gameObject.AddComponent<LODGroup>();

            LOD lod0 = new LOD(0.05f, GetComponentsInChildren<Renderer>());

            lodGroup.SetLODs(new LOD[] { lod0 });

            lodGroup.RecalculateBounds();
        }
    }
}
