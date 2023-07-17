using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScript : MonoBehaviour
{
    [SerializeField] private VoxelRunTimeManipulation _VoxelRunTimeManip;
    [SerializeField] private int _ActiveVolume = 0;

    public int ActiveVolume { set { _ActiveVolume = value; _VoxelRunTimeManip.SelectedVolume = value; } get { return _ActiveVolume; } }

    public void ColourSet(Color color)
    {
        _VoxelRunTimeManip.SetColor(color);
    }
}
