using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ChunkScriptV2 : MonoBehaviour
{
    List<List<List<GameObject>>> MainChunks = new List<List<List<GameObject>>>();
    int GridSize = 54;
    int floorSize = 6;

    int RoomScaleX = 6;
    int RoomScaleY = 6;
    int RoomScaleZ = 6;

    int padding = 3;

    [SerializeField] private GameObject _FloorParent;

    [Header("Block Creation Properties")]

    
    [SerializeField] public GameObject FloorsHolder;

    [Range(0, 20)]
    [SerializeField] private int FloorCreationIterations = 5;


    [Header("Walls")]
    [SerializeField] private List<BlendBlocks> _BlendBlocks;

    // Start is called before the first frame update
    void Start()
    {
        InitializeArray();

        MakeFloors();

        MakeWalls(0);
    }

    private void InitializeArray()
    {
        for (int k = 0; k < floorSize; k++)
        {
            List<List<GameObject>> kList = new List<List<GameObject>>();
            for (int i = 0; i < GridSize; i++)
            {
                List<GameObject> iList = new List<GameObject>();
                for (int j = 0; j < GridSize; j++)
                {
                    GameObject g = new GameObject(j.ToString() + "__" + i.ToString() + "__" + k.ToString());
                    g.AddComponent<BlocksV2>();
                    g.transform.parent = transform;
                    iList.Add(g);
                }
                kList.Add(iList);
            }
            MainChunks.Add(kList);
        }
    }

    #region FLOORS
    private void MakeFloors()
    {
        for (int i = 0; i < floorSize; i++)
        {
            LowerLeft(i);
            LowerMiddle(i);
            LowerRight(i);
            MiddleLeft(i);
            MiddleRight(i);
            TopLeft(i);
            TopMiddle(i);
            TopRight(i);

            ConnectRooms(i);
        }
    }

    private void InitiateSpawnTiles(int iMin, int iMax, int jMin, int jMax, int k)
    {
        for (int c = 0; c < FloorCreationIterations; c++)
        {
            int ii = Random.Range(iMin, iMax - 1);
            int jj = Random.Range(jMin, jMax - 1);

            int iiMax = Random.Range(Mathf.Min(ii + 2, iMax - 1), iMax - 1);
            int jjMax = Random.Range(Mathf.Min(jj + 2, jMax - 1), jMax - 1);

            for (int i = ii; i < iiMax; i++)
            {
                for (int j = jj; j < jjMax; j++)
                {
                    BlocksV2 chunk = MainChunks[k][i][j].GetComponent<BlocksV2>();
                    if (chunk.blockAssigned == null)
                    {
                        chunk.InstantiateTile(i - _FloorParent.transform.localScale.x / 2, j - _FloorParent.transform.localScale.y / 2, k, _FloorParent, RoomScaleX, RoomScaleY, RoomScaleZ, FloorsHolder.transform.GetChild(k));
                    }
                }
            }
        }
    }

    private void LowerLeft(int floorNo)
    {
        int iMin = padding;
        int iMax = iMin + (GridSize - 2 * padding) / 3;

        int jMin = padding;
        int jMax = jMin + (GridSize - 2 * padding) / 3;

        InitiateSpawnTiles(iMin, iMax, jMin, jMax, floorNo);
    }

    private void LowerMiddle(int floorNo)
    {
        int iMin = padding + (GridSize - 2 * padding) / 3;
        int iMax = iMin + (GridSize - 2 * padding) / 3;

        int jMin = padding;
        int jMax = jMin + (GridSize - 2 * padding) / 3;

        InitiateSpawnTiles(iMin, iMax, jMin, jMax, floorNo);
    }

    private void LowerRight(int floorNo)
    {
        int iMin = padding + (GridSize - 2 * padding) / 3 + (GridSize - 2 * padding) / 3;
        int iMax = iMin + (GridSize - 2 * padding) / 3;

        int jMin = padding;
        int jMax = jMin + (GridSize - 2 * padding) / 3;

        InitiateSpawnTiles(iMin, iMax, jMin, jMax, floorNo);
    }

    private void MiddleLeft(int floorNo)
    {
        int iMin = padding;
        int iMax = iMin + (GridSize - 2 * padding) / 3;

        int jMin = padding + (GridSize - 2 * padding) / 3;
        int jMax = jMin + (GridSize - 2 * padding) / 3;

        InitiateSpawnTiles(iMin, iMax, jMin, jMax, floorNo);
    }

    private void MiddleRight(int floorNo)
    {
        int iMin = padding + (GridSize - 2 * padding) / 3 + (GridSize - 2 * padding) / 3;
        int iMax = iMin + (GridSize - 2 * padding) / 3;

        int jMin = padding + (GridSize - 2 * padding) / 3;
        int jMax = jMin + (GridSize - 2 * padding) / 3;

        InitiateSpawnTiles(iMin, iMax, jMin, jMax, floorNo);
    }

    private void TopLeft(int floorNo)
    {
        int iMin = padding;
        int iMax = iMin + (GridSize - 2 * padding) / 3;

        int jMin = padding + (GridSize - 2 * padding) / 3 + (GridSize + 2 * padding) / 3;
        int jMax = jMin + (GridSize - 2 * padding) / 3;

        InitiateSpawnTiles(iMin, iMax, jMin, jMax, floorNo);
    }

    private void TopMiddle(int floorNo)
    {
        int iMin = padding + (GridSize - 2 * padding) / 3;
        int iMax = iMin + (GridSize - 2 * padding) / 3;

        int jMin = padding + (GridSize - 2 * padding) / 3 + (GridSize + 2 * padding) / 3;
        int jMax = jMin + (GridSize - 2 * padding) / 3;

        InitiateSpawnTiles(iMin, iMax, jMin, jMax, floorNo);
    }

    private void TopRight(int floorNo)
    {
        int iMin = padding + (GridSize - 2 * padding) / 3 + (GridSize - 2 * padding) / 3;
        int iMax = iMin + (GridSize - 2 * padding) / 3;

        int jMin = padding + (GridSize - 2 * padding) / 3 + (GridSize + 2 * padding) / 3;
        int jMax = jMin + (GridSize - 2 * padding) / 3;

        InitiateSpawnTiles(iMin, iMax, jMin, jMax, floorNo);
    }

    private void ConnectRooms(int floor)
    {

        // connect all the rooms to each other
        InstantiateConnectTilesHorizontal(padding + (GridSize - 2 * padding) / 6, padding + 5 * (GridSize - 2 * padding) / 6 + 1, padding + (GridSize - 2 * padding) / 6, floor);

        InstantiateConnectTilesHorizontal(padding + (GridSize - 2 * padding) / 6, padding + 5 * (GridSize - 2 * padding) / 6 + 1, padding + 5 * (GridSize - 2 * padding) / 6, floor);

        InstantiateConnectTilesVertical(padding + (GridSize - 2 * padding) / 6, padding + 5 * (GridSize - 2 * padding) / 6, padding + (GridSize - 2 * padding) / 6, floor);

        InstantiateConnectTilesVertical(padding + (GridSize - 2 * padding) / 6, padding + 5 * (GridSize - 2 * padding) / 6, padding + 5 * (GridSize - 2 * padding) / 6, floor);

        // connect middle rooms to the central arena

        InstantiateConnectTilesHorizontal(padding + (GridSize - 2 * padding) / 6, padding + (GridSize - 2 * padding) / 3, padding + (GridSize - 2 * padding) / 2, floor);

        InstantiateConnectTilesHorizontal(padding + 2 * (GridSize - 2 * padding) / 3, padding + 5 * (GridSize - 2 * padding) / 6, padding + (GridSize - 2 * padding) / 2, floor);

        InstantiateConnectTilesVertical(padding + (GridSize - 2 * padding) / 6, padding + (GridSize - 2 * padding) / 3 + 1, padding + (GridSize - 2 * padding) / 2, floor);

        InstantiateConnectTilesVertical(padding + 2 * (GridSize - 2 * padding) / 3, padding + (GridSize - 2 * padding) + 1, padding + (GridSize - 2 * padding) / 2, floor);
    }

    private void InstantiateConnectTilesHorizontal(int iMin, int iMax, int j, int k)
    {
        for (int i = iMin; i < iMax; i++)
        {
            BlocksV2 chunk = MainChunks[k][i][j].GetComponent<BlocksV2>();

            if (chunk.blockAssigned == null)
            {
                chunk.InstantiateTile(i - _FloorParent.transform.localScale.x / 2, j - _FloorParent.transform.localScale.y / 2, k, _FloorParent, RoomScaleX, RoomScaleY, RoomScaleZ, FloorsHolder.transform.GetChild(k));
            }
        }
    }

    private void InstantiateConnectTilesVertical(int jMin, int jMax, int i, int k)
    {
        for (int j = jMin; j < jMax; j++)
        {
            BlocksV2 chunk = MainChunks[k][i][j].GetComponent<BlocksV2>();

            if (chunk.blockAssigned == null)
            {
                chunk.InstantiateTile(i - _FloorParent.transform.localScale.x / 2, j - _FloorParent.transform.localScale.y / 2, k, _FloorParent, RoomScaleX, RoomScaleY, RoomScaleZ, FloorsHolder.transform.GetChild(k));
            }
        }
    }
    #endregion

    private void MakeWalls(int floor)
    {
        for (int i = 3; i < GridSize; i = GridSize)
        {
            for (int j = 0; j < GridSize - 1; j++)
            {
                BlocksV2 chunk = MainChunks[floor][i][j].GetComponent<BlocksV2>();
                if (chunk.blockAssigned == null) // this does not have floor
                {
                    if (MainChunks[floor][j + 1][i].GetComponent<BlocksV2>().blockAssigned != null)
                    {
                        Debug.Log("there is a wall above");
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
