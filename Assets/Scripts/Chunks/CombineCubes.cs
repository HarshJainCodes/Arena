using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineCubes : MonoBehaviour
{
    public static CombineCubes Instance { get; private set; }

    /// <summary>
    /// This will hold the combined cubs chunks 
    /// </summary>
    [SerializeField] private GameObject _CombinedCubeHolder;

    /// <summary>
    /// The empty Tile to replace when we cut the block to make asymmetric pattern around the borders
    /// </summary>
    [SerializeField] private GameObject _Empty;

    /// <summary>
    /// All the barriers that needs to be placed on the borders of the arena
    /// </summary>

    [SerializeField] private GameObject _BarrierParent;
    [SerializeField] private GameObject _BarrierLeft;
    [SerializeField] private GameObject _BarrierRight;
    [SerializeField] private GameObject _BarrierUp;
    [SerializeField] private GameObject _BarrierDown;

    /// <summary>
    /// The reference to the Chunk Cretor from the <see cref="ChunkCreator"/> Script
    /// </summary>
    private ChunkCreator _ChunkCreator;

    /// <summary>
    /// This will hold the grid size of our level, grabbed from the <see cref="ChunkCreator"/> Script
    /// </summary>
    private int _GridSize;

    /// <summary>
    /// This will hold the floorCount of our level, grabbed from the <see cref="ChunkCreator"/> Script
    /// </summary>
    private int _FloorCount;

    /// <summary>
    /// This will hold the chunk array of our level that contains all the information of our blocks (this stores the information of the type of block), grabbed from <see cref="ChunkCreator"/> Script
    /// </summary>
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

    /// <summary>
    /// <para> This will cut the cubes to form the asymmetric pattern that we can see on the sides of our blocks </para>
    /// <para> The side is the side which we want to cutoff, its value can be [0, 1, 2, 3] denoting the four sides </para>
    /// <para> This function is designed so that it will modify itself depending on the value of <paramref name="side"/> </para>
    /// <para> This alothough has the disadvantage that this generalizes the function and if you wanted to make particular side do something it would be very difficult to modify this function</para>
    /// <para> Incase you want to do something different to only a specific side than you need to make four function that each handle one side. Refer to <see cref="_AddBarrierAroundCubeUp"/> </para>
    /// </summary>
    /// <param name="side"></param>
    private void _CutCubes(int side)
    {
        int a = 0;

        Vector3Int roomScale = _ChunkCreator.GetRoomScale();

        while (a <= _GridSize)
        {
            int j = Random.Range(1, _ChunkCreator.GetPadding());
            int nextI = Mathf.Min(_GridSize - 1, Random.Range(a + 4, a + 8));
            for (int ii = a; ii < nextI; ii++)
            {
                int minJ;
                int maxJ;

                if (side == 0 || side == 2)
                {
                    minJ = 0;
                    maxJ = j;
                }
                else
                {
                    minJ = _GridSize - j;
                    maxJ = _GridSize;
                }

                for (int jj = minJ; jj < maxJ; jj++)
                {
                    int minK = Random.Range(0, _FloorCount - 1);
                    int maxK = Random.Range(minK, _FloorCount + 1);
                    for (int k = minK; k < maxK; k++)
                    {
                        Block b;
                        if (side == 0 || side == 1)
                        {
                            b = _ChunkArray[ii, jj, k].GetComponent<Block>();
                        }
                        else
                        {
                            b = _ChunkArray[jj, ii, k].GetComponent<Block>();
                        }
                        
                        if (b.ID == 0)
                        {
                            b.UnCollapse();
                            if (side == 0 || side == 1)
                            {
                                _ChunkArray[ii, jj, k] = new GameObject();
                                _ChunkArray[ii, jj, k].transform.parent = transform;
                                Block newB = _ChunkArray[ii, jj, k].AddComponent<Block>();

                                newB.InitializeBlock(ii, jj, k, _Empty, roomScale.x, roomScale.y, roomScale.z, 2);
                                newB.SetCollapsed(transform);
                            }
                            else
                            {
                                _ChunkArray[jj, ii, k] = new GameObject();
                                _ChunkArray[jj, ii, k].transform.parent = transform;
                                Block newB = _ChunkArray[jj, ii, k].AddComponent<Block>();

                                newB.InitializeBlock(jj, ii, k, _Empty, roomScale.x, roomScale.y, roomScale.z, 2);
                                newB.SetCollapsed(transform);
                            }
                        }
                    }
                }
            }
            a = nextI + Random.Range(2, 5);
            if (a >= _GridSize - 1)
            {
                break;
            }
        }

    }

    /// <summary>
    /// Checks if the i, j, k values lies in the bounds of the array <see cref="_ChunkArray"/>
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="k"></param>
    /// <returns>
    /// true or false signifying if the i, j, k are valid or not
    /// </returns>
    private bool _IsValid(int i, int j, int k)
    {
        if (i < 0 || j < 0 || k < 0 || i >= _GridSize || j >= _GridSize || k >= _FloorCount)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// This function will add barriers around the whole genreated cubes so that the player dosnt fall off the arena.
    /// <para> The way this works in you scan the whole padding (<see cref="ChunkCreator.GetPadding"/>) area of the <see cref="_ChunkArray"/> see if there is any <see cref="_Empty"/> Block.
    /// <para> If there are empty blocks than check [up, down, left, right] to see if there are any Wall Block or not (the one with ID = 1). </para>>
    /// <para> If there are Wall blocks than spawn a barrier on the same coordinated as of the blocks but on z + 1  as we want the barrier to be above the wall block </para>
    /// </para>
    /// <para> </para>
    /// </summary>
    private void _AddBarrierAroundCubeUp()
    {
        Vector3Int gridScale = _ChunkCreator.GetRoomScale();

        for (int i = 0; i < _GridSize; i++)
        {
            for (int j = _GridSize - _ChunkCreator.GetPadding(); j < _GridSize; j++)
            {
                Block b = _ChunkArray[i, j, 5].GetComponent<Block>();

                if (b.ID == 2)  // if it is an empty block
                {
                    if (_IsValid(i - 1, j, 5) && _ChunkArray[i - 1, j, 5].GetComponent<Block>().ID == 0)    // left
                    {
                        GameObject barrierLeftGO = Instantiate(_BarrierLeft, new Vector3((i - 1) * gridScale.x, 6.5f * gridScale.z, j * gridScale.y), Quaternion.identity);
                        barrierLeftGO.transform.parent = _BarrierParent.transform;
                        barrierLeftGO.transform.localScale = gridScale;
                    }
                    if (_IsValid(i + 1, j, 5) && _ChunkArray[i + 1, j, 5].GetComponent<Block>().ID == 0)    // right
                    {
                        GameObject barrierRightGO = Instantiate(_BarrierRight, new Vector3((i + 1) * gridScale.x, 6.5f * gridScale.z, j * gridScale.y), Quaternion.identity);
                        barrierRightGO.transform.parent = _BarrierParent.transform;
                        barrierRightGO.transform.localScale = gridScale;
                    }
                    if (_IsValid(i, j + 1, 5) && _ChunkArray[i, j + 1, 5].GetComponent<Block>().ID == 0)    // up
                    {
                        GameObject barrierUPGO = Instantiate(_BarrierUp, new Vector3(i * gridScale.x, 6.5f * gridScale.z, (j + 1) * gridScale.y), Quaternion.identity);
                        barrierUPGO.transform.parent = _BarrierParent.transform;
                        barrierUPGO.transform.localScale = gridScale;
                    }
                    if (_IsValid(i, j - 1, 5) && _ChunkArray[i, j - 1, 5].GetComponent<Block>().ID == 0)    // down
                    {
                        GameObject barrierDownGO = Instantiate(_BarrierDown, new Vector3(i * gridScale.x, 6.5f * gridScale.z, (j - 1) * gridScale.y), Quaternion.identity);
                        barrierDownGO.transform.parent = _BarrierParent.transform;
                        barrierDownGO.transform.localScale = gridScale;
                    }
                }
                else if (j == _GridSize - 1 && b.ID == 0)    // if it is an block
                {
                    GameObject barrierUpGO1 = Instantiate(_BarrierUp, new Vector3(i * gridScale.x, 6.5f * gridScale.z, (j + 1) * gridScale.y), Quaternion.identity);
                    barrierUpGO1.transform.parent = _BarrierParent.transform;
                    barrierUpGO1.transform.localScale = gridScale;
                }
            }
        }
    }

    /// <summary>
    /// same as <see cref="_AddBarrierAroundCubeUp"/> but for the bottom
    /// </summary>
    private void _AddBarrierAroundCubeBottom()
    {

        Vector3Int gridScale = _ChunkCreator.GetRoomScale();

        for (int i = 0; i < _GridSize; i++)
        {
            for (int j = 0; j < _ChunkCreator.GetPadding(); j++)
            {
                Block b = _ChunkArray[i, j, 5].GetComponent<Block>();

                if (b.ID == 2)  // if it is an empty block
                {
                    if (_IsValid(i - 1, j, 5) && _ChunkArray[i - 1, j, 5].GetComponent<Block>().ID == 0)
                    {
                        GameObject barrierLeftGO = Instantiate(_BarrierLeft, new Vector3((i - 1) * gridScale.x, 6.5f * gridScale.z, j * gridScale.y), Quaternion.identity);
                        barrierLeftGO.transform.parent = _BarrierParent.transform;
                        barrierLeftGO.transform.localScale = gridScale;
                    }if (_IsValid(i + 1, j, 5) && _ChunkArray[i + 1, j, 5].GetComponent<Block>().ID == 0)
                    {
                        GameObject barrierRightGO = Instantiate(_BarrierRight, new Vector3((i + 1) * gridScale.x, 6.5f * gridScale.z, j * gridScale.y), Quaternion.identity);
                        barrierRightGO.transform.parent = _BarrierParent.transform;
                        barrierRightGO.transform.localScale = gridScale;
                    }if (_IsValid(i, j + 1, 5) && _ChunkArray[i, j + 1, 5].GetComponent<Block>().ID == 0)
                    {
                        GameObject barrierUPGO = Instantiate(_BarrierUp, new Vector3(i * gridScale.x, 6.5f * gridScale.z, (j + 1) * gridScale.y), Quaternion.identity);
                        barrierUPGO.transform.parent = _BarrierParent.transform;
                        barrierUPGO.transform.localScale = gridScale;
                    }if (_IsValid(i, j - 1, 5) && _ChunkArray[i, j - 1, 5].GetComponent<Block>().ID == 0)
                    {
                        GameObject barrierDownGO = Instantiate(_BarrierDown, new Vector3(i * gridScale.x, 6.5f * gridScale.z, (j - 1) * gridScale.y), Quaternion.identity);
                        barrierDownGO.transform.parent = _BarrierParent.transform;
                        barrierDownGO.transform.localScale = gridScale;
                    }
                }else if (j == 0 && b.ID == 0)    // if it is an block
                {
                    GameObject barrierDownGO1 = Instantiate(_BarrierDown, new Vector3(i * gridScale.x, 6.5f * gridScale.z, (j - 1) * gridScale.y), Quaternion.identity);
                    barrierDownGO1.transform.parent = _BarrierParent.transform;
                    barrierDownGO1.transform.localScale = gridScale;
                }
            }
        }
    }

    /// <summary>
    /// same as <see cref="_AddBarrierAroundCubeUp"/> but for left
    /// </summary>
    private void _AddBarrierAroundCubeLeft()
    {
        Vector3Int gridScale = _ChunkCreator.GetRoomScale();

        for (int i = 0; i < _GridSize; i++)
        {
            for (int j = 0; j < _ChunkCreator.GetPadding(); j++)
            {
                Block b = _ChunkArray[j, i, 5].GetComponent<Block>();

                if (b.ID == 2)  // if it is an empty block
                {
                    if (_IsValid(j - 1, i, 5) && _ChunkArray[j - 1, i, 5].GetComponent<Block>().ID == 0)
                    {
                        GameObject barrierLeftGO = Instantiate(_BarrierLeft, new Vector3((j - 1) * gridScale.x, 6.5f * gridScale.z, i * gridScale.y), Quaternion.identity);
                        barrierLeftGO.transform.parent = _BarrierParent.transform;
                        barrierLeftGO.transform.localScale = gridScale;
                    }if (_IsValid(j + 1, i, 5) && _ChunkArray[j + 1, i, 5].GetComponent<Block>().ID == 0)
                    {
                        GameObject barrierRightGO = Instantiate(_BarrierRight, new Vector3((j + 1) * gridScale.x, 6.5f * gridScale.z, i * gridScale.y), Quaternion.identity);
                        barrierRightGO.transform.parent = _BarrierParent.transform;
                        barrierRightGO.transform.localScale = gridScale;
                    }if (_IsValid(j, i + 1, 5) && _ChunkArray[j, i + 1, 5].GetComponent<Block>().ID == 0)
                    {
                        GameObject barrierUpGO = Instantiate(_BarrierUp, new Vector3(j * gridScale.x, 6.5f * gridScale.z, (i + 1) * gridScale.y), Quaternion.identity);
                        barrierUpGO.transform.parent = _BarrierParent.transform;
                        barrierUpGO.transform.localScale = gridScale;
                    }if (_IsValid(j, i - 1, 5) && _ChunkArray[j, i - 1, 5].GetComponent<Block>().ID == 0)
                    {
                        GameObject barrierDownGO = Instantiate(_BarrierDown, new Vector3(j * gridScale.x, 6.5f * gridScale.z, (i - 1) * gridScale.y), Quaternion.identity);
                        barrierDownGO.transform.parent = _BarrierParent.transform;
                        barrierDownGO.transform.localScale = gridScale;
                    }
                }else if (j == 0 && b.ID == 0) // if it is an block 
                {
                    GameObject barrierleft1 = Instantiate(_BarrierLeft, new Vector3((j - 1) * gridScale.x, 6.5f * gridScale.z, i * gridScale.y), Quaternion.identity);
                    barrierleft1.transform.parent = _BarrierParent.transform;
                    barrierleft1.transform.localScale = gridScale;
                }
            }
        }
    }

    /// <summary>
    /// same as <see cref="_AddBarrierAroundCubeUp"/> but for right
    /// </summary>
    private void _AddBarrierAroundCubeRight()
    {
        Vector3Int gridScale = _ChunkCreator.GetRoomScale();

        for (int i = 0; i < _GridSize; i++)
        {
            for (int j = _GridSize - _ChunkCreator.GetPadding() - 1; j < _GridSize; j++)
            {
                Block b = _ChunkArray[j, i, 5].GetComponent<Block>();

                if (b.ID == 2)  // if it is an empty block
                {
                    if (_IsValid(j - 1, i, 5) && _ChunkArray[j - 1, i, 5].GetComponent<Block>().ID == 0)
                    {
                        GameObject barrierLeftGO = Instantiate(_BarrierLeft, new Vector3((j - 1) * gridScale.x, 6.5f * gridScale.z, i * gridScale.y), Quaternion.identity);
                        barrierLeftGO.transform.parent = _BarrierParent.transform;
                        barrierLeftGO.transform.localScale = gridScale;
                    }
                    if (_IsValid(j + 1, i, 5) && _ChunkArray[j + 1, i, 5].GetComponent<Block>().ID == 0)
                    {
                        GameObject barrierRightGO = Instantiate(_BarrierRight, new Vector3((j + 1) * gridScale.x, 6.5f * gridScale.z, i * gridScale.y), Quaternion.identity);
                        barrierRightGO.transform.parent = _BarrierParent.transform;
                        barrierRightGO.transform.localScale = gridScale;
                    }
                    if (_IsValid(j, i + 1, 5) && _ChunkArray[j, i + 1, 5].GetComponent<Block>().ID == 0)
                    {
                        GameObject barrierUpGO = Instantiate(_BarrierUp, new Vector3(j * gridScale.x, 6.5f * gridScale.z, (i + 1) * gridScale.y), Quaternion.identity);
                        barrierUpGO.transform.parent = _BarrierParent.transform;
                        barrierUpGO.transform.localScale = gridScale;
                    }
                    if (_IsValid(j, i - 1, 5) && _ChunkArray[j, i - 1, 5].GetComponent<Block>().ID == 0)
                    {
                        GameObject barrierDownGO = Instantiate(_BarrierDown, new Vector3(j * gridScale.x, 6.5f * gridScale.z, (i - 1) * gridScale.y), Quaternion.identity);
                        barrierDownGO.transform.parent = _BarrierParent.transform;
                        barrierDownGO.transform.localScale = gridScale;
                    }
                }
                else if (j == _GridSize - 1 && b.ID == 0)
                {
                    GameObject barrierRight1 = Instantiate(_BarrierRight, new Vector3((j + 1) * gridScale.x, 6.5f * gridScale.z, i * gridScale.y), Quaternion.identity);
                    barrierRight1.transform.parent = _BarrierParent.transform;
                    barrierRight1.transform.localScale = gridScale;
                }
            }
        }
    }

    /// <summary>
    /// This function will first cut the cubes, than add the barriers around them, and then after that cut the cubes into square chunks so that they are ready to combine and animate
    /// </summary>
    public void MakeCubeSegments()
    {
        _ChunkArray = _ChunkCreator.ChunkArray;

        // double iterate to produce more random and asymmetric pattern.

        _CutCubes(0);
        _CutCubes(0);

        _CutCubes(1);
        _CutCubes(1);

        _CutCubes(2);
        _CutCubes(2);

        _CutCubes(3);
        _CutCubes(3);

        // this will add the blocks around the whole arena on the borders of the chunks so that the player dosnt fall off it.
        _AddBarrierAroundCubeBottom();
        _AddBarrierAroundCubeUp();
        _AddBarrierAroundCubeLeft();
        _AddBarrierAroundCubeRight();

        // This part will put the cubes into 10 x 10 (for now) chunks so that they can be combined in a single mesh
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

    /// <summary>
    /// This will combine the cuted cube segments from <see cref="MakeCubeSegments"/>
    /// </summary>
    public void CombineCubeSegments()
    {
        for (int i = 0; i < _CombinedCubeHolder.transform.childCount; i++)
        {
            _CombinedCubeHolder.transform.GetChild(i).GetComponent<MeshCombinerScript>().CombineMesh();
        }
    }
}