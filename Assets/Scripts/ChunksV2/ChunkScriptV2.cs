using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ChunkScriptV2 : MonoBehaviour
{
    List<List<List<GameObject>>> MainChunks = new List<List<List<GameObject>>>();
    int GridSize = 54;
    int floorSize = 6;

    [SerializeField] int RoomScaleX = 6;
    [SerializeField] int RoomScaleY = 6;
    [SerializeField] int RoomScaleZ = 6;

    int padding = 3;

    [SerializeField] private GameObject _FloorParent;

    [Header("Block Creation Properties")]

    
    [SerializeField] public GameObject FloorsHolder;
    [SerializeField] public GameObject WallHolder;

    [Range(0, 20)]
    [SerializeField] private int FloorCreationIterations = 5;


    [Header("Walls")]
    [SerializeField] private List<BlendBlocks> _BlendBlocks;

    // Start is called before the first frame update
    void Start()
    {
        InitializeArray();

        MakeFloors();

        for (int i = 0; i < floorSize; i++)
        {
            MakeWalls(i);
        }
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
                        chunk.InstantiateTile(i - _FloorParent.transform.localScale.x / 2, j - _FloorParent.transform.localScale.y / 2, k, _FloorParent, RoomScaleX, RoomScaleY, RoomScaleZ, 0, FloorsHolder.transform.GetChild(k));
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

        int jMin = padding + 2 * (GridSize - 2 * padding) / 3;
        int jMax = jMin + (GridSize - 2 * padding) / 3;

        InitiateSpawnTiles(iMin, iMax, jMin, jMax, floorNo);
    }

    private void TopMiddle(int floorNo)
    {
        int iMin = padding + (GridSize - 2 * padding) / 3;
        int iMax = iMin + (GridSize - 2 * padding) / 3;

        int jMin = padding + 2 * (GridSize - 2 * padding) / 3;
        int jMax = jMin + (GridSize - 2 * padding) / 3;

        InitiateSpawnTiles(iMin, iMax, jMin, jMax, floorNo);
    }

    private void TopRight(int floorNo)
    {
        int iMin = padding + (GridSize - 2 * padding) / 3 + (GridSize - 2 * padding) / 3;
        int iMax = iMin + (GridSize - 2 * padding) / 3;

        int jMin = padding + 2 * (GridSize - 2 * padding) / 3;
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

        InstantiateConnectTilesHorizontal(padding + (GridSize - 2 * padding) / 6, padding + (GridSize - 2 * padding) / 3 + 1, padding + (GridSize - 2 * padding) / 2, floor);

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
                chunk.InstantiateTile(i - _FloorParent.transform.localScale.x / 2, j - _FloorParent.transform.localScale.y / 2, k, _FloorParent, RoomScaleX, RoomScaleY, RoomScaleZ, 0, FloorsHolder.transform.GetChild(k));
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
                chunk.InstantiateTile(i - _FloorParent.transform.localScale.x / 2, j - _FloorParent.transform.localScale.y / 2, k, _FloorParent, RoomScaleX, RoomScaleY, RoomScaleZ, 0, FloorsHolder.transform.GetChild(k));
            }
        }
    }
    #endregion

    private bool isValid(int i, int j)
    {
        return i >= 0 && j >= 0 && i < GridSize && j < GridSize;
    }

    private void MakeWalls(int floor)
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize - 1; j++)
            {
                if ((i > padding + (GridSize - 2 * padding) / 3) && (j > padding + (GridSize - 2 * padding) / 3) && (i < padding + 2 * (GridSize - 2 * padding) / 3) && (j < padding + 2 * (GridSize - 2 * padding) / 3))
                {
                    continue;
                }

                BlocksV2 chunk = MainChunks[floor][j][i].GetComponent<BlocksV2>();
                if (chunk.blockAssigned == null) // this does not have floor
                {
                    // this will spawn the 1 side wall facing upwards in the array
                    /* if (isValid(i, j + 1) && MainChunks[floor][j + 1][i].GetComponent<BlocksV2>().ID == 0 && isValid(i, j - 1) && MainChunks[floor][j - 1][i].GetComponent<BlocksV2>().ID != 0 && isValid(i - 1, j) && MainChunks[floor][j][i - 1].GetComponent<BlocksV2>().ID != 0 && isValid(j, i + 1) && MainChunks[floor][j][i + 1].GetComponent<BlocksV2>().ID != 0)
                     {
                         GameObject wallBlock = _BlendBlocks[0].blocks[Random.Range(0, 3)];
                         chunk.InstantiateWall(j -  wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 270);
                     }

                     // this will spawn the 1 side wall facing downwards in the array
                     if (isValid(i, j - 1) && MainChunks[floor][j - 1][i].GetComponent<BlocksV2>().ID == 0 && isValid(i, j + 1) && MainChunks[floor][j + 1][i].GetComponent<BlocksV2>().ID != 0 && isValid())*/
                    int val = 0;

                    // up
                    if (isValid(i, j + 1) && MainChunks[floor][j + 1][i].GetComponent<BlocksV2>().ID == 0)
                    {
                        val += 1;
                    }
                    if (isValid(i - 1, j) && MainChunks[floor][j][i - 1].GetComponent<BlocksV2>().ID == 0)
                    {
                        val += 10;
                    }
                    if (isValid(i, j - 1) && MainChunks[floor][j - 1][i].GetComponent<BlocksV2>().ID == 0)
                    {
                        val += 100;
                    }
                    if (isValid(i + 1, j) && MainChunks[floor][j][i + 1].GetComponent<BlocksV2>().ID == 0)
                    {
                        val += 1000;
                    }

                    GameObject wallBlock;
                    switch (val)
                    {

                        case 0:
                            wallBlock = _BlendBlocks[5].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 0);
                            break;
                        case 1:
                            wallBlock = _BlendBlocks[0].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 270);
                            break;
                        case 10:
                            wallBlock = _BlendBlocks[0].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 0);
                            break;
                        case 11:
                            wallBlock = _BlendBlocks[1].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 270);
                            break;
                        case 100:
                            wallBlock = _BlendBlocks[0].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 90);
                            break;
                        case 101:
                            wallBlock = _BlendBlocks[4].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 90);
                            break;
                        case 110:
                            wallBlock = _BlendBlocks[1].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 0);
                            break;
                        case 111:
                            wallBlock = _BlendBlocks[2].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 0);
                            break;
                        case 1000:
                            wallBlock = _BlendBlocks[0].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 180);
                            break;
                        case 1001:
                            wallBlock = _BlendBlocks[1].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 180);
                            break;
                        case 1010:
                            wallBlock = _BlendBlocks[4].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 0);
                            break;
                        case 1011:
                            wallBlock = _BlendBlocks[2].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 180);
                            break;
                        case 1100:
                            wallBlock = _BlendBlocks[1].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 90);
                            break;
                        case 1101:
                            wallBlock = _BlendBlocks[2].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 180);
                            break;
                        case 1110:
                            wallBlock = _BlendBlocks[2].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 90);
                            break;
                        case 1111:
                            wallBlock = _BlendBlocks[3].blocks[Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform, 0);
                            break;
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
