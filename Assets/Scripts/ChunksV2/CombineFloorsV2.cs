using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineFloorsV2 : MonoBehaviour
{
    [SerializeField] private Transform FloorsHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CombineFloorSegments()
    {
        foreach (Transform t in FloorsHolder.transform)
        {
            t.GetComponent<MeshCombinerScript>().CombineMesh();
        }
    }
}
