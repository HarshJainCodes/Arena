using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineFloors : MonoBehaviour
{

    public static CombineFloors Instance { get; private set; }
    [SerializeField] private GameObject _CombinedFloorHolder;

    private ChunkCreator _ChunkCreator;

    private int _GridSize;
    private int _FloorCount;
    private GameObject[,,] _ChunkArray;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _ChunkCreator = GetComponent<ChunkCreator>();
        _GridSize = _ChunkCreator.GetGridSize();
        _FloorCount = _ChunkCreator.GetFloorCount();
    }

    public void MakeFloorSegments()
    {
        _ChunkArray = _ChunkCreator.ChunkArray;

        for (int i = 0; i < _GridSize; i += 10)
        {
            for (int j = 0; j < _GridSize; j += 10)
            {
                string id = i.ToString() + '_' + j.ToString();

                GameObject go = new GameObject(id);
                go.transform.parent = _CombinedFloorHolder.transform;
                go.AddComponent<MeshCombinerScript>();

                for (int ii = i; ii < i + 10; ii++)
                {
                    for (int jj = j; jj < j + 10; jj++)
                    {
                        for (int k = 0; k < _FloorCount; k++)
                        {
                            Block b = _ChunkArray[ii, jj, k].GetComponent<Block>();
                            if (b.ID == 1)
                            {
                                GameObject created = b.Created;
                                created.transform.parent = go.transform;
                            }
                        }
                    }
                }
            }
        }
    }

    public void CombineFloorSegments()
    {
        for (int i = 0; i < _CombinedFloorHolder.transform.childCount; i++)
        {
            _CombinedFloorHolder.transform.GetChild(i).GetComponent<MeshCombinerScript>().CombineMesh();
        }
    }
}

