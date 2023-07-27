using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is the rewrite of the Blocks class that the original ChunkGenerator used.
/// </summary>
public class BlocksV2 : MonoBehaviour
{
    /// <summary>
    /// The x coordinate in the world space where the block needs to be instantiated
    /// </summary>
    private int _X;
    /// <summary>
    /// The y coordinate in the world space where the block needs to be instantiated
    /// </summary>
    private int _Y;
    /// <summary>
    /// The z coordinate in the world space where the block needs to be instantiated
    /// </summary>
    private int _Z;

    /// <summary>
    /// blockAssigned will contain the instantiated block that will be spawned in the world
    /// <para> This could be floor, wall or stairs </para>
    /// </summary>
    public GameObject blockAssigned = null;

    /// <summary>
    /// id of -1 means that there is no block assigned currently
    /// <para> Remember to change it back to -1 if you are destroying the assigned block for example to make the hole for stairs </para>
    /// </summary>
    public int ID = -1;

    /// <summary>
    /// This will instantiate a floor tile on the x, y and z, and will Instantiate the block and assign that tot the <see cref="blockAssigned"/> variable
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="tileToInstantiate"></param>
    /// <param name="scaleX"></param>
    /// <param name="scaleY"></param>
    /// <param name="scaleZ"></param>
    /// <param name="id"></param>
    /// <param name="parent"></param>
    public void InstantiateTile(float x, float y, float z, GameObject tileToInstantiate, int scaleX, int scaleY, int scaleZ, int id, Transform parent)
    {
        // not considering offset for now
        blockAssigned = Instantiate(tileToInstantiate, new Vector3(x * scaleX, z * scaleZ, y * scaleY), Quaternion.identity);
        ID = id;
        blockAssigned.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        blockAssigned.transform.parent = parent;
    }

    /// <summary>
    /// This will instantiate a wall tile on the x, y and z and rotate the wall block by the <paramref name="rotateBy"/> amount as the block needs to be properly oriented, and will Instantiate the block and assign that tot the <see cref="blockAssigned"/> variable
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="wallToInstantiate"></param>
    /// <param name="scaleX"></param>
    /// <param name="scaleY"></param>
    /// <param name="scaleZ"></param>
    /// <param name="parent"></param>
    /// <param name="rotateBy"></param>
    public void InstantiateWall(float x, float y, float z, GameObject wallToInstantiate, int scaleX, int scaleY, int scaleZ, Transform parent, float rotateBy)
    {
        blockAssigned = Instantiate(wallToInstantiate, new Vector3(x * scaleX, z * scaleZ, y * scaleY), Quaternion.identity);

        blockAssigned.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        blockAssigned.transform.Rotate(new Vector3(0, rotateBy, 0));

        blockAssigned.transform.parent = parent;
    }

    /// <summary>
    /// This will instantiate the stairs on the x, y and z will assign the instantiated gameobject to the <see cref="blockAssigned"/> variable
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="stairsToInstantiate"></param>
    /// <param name="scaleX"></param>
    /// <param name="scaleY"></param>
    /// <param name="scaleZ"></param>
    /// <param name="parent"></param>
    public void InstantiateStair(float x, float y, float z, GameObject stairsToInstantiate, int scaleX, int scaleY, int scaleZ, Transform parent)
    {
        blockAssigned = Instantiate(stairsToInstantiate, new Vector3(x * scaleX, z * scaleZ, y * scaleY), Quaternion.identity);

        blockAssigned.transform.localScale = new Vector3(scaleX/2, scaleY/2, scaleZ/2);
        blockAssigned.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        blockAssigned.transform.parent = parent;
    }
}
