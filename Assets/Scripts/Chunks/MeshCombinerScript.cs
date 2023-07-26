using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombinerScript : MonoBehaviour
{
    public void CombineMesh()
    {
        MeshCombiner meshCombiner = gameObject.AddComponent<MeshCombiner>();
        meshCombiner.CreateMultiMaterialMesh = true;
        //meshCombiner.DeactivateCombinedChildrenMeshRenderers = true;
        meshCombiner.DestroyCombinedChildren = true;

        meshCombiner.CombineMeshes(false);
        gameObject.AddComponent<MeshCollider>();
    }
}
