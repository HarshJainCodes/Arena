using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineCubesV2 : MonoBehaviour
{
    [SerializeField] private GameObject WallsHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CombineCubeSegments()
    {
        for (int i = 0; i < WallsHolder.transform.childCount; i++)
        {
            WallsHolder.transform.GetChild(i).GetComponent<MeshCombinerScript>().CombineMesh();
            Debug.Log(WallsHolder.transform.GetChild(i).GetComponent<MeshCombinerScript>() == null);
        }
    }
}
