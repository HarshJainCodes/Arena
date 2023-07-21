using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksV2 : MonoBehaviour
{

    private int _X;
    private int _Y;
    private int _Z;

    public GameObject blockAssigned = null;

    // 0 is floor
    public int ID = -1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateTile(float x, float y, float z, GameObject tileToInstantiate, int scaleX, int scaleY, int scaleZ, int id, Transform parent)
    {
        // not considering offset for now
        blockAssigned = Instantiate(tileToInstantiate, new Vector3(x * scaleX, z * scaleZ, y * scaleY), Quaternion.identity);
        ID = id;
        blockAssigned.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        blockAssigned.transform.parent = parent;
    }

    public void InstantiateWall(float x, float y, float z, GameObject wallToInstantiate, int scaleX, int scaleY, int scaleZ, Transform parent, float rotateBy)
    {
        blockAssigned = Instantiate(wallToInstantiate, new Vector3(x * scaleX, z * scaleZ, y * scaleY), Quaternion.identity);

        blockAssigned.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        blockAssigned.transform.Rotate(new Vector3(0, rotateBy, 0));

        blockAssigned.transform.parent = parent;
    }
}
