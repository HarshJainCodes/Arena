using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A master script that passes values between the VoxelRunTimeManipulation Script
/// </summary>
public class ControlScript : MonoBehaviour
{
    /// <summary>
    /// Handle to the actual manipulation script 
    /// </summary>
    [SerializeField] private VoxelRunTimeManipulation _VoxelRunTimeManip;
    /// <summary>
    /// Stores the currently active volume
    /// </summary>
    [SerializeField] private int _ActiveVolume = 0;

    /// <summary>
    /// Getter and setter method to set the current active volume to the desired one.
    /// </summary>
    public int ActiveVolume { set { _ActiveVolume = value; _VoxelRunTimeManip.SelectedVolume = value; } get { return _ActiveVolume; } }

    /// <summary>
    /// Sets the color it obtains from the buttons on the various palettes.
    /// </summary>
    /// <param name="color"></param>
    public void ColourSet(Color color)
    {
        _VoxelRunTimeManip.SetColor(color);
    }
}
