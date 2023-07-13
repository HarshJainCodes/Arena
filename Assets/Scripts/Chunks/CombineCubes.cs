using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineCubes : MonoBehaviour
{
    public static CombineCubes Instance { get; private set; }

    [SerializeField] private GameObject _CombinedCubeHolder;
    [SerializeField] private GameObject _Empty;

    private ChunkCreator _ChunkCreator;

    private int _GridSize;
    private int _FloorCount;
    private GameObject[,,] _ChunkArray;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _ChunkCreator = GetComponent<ChunkCreator>();
        _GridSize = _ChunkCreator.GetGridSize();
        _FloorCount = _ChunkCreator.GetFloorCount();
    }

    public void MakeCubeSegments()
    {
        _ChunkArray = _ChunkCreator.ChunkArray;

        int a = 0;

        Vector3Int roomScale = _ChunkCreator.GetRoomScale();

        while (a <= _GridSize)
        {
            int j = Random.Range(1, _ChunkCreator.GetPadding());
            int nextI = Mathf.Min(_GridSize - 1, Random.Range(a + 4, a + 8));
            for (int ii = a; ii < nextI; ii++)
            {
                for (int jj = 0; jj < j; jj++)
                {
                    for (int k = 0; k < _FloorCount; k++)
                    {
                        Block b = _ChunkArray[ii, jj, k].GetComponent<Block>();

                        if (b.ID == 0)
                        {
                            b.UnCollapse();
                            _ChunkArray[ii, jj, k] = new GameObject();
                            _ChunkArray[ii, jj, k].transform.parent = transform;
                            Block newB = _ChunkArray[ii, jj, k].AddComponent<Block>();

                            newB.InitializeBlock(ii, jj, k, _Empty, roomScale.x, roomScale.y, roomScale.z, 2);
                            newB.SetCollapsed(transform);
                        }
                    }
                }
            }
            a = nextI;
            if (a >= _GridSize - 1)
            {
                break;
            }
        }

        for (int i = 0; i < _GridSize; i += 10)
        {
            for (int j = 0; j < _GridSize; j += 10) {
                string id = (i.ToString() + "_" + j.ToString());

                GameObject go = new GameObject(id);
                go.transform.parent = _CombinedCubeHolder.transform;
                go.AddComponent<MeshCombinerScript>();

                for (int ii = i; ii < i + 10; ii++)
                {
                    for (int jj = j; jj < j + 10; jj++)
                    {
                        for (int k = 0; k < _FloorCount; k++)
                        {
                            Block b = _ChunkArray[ii, jj, k].GetComponent<Block>();
                            if (b.ID == 0)
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

    public void CombineCubeSegments()
    {
        for (int i = 0; i < _CombinedCubeHolder.transform.childCount; i++)
        {
            _CombinedCubeHolder.transform.GetChild(i).GetComponent<MeshCombinerScript>().CombineMesh();
        }
    }
}
