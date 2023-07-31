using PicaVoxel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the rewrite of the chunkScipt that was the rewrite of another chunk script that was used before.
/// This handles the core arena formation algorithm,  
/// </summary>
public class ChunkScriptV2 : MonoBehaviour
{
    /// <summary>
    /// Main Chunks contains the 3 dimentional arena that will store the GameObjects that contains the BlocksV2 script that contains all the important data used by the algorithm to form the arena
    /// <para> This is set up as Floor x Width x Height so for example this is a 6 x 50 x 50 3 dimentional list</para>
    /// </summary>
    List<List<List<GameObject>>> MainChunks = new List<List<List<GameObject>>>();

    [Tooltip("This determines the grid size of the whole procedurally generated arena. Remember if you change this values then you would also need to change the coordinated of the central arena. \nTypically the central arena coorinates are given by GridSize * RoomScale / 2")]
    [SerializeField] private int GridSize = 54;

    [Tooltip("This contains floors as well as the demo layers that might be used for populating stuff around the padding")]
    int totalFloorSize = 9;
    [Tooltip("Total number of floors that would be used to instantiate walls and tiles")]
    int floorSize = 6;

    [SerializeField] int RoomScaleX = 6;
    [SerializeField] int RoomScaleY = 6;
    [SerializeField] int RoomScaleZ = 6;

    [Tooltip("Padding used to avoid formation of floors around the corners of the arena so that we spawn walls in those area")]
    int padding = 6;

    /// <summary>
    /// This bool is used to trigger the Enemies spawnning as we only want to spawn the enemies after all other things like animation, A* graph scan, combining floors are done

    /// </summary>
    public bool IsGridGenerated = false;

    
    [SerializeField] private GameObject _FloorParent;
    
    [SerializeField] private GameObject _StairsPrefab;
    
    /// <summary>
    /// this will store all the floors to keep the heiarchy clean
    /// </summary>
    [Header("Block Creation Properties")]

    [SerializeField] public GameObject FloorsHolder;

    /// <summary>
    /// This will store all the walls to keep the heiarchy clean
    /// </summary>
    [SerializeField] public GameObject WallHolder;

    /// <summary>
    /// This will store all the stiars to keep the heiarchy clean
    /// </summary>
    [SerializeField] private GameObject StairsHolder;

    [Range(0, 20)]
    [SerializeField] private int FloorCreationIterations = 5;


    /// <summary>
    /// This will store the all the 4 side walls and top and parrallel side walls to be instantiated in the scene
    /// </summary>
    [Header("Walls")]
    [SerializeField] private List<BlendBlocks> _BlendBlocks;

    /// <summary>
    /// This will stores the cubes that are spawned on the sides of the padding above the 6 floors
    /// </summary>
    [SerializeField] Transform CubesOnTopHolder;

    private bool run = false;

    CombineCubesV2 CombnieCubesV2Script;
    CombineFloorsV2 FloorCombineV2Script;

    [SerializeField] private GameObject _LavaRocks;
    [SerializeField] private GameObject LoadingScreen;

    // Start is called before the first frame update

    private bool _coroutineDone = false;

    /// <summary>
    /// We make this an enumerator becuase this is needed in order for the pica voxel to work, otherwise the blocks wouldnt combine properly
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        // while the arena is being formed show the loading screen.
        LoadingScreen.SetActive(true);
        _LavaRocks.SetActive(false);
        WallHolder.SetActive(false);
        FloorsHolder.SetActive(false);

        for (int i = 0; i < _Volumes.Length; i++)
        {
            _VolumeStorages[i] = new VolumeStorage(_Volumes[i].XSize, _Volumes[i].YSize, _Volumes[i].ZSize);
        }

        LoadChunks();

        if (GetBlocks.Instance.export)
            StartCoroutine(changeVolumesnotTiles());

        yield return new WaitForSeconds(10f);
        

        CombnieCubesV2Script = GetComponent<CombineCubesV2>();
        FloorCombineV2Script = GetComponent<CombineFloorsV2>();

        InitializeArray();

        MakeFloors();

        for (int i = 0; i < floorSize; i++)
        {
            MakeWalls(i);
        }

        CutCubesBottom();
        //CutCubesTop();
        //CutCubesLeft();

        AddCubesOnTop();
        addStairsToAccessTop();

