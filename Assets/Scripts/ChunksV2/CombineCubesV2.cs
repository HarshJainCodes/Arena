using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineCubesV2 : MonoBehaviour
{
    /// <summary>
    /// This is the gameobject that is parent to all the walls that are created.
    /// It contains children names Floor0, Floor1, .... Floor 5
    /// </summary>
    [SerializeField] private GameObject WallsHolder;

    /// <summary>
    /// This will combine all the cube segments by iterating over the <see cref="WallsHolder"/> gameobject and calling the 
    /// <see cref="MeshCombinerScript.CombineMesh"/> function
    /// </summary>
    public void CombineCubeSegments()
    {
        for (int i = 0; i < WallsHolder.transform.childCount; i++)
        {
            WallsHolder.transform.GetChild(i).GetComponent<MeshCombinerScript>().CombineMesh();
        }
    }
}
