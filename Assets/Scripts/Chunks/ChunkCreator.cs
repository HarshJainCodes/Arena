using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ChunkCreator : MonoBehaviour
{
    /// <summary>
    /// This Array of GameObject of size [<see cref="_GridSize"/>, <see cref="_GridSize"/>, <see cref="_FloorCount"/>] will store GameObjects that will have the <see cref="Block"/> script attached to it.
    /// This change is done so that <see cref="GameObject.Find(string)"/> can be avoided hence it is quite expensive.
    /// </summary>
    public GameObject[,,] ChunkArray;

    /// <summary>
    /// This will store tiles (Wall, floorParent, Empty, Stairs) that will be used to reference prefabs for Instantiating.
    /// </summary>
    [SerializeField] private GameObject[] _Tiles;

    /// <summary>
    /// This will store the various prefabs of the walls (1 side, 2 side, 3 side, 4 side, parallel, top_bottom) that will be used to Instantiate in <see cref="Block.InstantiatePrefab(Transform)"/>
    /// </summary>
    [SerializeField] private List<BlendBlocks> _BlendBlocks = new List<BlendBlocks>();

    [Tooltip("The size of the grid. Grid is generally (GridSize x GridSize)")]
    [SerializeField] private int _GridSize = 10;
    public int GetGridSize()
    {
        return _GridSize;
    }

    [Tooltip("the padding between the blocks")]
    [SerializeField] private int _Padding = 2;
    [Tooltip("Number of floors in the arena")]
    [SerializeField] private int _FloorCount = 2;
    public int GetFloorCount()
    {
        return _FloorCount;
    }

    [Tooltip("Minimum number of floors")]
    [SerializeField] private int _MinRoomCount = 3;
    [Tooltip("Maximum number of floors")]
    [SerializeField] private int _MaxRoomCount = 5;

    [Tooltip("Room Scale X")]
    [SerializeField] private int _RoomScaleX = 1;
    [Tooltip("Room Scale Y")]
    [SerializeField] private int _RoomScaleY = 1;
    [Tooltip("Room Scale Z")]
    [SerializeField] private int _RoomScaleZ = 1;

    [SerializeField] private int _CentralRoomSize;
    [SerializeField] private int _CentralRoomSizeX;
    [SerializeField] private int _CentralRoomSizeY;
    [SerializeField] private int _CentralRoomCoordX;

    [Tooltip("This will hold all the runtime instantiated chunks")]
    [SerializeField] private GameObject _ChunkHolder;
    [Tooltip("This will hold all the runtime instantiated floors")]
    [SerializeField] private GameObject _FloorHolder;
    [Tooltip("This will hold all the runtime instantiated stairs")]
    [SerializeField] private GameObject _StairsHolder;
    [Tooltip("This will hold all the runtime instantiated empty ")]
    [SerializeField] private GameObject _EmptyHolder;

    private System.Random _Rnd = new System.Random();

    private Queue<int[]> _Queue = new Queue<int[]>();

    /// <summary>
    /// this is required as this cant be put in Start cuz if Other Script's Start Method is called first than it will break as it has dependencies on this script
    /// </summary>
    private bool _HasRun = false;

    // Start is called before the first frame update
    void Start()
    {
        ChunkArray = new GameObject[_GridSize, _GridSize, _FloorCount];
        _CentralRoomCoordX = _GridSize / 2;
        _CentralRoomSize = _CentralRoomSize > _GridSize ? (_GridSize / 2) - _Padding : _CentralRoomSize;

        FloorCreator();

        CoverRest();

        while (_Queue.Any())
        {
            ExecuteQueue();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void FloorCreator()
    {
        for (int i = 0; i < _FloorCount; i++)
        {
            int max = _Rnd.Next(_MinRoomCount, _MaxRoomCount);
            for (int j = 0; j < max; j++)
            {
                CollapseRoom(i);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="floor"></param>
    private void CollapseRoom(int floor)
    {
        int grid_rnd_start_x = _Rnd.Next(_Padding, _GridSize - _Padding);
        int grid_rnd_start_y = _Rnd.Next(_Padding, _GridSize - _Padding);

        int grid_rnd_height = _Rnd.Next(1, _Padding);
        int grid_rnd_width = _Rnd.Next(1, _Padding);

        Addqueue(grid_rnd_start_x + _Rnd.Next(1, grid_rnd_height), grid_rnd_start_y + _Rnd.Next(1, grid_rnd_width), floor);

        for (int i = grid_rnd_start_x; i < ((grid_rnd_start_x + grid_rnd_width) > _GridSize ? _GridSize : (grid_rnd_start_x + grid_rnd_width)); i++)
        {
            for (int j = grid_rnd_start_y; j < ((grid_rnd_start_y + grid_rnd_height) > _GridSize ? _GridSize : (grid_rnd_start_y + grid_rnd_height)); j++)
            {
                if (ChunkArray[i, j, floor] == null)
                {
                    ChunkArray[i, j, floor] = new GameObject();
                    ChunkArray[i, j, floor].transform.parent = transform;
                    ChunkArray[i, j, floor].AddComponent<Block>();

                    Block b = ChunkArray[i, j, floor].GetComponent<Block>();
                    b.InitializeBlock(i, j, floor, _Tiles[1], _RoomScaleX, _RoomScaleY, _RoomScaleZ, 1);
                    b.SetCollapsed(_FloorHolder.transform);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CoverRest()
    {
        for (int i = 0; i < _GridSize; i++)
        {
            for (int j = 0; j < _GridSize; j++)
            {
                for (int  k = 0; k < _FloorCount; k++)
                {
                    if (ChunkArray[i, j, k] == null || ChunkArray[i, j, k].GetComponent<Block>().ID == 0)
                    {
                        int[] orient = new int[4];

                        if (ChunkArray[i - 1 > 0 ? i - 1 : i, j, k] == null)
                            orient[0] = 0;
                        else if (ChunkArray[i - 1 > 0 ? i - 1 : i, j, k].GetComponent<Block>().ID == 1)
                            orient[0] = 1;
                        else
                            orient[0] = 0;
                        if (ChunkArray[i, j - 1 > 0 ? j - 1 : j, k] == null)
                            orient[1] = 0;
                        else if (ChunkArray[i, j - 1 > 0 ? j - 1 : j, k].GetComponent<Block>().ID == 1)
                            orient[1] = 1;
                        else
                            orient[1] = 0;
                        if (ChunkArray[i + 1 < _GridSize ? i + 1 : i, j, k] == null)
                            orient[2] = 0;
                        else if (ChunkArray[i + 1 < _GridSize ? i + 1 : i, j, k].GetComponent<Block>().ID == 1)
                            orient[2] = 1;
                        else
                            orient[2] = 0;
                        if (ChunkArray[i, j + 1 > _GridSize ? j + 1 : j, k] == null)
                            orient[3] = 0;
                        else if (ChunkArray[i, j + 1 > 0 ? j + 1 : j, k].GetComponent<Block>().ID == 1)
                            orient[3] = 1;
                        else
                            orient[3] = 0;

                        int sum = orient[0] * 1000 + orient[1] * 100 + orient[2] * 10 + orient[3];

                        GameObject obj = _BlendBlocks[0].blocks[UnityEngine.Random.Range(0, 3)];

                        int[] rot = new int[3];

                        switch (sum)
                        {
                            case 0000:
                                obj = _BlendBlocks[5].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 0;
                                rot[2] = 0;
                                break;
                            case 1000:
                                obj = _BlendBlocks[0].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 90;
                                rot[2] = 0;
                                break;

                            case 0100:
                                obj = _BlendBlocks[0].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 0;
                                rot[2] = 0;
                                break;

                            case 0010:
                                obj = _BlendBlocks[0].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = -90;
                                rot[2] = 0;
                                break;

                            case 0001:
                                obj = _BlendBlocks[0].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = -180;
                                rot[2] = 0;
                                break;

                            case 1100:
                                obj = _BlendBlocks[1].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 0;
                                rot[2] = 0;
                                break;

                            case 0110:
                                obj = _BlendBlocks[1].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = -90;
                                rot[2] = 0;
                                break;

                            case 0011:
                                obj = _BlendBlocks[1].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = -180;
                                rot[2] = 0;
                                break;

                            case 1001:
                                obj = _BlendBlocks[1].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 90;
                                rot[2] = 0;
                                break;

                            case 1010:
                                obj = _BlendBlocks[4].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 90;
                                rot[2] = 0;
                                break;

                            case 0101:
                                obj = _BlendBlocks[4].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 90;
                                rot[2] = 0;
                                break;

                            case 1110:
                                obj = _BlendBlocks[2].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 0;
                                rot[2] = 0;
                                break;

                            case 0111:
                                obj = _BlendBlocks[2].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = -90;
                                rot[2] = 0;
                                break;

                            case 1011:
                                obj = _BlendBlocks[2].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = -180;
                                rot[2] = 0;
                                break;

                            case 1101:
                                obj = _BlendBlocks[2].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 90;
                                rot[2] = 0;
                                break;

                            case 1111:
                                obj = _BlendBlocks[3].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 0;
                                rot[2] = 0;
                                break;

                            default:
                                break;
                        }
                        ChunkArray[i, j, k] = new GameObject();
                        ChunkArray[i, j, k].transform.parent = transform;
                        ChunkArray[i, j, k].AddComponent<Block>();

                        Block b = ChunkArray[i, j, k].GetComponent<Block>();
                        b.InitializeBlock(i, j, k, obj, _RoomScaleX, _RoomScaleY, _RoomScaleZ, 0);
                        b.SetCollapsed(_ChunkHolder.transform);
                        b.Rotate(rot[0], rot[2], rot[1]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ExecuteQueue()
    {
        int[] coordinate = _Queue.Peek();

        float xdirection = ((_GridSize / 2) - coordinate[0]) / (_GridSize / 2);
        float ydirection = ((_GridSize / 2) - coordinate[1]) / (_GridSize / 2);
        int z_path = coordinate[2];
        int x_create_direction = xdirection >= 0 ? 1 : -1;
        int y_create_direction = ydirection >= 0 ? 1 : -1;
        int x_path = coordinate[0];
        int y_path = coordinate[1];
        int x_origin = coordinate[0];
        int y_origin = coordinate[1];
        int z_origin = coordinate[2];
        bool flip = false;

        while (x_path < _GridSize - _Padding && x_path > _Padding)
        {
            if (ChunkArray[x_path, coordinate[1], coordinate[2]].GetComponent<Block>().ID != 1)
            {
                flip = true;
            }
            if (flip)
            {
                if (ChunkArray[x_path, coordinate[1], coordinate[2]].GetComponent<Block>().ID == 1)
                {
                    /*chunks[x_path, coordinate[1], coordinate[2]].un_collapse();
                    chunks[x_path, coordinate[1], coordinate[2]] = new block(x_path, coordinate[1], coordinate[2], tiles[4], room_x_scale, room_y_scale, room_z_scale, 4);
                    chunks[x_path, coordinate[1], coordinate[2]].set_collapsed();
                    chunks[coordinate[0], y_path, coordinate[2]].rotate(-90, 0, x_create_direction==-1?180:0);
                    x_path = x_path + x_create_direction;*/
                    break;
                }
            }
            ChunkArray[x_path, coordinate[1], coordinate[2]].GetComponent<Block>().UnCollapse();

            ChunkArray[x_path, coordinate[1], coordinate[2]] = new GameObject();
            ChunkArray[x_path, coordinate[1], coordinate[2]].transform.parent = transform;
            ChunkArray[x_path, coordinate[1], coordinate[2]].AddComponent<Block>();

            Block b = ChunkArray[x_path, coordinate[1], coordinate[2]].GetComponent<Block>();
            b.InitializeBlock(x_path, coordinate[1], coordinate[2], _Tiles[1], _RoomScaleX, _RoomScaleY, _RoomScaleZ, 1);
            b.SetCollapsed(_FloorHolder.transform);

            x_path = x_path + x_create_direction;
        }
        flip = false;
        while (y_path < _GridSize - _Padding && y_path > _Padding)
        {
            if (ChunkArray[coordinate[0], y_path, z_path].GetComponent<Block>().Current == _Tiles[1] && flip)
            {
                break;
            }
            if (ChunkArray[coordinate[0], y_path, z_path].GetComponent<Block>().Current == _Tiles[1])
            {
                flip = true;
            }
            ChunkArray[coordinate[0], y_path, z_path].GetComponent<Block>().UnCollapse();

            ChunkArray[coordinate[0], y_path, z_path] = new GameObject();
            ChunkArray[coordinate[0], y_path, z_path].transform.parent = transform;
            ChunkArray[coordinate[0], y_path, z_path].AddComponent<Block>();

            Block b = ChunkArray[coordinate[0], y_path, z_path].GetComponent<Block>();
            b.InitializeBlock(coordinate[0], y_path, z_path, _Tiles[1], _RoomScaleX, _RoomScaleY, _RoomScaleZ, 1);
            b.SetCollapsed(_FloorHolder.transform);

            y_path = y_path + y_create_direction;
        }
        _Queue.Dequeue();

        if (z_origin != _FloorCount - 1)
        {
            if (ChunkArray[x_origin, y_origin, z_origin + 1].GetComponent<Block>().ID == 1)
            {
                //Debug.Log("This is executed");
                ChunkArray[x_origin, y_origin, z_origin].GetComponent<Block>().UnCollapse();
                ChunkArray[x_origin, y_origin, z_origin] = new GameObject();
                ChunkArray[x_origin, y_origin, z_origin].transform.parent = transform;
                ChunkArray[x_origin, y_origin, z_origin].AddComponent<Block>();

                Block b = ChunkArray[x_origin, y_origin, z_origin].GetComponent<Block>();
                b.InitializeBlock(x_origin, y_origin, z_origin, _Tiles[3], _RoomScaleX, _RoomScaleY, _RoomScaleZ, 3);
                b.SetCollapsed(_StairsHolder.transform);

                ChunkArray[x_origin, y_origin, z_origin + 1].GetComponent<Block>().UnCollapse();
                ChunkArray[x_origin, y_origin, z_origin + 1] = new GameObject();
                ChunkArray[x_origin, y_origin, z_origin + 1].transform.parent = transform;
                ChunkArray[x_origin, y_origin, z_origin + 1].AddComponent<Block>();

                Block c = ChunkArray[x_origin, y_origin, z_origin + 1].GetComponent<Block>();
                c.InitializeBlock(x_origin, y_origin, z_origin + 1, _Tiles[2], _RoomScaleX, _RoomScaleY, _RoomScaleZ, 2);
                c.SetCollapsed(_EmptyHolder.transform);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CentralRoomCreater()
    {
        int step = 1;

        for (int k = 0; k < _FloorCount; k++)
        {
            for (int i = _CentralRoomCoordX - _CentralRoomSizeX /*-step*/; i < _CentralRoomCoordX + _CentralRoomSizeX /*+ step*/; i++)
            {
                for (int j = _CentralRoomCoordX - _CentralRoomSizeY /*- step*/; j < _CentralRoomCoordX + _CentralRoomSizeY /*+ step*/; j++)
                {
                    if (i == _CentralRoomCoordX - _CentralRoomSizeX || i == _CentralRoomCoordX + _CentralRoomSizeY - 1 || j == _CentralRoomCoordX - _CentralRoomSizeY || j == _CentralRoomCoordX + _CentralRoomSizeY - 1)
                    {
                        ChunkArray[i, j, k].GetComponent<Block>().UnCollapse();
                        ChunkArray[i, j, k] = new GameObject();
                        ChunkArray[i, j, k].transform.parent = transform;

                        ChunkArray[i, j, k].AddComponent<Block>();
                        Block p = ChunkArray[i, j, k].GetComponent<Block>();
                        p.InitializeBlock(i, j, k, _Tiles[1], _RoomScaleX, _RoomScaleY, _RoomScaleZ, 1);
                        p.SetCollapsed(_FloorHolder.transform);
                    }
                    else
                    {
                        ChunkArray[i, j, k].GetComponent<Block>().UnCollapse();
                        ChunkArray[i, j, k] = new GameObject();
                        ChunkArray[i, j, k].transform.parent = transform;
                        ChunkArray[i, j, k].AddComponent<Block>();

                        Block p = ChunkArray[i, j, k].GetComponent<Block>();
                        if (k == 0)
                        {
                            p.InitializeBlock(i, j, k, _Tiles[1], _RoomScaleX, _RoomScaleY, _RoomScaleZ, 1);
                            p.SetCollapsed(_FloorHolder.transform);
                        }
                        else
                        {
                            p.InitializeBlock(i, j, k, _Tiles[2], _RoomScaleX, _RoomScaleY, _RoomScaleZ, 2);
                            p.SetCollapsed(_EmptyHolder.transform);
                        }
                    }
                }
            }
            step = step + 1;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ChangeOrientationOfBlocks()
    {
        for (int i = 1; i < _GridSize - 1; i++)
        {
            for (int j = 1; j < _GridSize - 1; j++)
            {
                for (int k = 0; k < _FloorCount; k++)
                {
                    if (ChunkArray[i, j, k].GetComponent<Block>().ID == 0)
                    {
                        int[] orient = new int[4];

                        if (ChunkArray[i - 1 > 0 ? i - 1 : i, j, k].GetComponent<Block>().ID == 1)
                            orient[0] = 1;
                        else
                            orient[0] = 0;
                        /*if (chunks[i, j - 1 > 0 ? j - 1 : j, k] == null)
                            orient[1] = 0;*/
                        if (ChunkArray[i, j - 1 > 0 ? j - 1 : j, k].GetComponent<Block>().ID == 1)
                            orient[1] = 1;
                        else
                            orient[1] = 0;
                        /*if (chunks[i + 1 < grid_size ? i + 1 : i, j, k] == null)
                            orient[2] = 0;*/
                        if (ChunkArray[i + 1 < _GridSize ? i + 1 : i, j, k].GetComponent<Block>().ID == 1)
                            orient[2] = 1;
                        else
                            orient[2] = 0;
                        /*if (chunks[i, j + 1 > grid_size ? j + 1 : j, k] == null)
                            orient[3] = 0;*/
                        if (ChunkArray[i, j + 1 > 0 ? j + 1 : j, k].GetComponent<Block>().ID == 1)
                            orient[3] = 1;
                        else
                            orient[3] = 0;

                        int sum = orient[0] * 1000 + orient[1] * 100 + orient[2] * 10 + orient[3];
                        GameObject obj = _BlendBlocks[0].blocks[UnityEngine.Random.Range(0, 3)];
                        int[] rot = new int[3];

                        switch (sum)
                        {
                            case 0000:
                                obj = _BlendBlocks[5].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 0;
                                rot[2] = 0;
                                break;

                            case 1000:
                                obj = _BlendBlocks[0].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 90;
                                rot[2] = 0;
                                break;

                            case 0100:
                                obj = _BlendBlocks[0].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 0;
                                rot[2] = 0;
                                break;

                            case 0010:
                                obj = _BlendBlocks[0].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = -90;
                                rot[2] = 0;
                                break;

                            case 0001:
                                obj = _BlendBlocks[0].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = -180;
                                rot[2] = 0;
                                break;

                            case 1100:
                                obj = _BlendBlocks[1].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 0;
                                rot[2] = 0;
                                break;

                            case 0110:
                                obj = _BlendBlocks[1].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = -90;
                                rot[2] = 0;
                                break;

                            case 0011:
                                obj = _BlendBlocks[1].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = -180;
                                rot[2] = 0;
                                break;

                            case 1001:
                                obj = _BlendBlocks[1].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0; 
                                rot[1] = 90;
                                rot[2] = 0;
                                break;

                            case 1010:
                                obj = _BlendBlocks[4].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 90;
                                rot[2] = 0;
                                break;

                            case 0101:
                                obj = _BlendBlocks[4].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 0;
                                rot[2] = 0;
                                break;

                            case 1110:
                                obj = _BlendBlocks[2].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 0;
                                rot[2] = 0;
                                break;

                            case 0111:
                                obj = _BlendBlocks[2].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = -90;
                                rot[2] = 0;
                                break;

                            case 1011:
                                obj = _BlendBlocks[2].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = -180;
                                rot[2] = 0;
                                break;

                            case 1101:
                                obj = _BlendBlocks[2].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 90;
                                rot[2] = 0;
                                break;

                            case 1111:
                                obj = _BlendBlocks[3].blocks[UnityEngine.Random.Range(0, 3)];
                                rot[0] = 0;
                                rot[1] = 0;
                                rot[2] = 0;
                                break;

                            default:
                                break;
                        }

                        ChunkArray[i, j, k].GetComponent<Block>().UnCollapse();
                        ChunkArray[i, j, k] = new GameObject();
                        ChunkArray[i, j, k].transform.parent = transform;
                        ChunkArray[i, j, k].AddComponent<Block>();

                        Block b = ChunkArray[i, j, k].GetComponent<Block>();
                        b.InitializeBlock(i, j, k, obj, _RoomScaleX, _RoomScaleY, _RoomScaleZ, 0);
                        b.SetCollapsed(_ChunkHolder.transform);
                        b.Rotate(rot[0], rot[2], rot[1]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    private void Addqueue(int x, int y, int z)
    {
        int[] arr = new int[3];
        arr[0] = x;
        arr[1] = y;
        arr[2] = z;
        _Queue.Enqueue(arr);
    }

    // Update is called once per frame
    void Update()
    {
        if (_HasRun == false)
        {
            _HasRun = true;
            CentralRoomCreater();

            ChangeOrientationOfBlocks();

            CombineCubes.Instance.MakeCubeSegments();
            CombineCubes.Instance.CombineCubeSegments();
        }
    }
}
