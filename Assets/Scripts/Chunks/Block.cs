using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // the current position of each block
    private int _X;
    private int _Y;
    private int _Z;

    /// <summary>
    /// current is the gameobject that needs to be instantiated
    /// , this is assigned randomly 
    /// </summary>
    public GameObject Current;

    public bool Collapsed = false;

    private int _ScaleX;
    private int _ScaleY;
    private int _ScaleZ;

    public GameObject Created;

    [SerializeField] private float _XOffset;
    [SerializeField] private float _YOffset;
    [SerializeField] private float _ZOffset;

    Vector3 chunkScale;

    /// <summary>
    /// This is the ID of the each block
    /// 0 is walls, 1 is floor, 2 is empty, 3 is stairs, 4 is doors (not yet implemented)
    /// </summary>
    public int ID;

    /// <summary>
    /// All the blocks created will be the child of this gameobject
    /// </summary>
    private GameObject _BlockParent;

    /// <summary>
    /// All the tiles created will be the child of this gameobject
    /// </summary>
    private GameObject _TileParent;

    /// <summary>
    /// This will be used to initialize the Block variables
    /// Since this is a mono behaviour, this is also the constructor of the class
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="current"></param>
    /// <param name="scaleX"></param>
    /// <param name="scaleY"></param>
    /// <param name="scaleZ"></param>
    /// <param name="id"></param>
    public void InitializeBlock(int x, int y, int z, GameObject current, int scaleX, int scaleY, int scaleZ, int id)
    {
        _X = x;
        _Y = y;
        _Z = z;
        Current = current;
        _ScaleX = scaleX;
        _ScaleY = scaleY;
        _ScaleZ = scaleZ;
        ID = id;
    }

    public void SetCollapsed(Transform Holder)
    {
        Collapsed = true;
        InstantiatePrefab(Holder);
    }

    public void UnCollapse()
    {
        Collapsed = false;

        if (Created != null)
        {
            Destroy(Created);
        }
    }

    public void Rotate(int rotX, int rotY, int rotZ)
    {
        Created.transform.rotation = Quaternion.Euler(new Vector3(rotX, rotZ, rotY));
    }

    private void InstantiatePrefab(Transform Holder)
    {
        Vector3 _BlockOffset = new Vector3((_X + _XOffset) * _ScaleX, (_Z + _ZOffset) * _ScaleZ, (_Y + _YOffset) * _ScaleY);
        
        switch (ID)
        {
            case 0:
                Created = Instantiate(Current, _BlockOffset, Quaternion.identity, Holder);
                this.chunkScale = Created.transform.localScale;
                Created.transform.localScale = new Vector3(_ScaleX * this.chunkScale.x, _ScaleZ * this.chunkScale.z, _ScaleY * this.chunkScale.y);
                break;
            case 1:
                Created = Instantiate(Current, _BlockOffset, Quaternion.identity, Holder);
                this.chunkScale = Created.transform.localScale;
                Created.transform.localScale = new Vector3(_ScaleX * this.chunkScale.x, _ScaleZ * this.chunkScale.z, _ScaleY * this.chunkScale.y);
                break;
            case 2:
                break;
            case 3:
                Created = Instantiate(Current, _BlockOffset, Quaternion.Euler(new Vector3(-90, 0, 0)), Holder);
                this.chunkScale = Created.transform.localScale;
                Created.transform.localScale = new Vector3(_ScaleX * this.chunkScale.x, _ScaleZ * this.chunkScale.z, _ScaleY * this.chunkScale.y);
                break;
            case 4:
                Created = Instantiate(Current, _BlockOffset, Quaternion.Euler(new Vector3(-90, 0, 0)), Holder);
                this.chunkScale = Created.transform.localScale;
                Created.transform.localScale = new Vector3(_ScaleX * this.chunkScale.x, _ScaleZ * this.chunkScale.z, _ScaleY * this.chunkScale.y);
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
