using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ChunkCreator : MonoBehaviour
{

    private GameObject[,,] ChunkArray;
    [SerializeField] private GameObject[] _Tiles;
    [SerializeField] private List<BlendBlocks> _BlendBlocks = new List<BlendBlocks>();

    [SerializeField] private int GridSize = 10;
    [SerializeField] private int Padding = 2;
    [SerializeField] private int FloorCount = 2;
    [SerializeField] private int MinRoomCount = 3;
    [SerializeField] private int MaxRoomCount = 5;

    [SerializeField] private int RoomScaleX = 1;
    [SerializeField] private int RoomScaleY = 1;
    [SerializeField] private int RoomScaleZ = 1;

    [SerializeField] private int _CentralRoomSize;
    [SerializeField] private int _CentralRoomSizeX;
    [SerializeField] private int _CentralRoomSizeY;
    [SerializeField] private int _CentralRoomCoordX;

    [SerializeField] private GameObject ChunkHolder;
    [SerializeField] private GameObject FloorHolder;
    [SerializeField] private GameObject StairsHolder;
    [SerializeField] private GameObject EmptyHolder;

    private System.Random _Rnd = new System.Random();

    private Queue<int[]> queue = new Queue<int[]>();

    // Start is called before the first frame update
    void Start()
    {
        ChunkArray = new GameObject[GridSize, GridSize, FloorCount];
        _CentralRoomCoordX = GridSize / 2;
        _CentralRoomSize = _CentralRoomSize > GridSize ? (GridSize / 2) - Padding : _CentralRoomSize;

        FloorCreator();

        CoverRest();

        while (queue.Any())
        {
            ExecuteQueue();
        }

        CentralRoomCreater();

        ChangeOrientationOfBlocks();
    }

    private void FloorCreator()
    {
        for (int i = 0; i < FloorCount; i++)
        {
            int max = _Rnd.Next(MinRoomCount, MaxRoomCount);
            for (int j = 0; j < max; j++)
            {
                CollapseRoom(i);
            }
        }
    }

    private void CollapseRoom(int floor)
    {
        int grid_rnd_start_x = _Rnd.Next(Padding, GridSize - Padding);
        int grid_rnd_start_y = _Rnd.Next(Padding, GridSize - Padding);

        int grid_rnd_height = _Rnd.Next(1, Padding);
        int grid_rnd_width = _Rnd.Next(1, Padding);

        Addqueue(grid_rnd_start_x + _Rnd.Next(1, grid_rnd_height), grid_rnd_start_y + _Rnd.Next(1, grid_rnd_width), floor);

        for (int i = grid_rnd_start_x; i < ((grid_rnd_start_x + grid_rnd_width) > GridSize ? GridSize : (grid_rnd_start_x + grid_rnd_width)); i++)
        {
            for (int j = grid_rnd_start_y; j < ((grid_rnd_start_y + grid_rnd_height) > GridSize ? GridSize : (grid_rnd_start_y + grid_rnd_height)); j++)
            {
                if (ChunkArray[i, j, floor] == null)
                {
                    ChunkArray[i, j, floor] = new GameObject();
                    ChunkArray[i, j, floor].transform.parent = transform;
                    ChunkArray[i, j, floor].AddComponent<Block>();

                    Block b = ChunkArray[i, j, floor].GetComponent<Block>();
                    b.InitializeBlock(i, j, floor, _Tiles[1], RoomScaleX, RoomScaleY, RoomScaleZ, 1);
                    b.SetCollapsed(FloorHolder.transform);
                }
            }
        }
    }

    private void CoverRest()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                for (int  k = 0; k < FloorCount; k++)
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
                        if (ChunkArray[i + 1 < GridSize ? i + 1 : i, j, k] == null)
                            orient[2] = 0;
                        else if (ChunkArray[i + 1 < GridSize ? i + 1 : i, j, k].GetComponent<Block>().ID == 1)
                            orient[2] = 1;
                        else
                            orient[2] = 0;
                        if (ChunkArray[i, j + 1 > GridSize ? j + 1 : j, k] == null)
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
                        b.InitializeBlock(i, j, k, obj, RoomScaleX, RoomScaleY, RoomScaleZ, 0);
                        b.SetCollapsed(ChunkHolder.transform);
                        b.Rotate(rot[0], rot[2], rot[1]);
                    }
                }
            }
        }
    }

    private void ExecuteQueue()
    {
        int[] coordinate = queue.Peek();

        float xdirection = ((GridSize / 2) - coordinate[0]) / (GridSize / 2);
        float ydirection = ((GridSize / 2) - coordinate[1]) / (GridSize / 2);
        int z_path = coordinate[2];
        int x_create_direction = xdirection >= 0 ? 1 : -1;
        int y_create_direction = ydirection >= 0 ? 1 : -1;
        int x_path = coordinate[0];
        int y_path = coordinate[1];
        int x_origin = coordinate[0];
        int y_origin = coordinate[1];
        int z_origin = coordinate[2];
        bool flip = false;

        while (x_path < GridSize - Padding && x_path > Padding)
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
            b.InitializeBlock(x_path, coordinate[1], coordinate[2], _Tiles[1], RoomScaleX, RoomScaleY, RoomScaleZ, 1);
            b.SetCollapsed(FloorHolder.transform);

            x_path = x_path + x_create_direction;
        }
        flip = false;
        while (y_path < GridSize - Padding && y_path > Padding)
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
            b.InitializeBlock(coordinate[0], y_path, z_path, _Tiles[1], RoomScaleX, RoomScaleY, RoomScaleZ, 1);
            b.SetCollapsed(FloorHolder.transform);

            y_path = y_path + y_create_direction;
        }
        queue.Dequeue();

        if (z_origin != FloorCount - 1)
        {
            if (ChunkArray[x_origin, y_origin, z_origin + 1].GetComponent<Block>().ID == 1)
            {
                //Debug.Log("This is executed");
                ChunkArray[x_origin, y_origin, z_origin].GetComponent<Block>().UnCollapse();
                ChunkArray[x_origin, y_origin, z_origin] = new GameObject();
                ChunkArray[x_origin, y_origin, z_origin].transform.parent = transform;
                ChunkArray[x_origin, y_origin, z_origin].AddComponent<Block>();

                Block b = ChunkArray[x_origin, y_origin, z_origin].GetComponent<Block>();
                b.InitializeBlock(x_origin, y_origin, z_origin, _Tiles[3], RoomScaleX, RoomScaleY, RoomScaleZ, 3);
                b.SetCollapsed(StairsHolder.transform);

                ChunkArray[x_origin, y_origin, z_origin + 1].GetComponent<Block>().UnCollapse();
                ChunkArray[x_origin, y_origin, z_origin + 1] = new GameObject();
                ChunkArray[x_origin, y_origin, z_origin + 1].transform.parent = transform;
                ChunkArray[x_origin, y_origin, z_origin + 1].AddComponent<Block>();

                Block c = ChunkArray[x_origin, y_origin, z_origin + 1].GetComponent<Block>();
                c.InitializeBlock(x_origin, y_origin, z_origin + 1, _Tiles[2], RoomScaleX, RoomScaleY, RoomScaleZ, 2);
                c.SetCollapsed(EmptyHolder.transform);
            }
        }
    }

    private void CentralRoomCreater()
    {
        int step = 1;

        for (int k = 0; k < FloorCount; k++)
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
                        p.InitializeBlock(i, j, k, _Tiles[1], RoomScaleX, RoomScaleY, RoomScaleZ, 1);
                        p.SetCollapsed(FloorHolder.transform);
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
                            p.InitializeBlock(i, j, k, _Tiles[1], RoomScaleX, RoomScaleY, RoomScaleZ, 1);
                            p.SetCollapsed(FloorHolder.transform);
                        }
                        else
                        {
                            p.InitializeBlock(i, j, k, _Tiles[2], RoomScaleX, RoomScaleY, RoomScaleZ, 2);
                            p.SetCollapsed(EmptyHolder.transform);
                        }
                    }
                }
            }
            step = step + 1;
        }
    }

    private void ChangeOrientationOfBlocks()
    {
        for (int i = 1; i < GridSize - 1; i++)
        {
            for (int j = 1; j < GridSize - 1; j++)
            {
                for (int k = 0; k < FloorCount; k++)
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
                        if (ChunkArray[i + 1 < GridSize ? i + 1 : i, j, k].GetComponent<Block>().ID == 1)
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
                        b.InitializeBlock(i, j, k, obj, RoomScaleX, RoomScaleY, RoomScaleZ, 0);
                        b.SetCollapsed(ChunkHolder.transform);
                        b.Rotate(rot[0], rot[2], rot[1]);
                    }
                }
            }
        }
    }

    private void Addqueue(int x, int y, int z)
    {
        int[] arr = new int[3];
        arr[0] = x;
        arr[1] = y;
        arr[2] = z;
        queue.Enqueue(arr);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