        _coroutineDone = true;
    }

    /// <summary>
    /// This function will initialize the array in all the three dimension by the <see cref="GridSize"/> and the <see cref="floorSize"/> variable
    /// </summary>
    private void InitializeArray()
    {
        for (int k = 0; k < totalFloorSize; k++)
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

    /// <summary>
    /// This will form the floors based on a certain algorithm of dividing the grid into 9 blocks so that each block has a central pathway and then we can connect these pathway so that 
    /// all the rooms remains connected to each other and the middle rooms gets connected to the central arena 
    /// </summary>
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

    /// <summary>
    /// This will initialize the given floor tiles based on the x, y, and z component passed to the function
    /// <para> this will also make a stair in eacch of the 9 block so that we can go down</para>
    /// </summary>
    /// <param name="iMin"></param>
    /// <param name="iMax"></param>
    /// <param name="jMin"></param>
    /// <param name="jMax"></param>
    /// <param name="k"></param>
    private void InitiateSpawnTiles(int iMin, int iMax, int jMin, int jMax, int k)
    {
        BlocksV2 chunk;
        BlocksV2 chunkBelow;
        bool stairsFormed = false;

        for (int c = 0; c < FloorCreationIterations; c++)
        {
            int ii = UnityEngine.Random.Range(iMin, iMax - 1);
            int jj = UnityEngine.Random.Range(jMin, jMax - 1);

            int iiMax = UnityEngine.Random.Range(Mathf.Min(ii + 2, iMax - 1), iMax - 1);
            int jjMax = UnityEngine.Random.Range(Mathf.Min(jj + 2, jMax - 1), jMax - 1);

            for (int i = ii; i < iiMax; i++)
            {
                for (int j = jj; j < jjMax; j++)
                {
                    chunk = MainChunks[k][i][j].GetComponent<BlocksV2>();
                    if (chunk.blockAssigned == null)
                    {
                        chunk.InstantiateTile(i - _FloorParent.transform.localScale.x / 2, j - _FloorParent.transform.localScale.y / 2, k, _FloorParent, RoomScaleX, RoomScaleY, RoomScaleZ, 0, FloorsHolder.transform.GetChild(k));
                    }else if (stairsFormed == false && chunk.ID == 0)
                    {
                        if (k > 0)
                        {
                            chunkBelow = MainChunks[k - 1][i][j].GetComponent<BlocksV2>();
                            if (chunkBelow.ID == 0)
                            {
                                Destroy(chunkBelow.blockAssigned);
                                chunkBelow.ID = -1;
                                Destroy(chunk.blockAssigned);
                                chunk.ID = -1;
                                stairsFormed = true;

                                chunkBelow.InstantiateStair(i - _StairsPrefab.transform.localScale.x , j - _StairsPrefab.transform.localScale.y, k - 1, _StairsPrefab, RoomScaleX, RoomScaleY, RoomScaleZ, StairsHolder.transform,0);
                            }
                        }
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
        int jMax = jMin + (GridSize - 2 * padding) / 3 - 1;

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
        int iMax = iMin + (GridSize - 2 * padding) / 3 - 1;

        int jMin = padding + (GridSize - 2 * padding) / 3;
        int jMax = jMin + (GridSize - 2 * padding) / 3;

        InitiateSpawnTiles(iMin, iMax, jMin, jMax, floorNo);
    }

    private void MiddleRight(int floorNo)
    {
        int iMin = padding + (GridSize - 2 * padding) / 3 + (GridSize - 2 * padding) / 3 + 1;
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

        int jMin = padding + 2 * (GridSize - 2 * padding) / 3 + 1;
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

    /// <summary>
    /// After the tiles are spawn in each 9 regions we connect them by spawning floors from the center of a arena to the center of another arena 
    /// </summary>
    /// <param name="floor"></param>
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

    /// <summary>
    /// checks if the given i and j parameters are outside of the bounds of array or not
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    private bool isValid(int i, int j)
    {
        return i >= 0 && j >= 0 && i < GridSize && j < GridSize;
    }

    /// <summary>
    /// This will make the wall on the place where there is no tile spawned also it scan all the 4 sides to check which type of wall to spawn (1side, 2side, 3side, 4side, parallel, topbottom)
    /// </summary>
    /// <param name="floor"></param>
    private void MakeWalls(int floor)
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize - 1; j++)
            {
                if ((i >= padding + (GridSize - 2 * padding) / 3 - 1) && (j >= padding + (GridSize - 2 * padding) / 3 - 1) && (i <= padding + 2 * (GridSize - 2 * padding) / 3 + 1) && (j <= padding + 2 * (GridSize - 2 * padding) / 3 + 1))
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
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor+1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[5].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 0);
                            break;
                        case 1:
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor+1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[0].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 270);
                            break;
                        case 10:
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor + 1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[0].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 0);
                            break;
                        case 11:
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor + 1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[1].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 270);
                            break;
                        case 100:
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor+1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[0].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 90);
                            break;
                        case 101:
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor + 1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[4].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 90);
                            break;
                        case 110:
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor + 1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[1].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 0);
                            break;
                        case 111:
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor + 1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[2].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 0);
                            break;
                        case 1000:
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor + 1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[0].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 180);
                            break;
                        case 1001:
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor + 1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[1].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 180);
                            break;
                        case 1010:
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor + 1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[4].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 0);
                            break;
                        case 1011:
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor + 1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[2].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 180);
                            break;
                        case 1100:
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor + 1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[1].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 90);
                            break;
                        case 1101:
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor + 1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[2].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 180);
                            break;
                        case 1110:
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor + 1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[2].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 90);
                            break;
                        case 1111:
                            if (floor == floorSize - 1)
                            {
                                var tile = Instantiate(_FloorParent, new Vector3((i - _FloorParent.transform.localScale.x / 2) * RoomScaleX, (floor + 1) * RoomScaleZ, (j - _FloorParent.transform.localScale.y / 2) * RoomScaleY), Quaternion.identity, FloorsHolder.transform.GetChild(floor + 1));
                                tile.transform.localScale = new Vector3(RoomScaleX, RoomScaleY, RoomScaleZ);
                            }
                            wallBlock = _BlendBlocks[3].blocks[UnityEngine.Random.Range(0, 3)];
                            chunk.InstantiateWall(j - wallBlock.transform.localScale.y / 2, i - wallBlock.transform.localScale.x / 2, floor, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, WallHolder.transform.GetChild(floor), 0);
                            break;
                    }
                    
                }
            }
        }
    }

    /// <summary>
    /// This will cut the cubes to make some asymettric structures around the padding of the arena 
    /// </summary>
    private void CutCubesBottom()
    {
        int a = 0;

        while (a <= GridSize)
        {
            int j = UnityEngine.Random.Range(1, padding);
            int nextI = Mathf.Min(GridSize - 1, UnityEngine.Random.Range(a + 4, a + 8));

            for (int ii = a; ii < nextI; ii++)
            {
                for (int jj = 0; jj < j; jj++)
                {
                    int minK = UnityEngine.Random.Range(0, floorSize - 1);
                    int maxK = UnityEngine.Random.Range(minK, floorSize + 1);
                    //int maxK = floorSize;

                    for (int k = minK; k < maxK; k++)
                    {
                        BlocksV2 chunk = MainChunks[k][ii][jj].GetComponent<BlocksV2>();

                        if (chunk.blockAssigned != null)
                        {
                            Destroy(chunk.blockAssigned);
                            chunk.blockAssigned = null;
                            chunk.ID = -1;
                        }
                    }
                }
            }
            a = nextI + UnityEngine.Random.Range(2, 5);
            if (a >= GridSize - 1)
            {
                break;
            }
        }
    }

    private void CutCubesTop()
    {
        int a = 0;

        while (a <= GridSize)
        {
            int j = UnityEngine.Random.Range(GridSize - padding + 1, GridSize + 1);
            int nextI = Mathf.Min(GridSize - 1, UnityEngine.Random.Range(a + 4, a + 8));

            for (int ii = a; ii < nextI; ii++)
            {
                for (int jj = 0; jj < j; jj++)
                {
                    int minK = UnityEngine.Random.Range(0, floorSize - 1);
                    int maxK = UnityEngine.Random.Range(minK, floorSize + 1);
                    //int maxK = floorSize;

                    for (int k = minK; k < maxK; k++)
                    {
                        BlocksV2 chunk = MainChunks[k][ii][jj].GetComponent<BlocksV2>();

                        if (chunk.blockAssigned != null)
                        {
                            Destroy(chunk.blockAssigned);
                            chunk.blockAssigned = null;
                            chunk.ID = -1;
                        }
                    }
                }
            }
            a = nextI + UnityEngine.Random.Range(2, 5);
            if (a >= GridSize - 1)
            {
                break;
            }
        }
    }

    private void CutCubesLeft()
    {
        int a = 0;

        while (a <= GridSize)
        {
            int i = UnityEngine.Random.Range(1, padding);
            int nextJ = Mathf.Min(GridSize - 1, UnityEngine.Random.Range(a + 4, a + 8));

            for (int ii = a; ii < i; ii++)
            {
                for (int jj = 0; jj < nextJ; jj++)
                {
                    int minK = UnityEngine.Random.Range(0, floorSize - 1);
                    int maxK = UnityEngine.Random.Range(minK, floorSize + 1);
                    //int maxK = floorSize;

                    for (int k = minK; k < maxK; k++)
                    {
                        BlocksV2 chunk = MainChunks[k][jj][ii].GetComponent<BlocksV2>();

                        if (chunk.blockAssigned != null)
                        {
                            Destroy(chunk.blockAssigned);
                            chunk.blockAssigned = null;
                            chunk.ID = -1;
                        }
                    }
                }
            }
            a = nextJ + UnityEngine.Random.Range(2, 5);
            if (a >= GridSize - 1)
            {
                break;
            }
        }
    }


    private void AddCubesOnTopHelper(int iMin, int iMax, int jMin, int jMax, int k)
    {
        GameObject wallBlock = _BlendBlocks[3].blocks[UnityEngine.Random.Range(0, 3)];

        for (int i = iMin; i < iMax; i++)
        {
            for (int j = jMin; j < jMax; j++)
            {
                if (MainChunks[k - 1][i][j].GetComponent<BlocksV2>().blockAssigned != null)
                {
                    BlocksV2 chunk = MainChunks[k][i][j].GetComponent<BlocksV2>();
                    chunk.InstantiateWall(i - wallBlock.transform.localScale.x / 2, j - wallBlock.transform.localScale.y / 2, k, wallBlock, RoomScaleX, RoomScaleY, RoomScaleZ, CubesOnTopHolder, 0);
                }
            }
        }
    }

    private void AddCubesOnTop()
    {
        int iMin, iMax, jMin, jMax;

        for (int k = floorSize - 1; k < totalFloorSize; k++)
        {
            for (int c = 0; c < 10; c++)
            {
                // left side
                iMin = UnityEngine.Random.Range(0, padding / 2);
                iMax = UnityEngine.Random.Range(iMin, padding);
                jMin = UnityEngine.Random.Range(0, GridSize + 1);
                jMax = UnityEngine.Random.Range(jMin, Mathf.Min(jMin + 15, GridSize + 1));
                AddCubesOnTopHelper(iMin, iMax, jMin, jMax, k);

                // right side
                iMin = UnityEngine.Random.Range(GridSize - padding + 1, GridSize - padding / 2);
                iMax = UnityEngine.Random.Range(iMin, GridSize + 1);
                jMin = UnityEngine.Random.Range(0, GridSize + 1);
                jMax = UnityEngine.Random.Range(jMin, Mathf.Min(jMin + 15, GridSize + 1));
                AddCubesOnTopHelper(iMin, iMax, jMin, jMax, k);

                // bottom side
                iMin = UnityEngine.Random.Range(0, GridSize + 1);
                iMax = UnityEngine.Random.Range(iMin, GridSize + 1);
                jMin = UnityEngine.Random.Range(0, padding / 2);
                jMax = UnityEngine.Random.Range(jMin, padding);
                AddCubesOnTopHelper(iMin, iMax, jMin, jMax, k);

                // top side
                iMin = UnityEngine.Random.Range(0, GridSize + 1);
                iMax = UnityEngine.Random.Range(iMin, GridSize + 1);
                jMin = UnityEngine.Random.Range(GridSize - padding + 1, GridSize - padding / 2);
                jMax = UnityEngine.Random.Range(jMin, GridSize + 1);
                AddCubesOnTopHelper(iMin, iMax, jMin, jMax, k);
            }
        }
    }

    /// <summary>
    /// this is used by the spawn manager to get the list of valid game objects where enemies can be spawned 
    /// </summary>
    /// <param name="Player"></param>
    /// <param name="numOfEnemies"></param>
    /// <param name="minEnemyDistance"></param>
    /// <param name="maxEnemyDistance"></param>
    /// <returns> List<GameObject> </returns>
    public List<GameObject> GetSpawnPoints(Transform Player, int numOfEnemies, int minEnemyDistance, int maxEnemyDistance)
    {
        Debug.Log("calling spawn points");
        Vector3 PlayerPosition = Player.TransformPoint(Vector3.zero);


        List<GameObject> possibleSpawnPoint = new List<GameObject>();
        List<GameObject> spawnPoints = new List<GameObject>();

        for (int k = 0; k < floorSize; k++)
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    if (MainChunks[k][i][j].GetComponent<BlocksV2>().ID == 0 && Vector3.Distance(PlayerPosition, MainChunks[k][i][j].GetComponent<BlocksV2>().blockAssigned.transform.position) < maxEnemyDistance && Vector3.Distance(PlayerPosition, MainChunks[k][i][j].GetComponent<BlocksV2>().blockAssigned.transform.position) > minEnemyDistance)
                    {
                        possibleSpawnPoint.Add(MainChunks[k][i][j].GetComponent<BlocksV2>().blockAssigned);
                    }
                }
            }
        }

        possibleSpawnPoint.Sort(delegate (GameObject A, GameObject B)
        {
            if (A.transform.position.y < B.transform.position.y)
            {
                return 1;
            }
            return 0;
        });

        for (int i = 0; i < numOfEnemies; i++)
        {
            spawnPoints.Add(possibleSpawnPoint[i]);
        }

        return spawnPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (run == false && _coroutineDone)
        {
            run = true;
            CombnieCubesV2Script.CombineCubeSegments();
            //FloorCombineV2Script.CombineFloorSegments();

            LoadingScreen.SetActive(false);
            _LavaRocks.SetActive(true);
            WallHolder.SetActive(true);
            FloorsHolder.SetActive(true);

            StartCoroutine(_LavaRocks.GetComponent<ChunkAnimationV2>().AnimateChunkComingToTop());

            foreach (Transform t in WallHolder.transform)
            {
                StartCoroutine(t.GetComponent<ChunkAnimationV2>().AnimateChunkComingToTop());
            }

            FloorsHolder.GetComponent<FloorAnimaationV2>().AnimateFloorComingToTop();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Invoke("SetCursorInvisible", 5);
        }
    }

    private void SetCursorInvisible()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void _setVoxelBlock()
    {
        for (int i = 0; i < _Volumes.Length; i++)
        {
            _Volumes[i].XSize = _VolumeStorages[i].sizex;
            _Volumes[i].YSize = _VolumeStorages[i].sizey;
            _Volumes[i].ZSize = _VolumeStorages[i].sizez;
            /*_Volumes[i].Pivot = new Vector3(_Volumes[i].XSize/2, _Volumes[i].YSize / 2, _Volumes[i].ZSize / 2);
            _Volumes[i].UpdatePivot();*/
            _Volumes[i].UpdateAllChunks();
            for (int x = 0; x < _Volumes[i].XSize; x++)
            {
                for (int y = 0; y < _Volumes[i].YSize; y++)
                {
                    for (int z = 0; z < _Volumes[i].ZSize; z++)
                    {
                        PicaVoxelPoint p = new PicaVoxelPoint(x, y, z);
                        Color current = new Color(_VolumeStorages[i].store[x, y, z].r, _VolumeStorages[i].store[x, y, z].g, _VolumeStorages[i].store[x, y, z].b, _VolumeStorages[i].store[x, y, z].a);
                        _Volumes[i].SetVoxelAtArrayPosition(p, new Voxel()
                        {
                            State = (VoxelState)_VolumeStorages[i].store[x, y, z].currentstate,
                            Color = current,
                            Value = _VolumeStorages[i].store[x, y, z].val
                        });
                    }
                }
            }
            Debug.Log("Before spawn");
        }
    }

    public void LoadData()
    {
        //getVolumes();
        for (int i = 0; i < _Volumes.Length; i++)
        {
            string _currentSet = $"/{set}block{i}.json";
            _VolumeStorages[i] = _DataSerializer.LoadData<VolumeStorage>(_currentSet);
        }
        _setVoxelBlock();
        //SetExport();
    }
    [SerializeField] private int set = 0;
    private DataSerializer _DataSerializer = new DataSerializer();
    [SerializeField] Volume[] _Volumes = new Volume[7];
    private VolumeStorage[] _VolumeStorages = new VolumeStorage[10];
    public void LoadChunks()
    {
        if (GetBlocks.Instance.export)
        {
            LoadData();

            for (int i = 0; i < _Volumes.Length; i++)
            {
                objects[i] = _Volumes[i].gameObject;   
            }

            /*for (int i = 0; i < _BlendBlocks.Count; i++)
            {
                for (int j = 0; j < _BlendBlocks[i].blocks.Count; j++)
                {

                    GameObject temp = pref;
                    Volume _volume = temp.GetComponent<Volume>();
                    _volume = GetBlocks.blocks[i];
                    for (int x = 0; x < GetBlocks.blocks[i].XSize; x++)
                    {
                        for (int y = 0; y < GetBlocks.blocks[i].YSize; y++)
                        {
                            for (int z = 0; z < GetBlocks.blocks[i].ZSize; z++)
                            {
                                Voxel? v = GetBlocks.blocks[i].GetVoxelAtArrayPosition(x, y, z);
                                _volume.SetVoxelAtArrayPosition(new PicaVoxelPoint(x, y, z), new Voxel()
                                {
                                    State = v.Value.State,
                                    Color = v.Value.Color,
                                    Value = v.Value.Value
                                });
                            }
                        }
                    }

                    // _BlendBlocks[i].blocks[j] = _Volumes[i].gameObject;
                }
            }*/
        }
    }
    private GameObject[] objects = new GameObject[7];

    IEnumerator changeVolumesnotTiles()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < _BlendBlocks.Count; i++)
        {
            objects[i] = _Volumes[i].gameObject;
            objects[i].AddComponent<MeshCombiner>();
            MeshCombiner getter = objects[i].GetComponent<MeshCombiner>();
            getter.CreateMultiMaterialMesh = true;
            getter.DestroyCombinedChildren = true;
            getter.CombineMeshes(false);
            Debug.Log("How is this not done");
            _BlendBlocks[i].blocks[0] = objects[i].gameObject;
            _BlendBlocks[i].blocks[1] = objects[i].gameObject;
            _BlendBlocks[i].blocks[2] = objects[i].gameObject;
        }
        Debug.Log("This is done");
    }
    /// <summary>
    /// This function adds the stairs to the top floor in such a fashion that the staris are connected to atleast two walls.
    /// </summary>
    private void addStairsToAccessTop()
    {
        int k = floorSize-1;
        for(int i=0;i<GridSize;i++)
        {
            for(int j=0;j<GridSize;j++)
            {
                if (MainChunks[k][i][j].GetComponent<BlocksV2>()!=null && MainChunks[k][i][j].GetComponent<BlocksV2>().ID==0)
                {
                    int orient = (MainChunks[k][i][j + 1].GetComponent<BlocksV2>().ID == 0 ? 0 : 1) * 1 + (MainChunks[k][i + 1][j].GetComponent<BlocksV2>().ID == 0 ? 0 : 1) * 10 + (MainChunks[k][i][j - 1].GetComponent<BlocksV2>().ID == 0 ? 0 : 1) * 100 + (MainChunks[k][i-1][j].GetComponent<BlocksV2>().ID == 0 ? 0 : 1) * 1000;
                    Debug.Log(orient);
                    int rng = UnityEngine.Random.Range(0, 100);
                    if (rng < 15)
                    {
                        switch (orient)
                        {
                            case 0011:
                                MainChunks[k][i][j].GetComponent<BlocksV2>().InstantiateStair(i -0.5f, j - 0.5f, k, _StairsPrefab, RoomScaleX, RoomScaleY, RoomScaleZ, StairsHolder.transform, -180);
                                break;

                            case 0110:
                                MainChunks[k][i][j].GetComponent<BlocksV2>().InstantiateStair(i -0.5f, j -0.5f, k, _StairsPrefab, RoomScaleX, RoomScaleY, RoomScaleZ, StairsHolder.transform, -90);
                                break;

                            case 1100:
                                MainChunks[k][i][j].GetComponent<BlocksV2>().InstantiateStair(i  -0.5f, j  -0.5f, k, _StairsPrefab, RoomScaleX, RoomScaleY, RoomScaleZ, StairsHolder.transform,0);
                                break;

                            case 1001:
                                MainChunks[k][i][j].GetComponent<BlocksV2>().InstantiateStair(i  -0.5f, j -0.5f, k, _StairsPrefab, RoomScaleX, RoomScaleY, RoomScaleZ, StairsHolder.transform, 90);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}
