using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkCreator : MonoBehaviour
{

    private GameObject[,,] ChunkArray;

    [SerializeField] private int GridSize = 10;

    // Start is called before the first frame update
    void Start()
    {
        ChunkArray = new GameObject[GridSize, GridSize, GridSize];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
