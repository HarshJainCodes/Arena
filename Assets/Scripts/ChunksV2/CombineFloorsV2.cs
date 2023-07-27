using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineFloorsV2 : MonoBehaviour
{
    /// <summary>
    /// This will contain the gameobject that will contain all the created floor tiles.
    /// It has child objects that stores the tiles for the respective floors. 
    /// Floor0, Floor1, Floor2 ... Floor5
    /// </summary>
    [SerializeField] private Transform FloorsHolder;

    /// <summary>
    /// This script combines the floor tiles by iterating over the <see cref="FloorsHolder"/> gameobject and calls the <see cref="MeshCombinerScript.CombineMesh"/> function.
    /// </summary>
    public void CombineFloorSegments()
    {
        foreach (Transform t in FloorsHolder.transform)
        {
            t.GetComponent<MeshCombinerScript>().CombineMesh();
        }
    }
}
