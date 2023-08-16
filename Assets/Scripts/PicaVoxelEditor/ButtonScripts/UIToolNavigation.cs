using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class handles the UI of various possible edit options that the voxel volume editor provides
/// </summary>
[RequireComponent(typeof(Button))]
public class UIToolNavigation : MonoBehaviour
{
    /// <summary>
    /// Handle to the main <see cref="VoxelRunTimeManipulation"/> script that controls all the various operations that are performed on the voxel volume
    /// </summary>
    [SerializeField] VoxelRunTimeManipulation _ManipScript;

    /// <summary>
    /// Adds blocks to the voxel volume
    /// </summary>
    public void AddBlock()
    {
        AudioManager.instance?.PlayOneShot(FMODEvents.instance.Select, this.transform.position);
        _ManipScript.SetTypeOfOperation(VoxelRunTimeManipulation.OperationType.add);
    }

    /// <summary>
    /// Removes Blocks from the Voxel volume
    /// </summary>
    public void RemoveBlock()
    {
        AudioManager.instance?.PlayOneShot(FMODEvents.instance.Select, this.transform.position);
        _ManipScript.SetTypeOfOperation(VoxelRunTimeManipulation.OperationType.remove);
    }

    /// <summary>
    /// Enables and disables the mirror operation option.
    /// </summary>
    public void MirrorBlock()
    {
        AudioManager.instance?.PlayOneShot(FMODEvents.instance.Select, this.transform.position);
        _ManipScript.MirrorChange = !_ManipScript.MirrorChange;
    }

    /// <summary>
    /// Allows the user to edit the color of the block.
    /// </summary>
    public void ColourBlock()
    {
        AudioManager.instance?.PlayOneShot(FMODEvents.instance.Select, this.transform.position);
        _ManipScript.SetTypeOfOperation(VoxelRunTimeManipulation.OperationType.edit);
    }

    /// <summary>
    /// Enables the bucket fill tool
    /// </summary>
    public void BucketFill()
    {
        AudioManager.instance?.PlayOneShot(FMODEvents.instance.Select, this.transform.position);
        _ManipScript.SetTypeOfOperation(VoxelRunTimeManipulation.OperationType.bucket);
    }
}
