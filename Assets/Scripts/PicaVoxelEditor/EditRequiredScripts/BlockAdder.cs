using PicaVoxel;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BlockAdder : MonoBehaviour
{
    /// <summary>
    /// The bool checks whether the script to add is enables or disabled
    /// </summary>
    [SerializeField]
    private bool _Add = false;
    /// <summary>
    /// The main camera transform is necessary to fire a ray from the centre
    /// </summary>
    [SerializeField]
    private Transform _CamTransform;
    /// <summary>
    /// The layer mask is necessary to target only the voxel
    /// </summary>
    [SerializeField]
    private LayerMask _VoxelLayer;
    /// <summary>
    /// This array stores the volumes of the voxels that are to be modified
    /// </summary>
    [SerializeField]
    private Volume[] _VoxelVolumes;
    /// <summary>
    /// This variable points to the currently selected volume
    /// </summary>
    [SerializeField]
    private int _SelectedVolume=0;
    /// <summary>
    /// This colour will be used to select a colour from the editor and use the same
    /// </summary>
    [SerializeField]
    private Color _selectedColour;

    void Update()
    {
        _VoxelAdder();
    }

    private void _VoxelAdder()
    {
        if(Input.GetMouseButtonDown(0) && _Add)
        {
            bool found = false;
            // Casting a ray to detect whether we hit a voxel or not with the cam centre
            Ray r = new Ray(_CamTransform.position,_CamTransform.forward);
            // Debugging and checking the ray if it is working properly
            Debug.DrawRay(r.origin, r.direction * 100f, Color.red, 10f);
            // The for loop runs to find the exact spot at which the voxel is found
            for (float d = 0; d < 50f; d += 0.05f)
            {
                // This is a temporary variable that stores the volume that is currently being edited
                Volume currentVoxelVolume = _VoxelVolumes[_SelectedVolume];
                // The Voxel v stores the position of the current voxel that is selected by the ray
                Voxel? v = currentVoxelVolume.GetVoxelAtWorldPosition(r.GetPoint(d));
                //checks if that voxel is active
                if(v.HasValue && v.Value.Active)
                {
                    //gets the position of the voxel to be built
                    Vector3 buildPos = r.GetPoint(d-0.05f);
                    // The Voxel v2 stores the location of the voxel to be created and is made to check whether the voxel here is active or not 
                    Voxel? v2 = currentVoxelVolume.GetVoxelAtWorldPosition(buildPos);
                    if (v2.HasValue && !v2.Value.Active)
                    {
                        // Here we actually set the values of the voxel at that position
                        currentVoxelVolume.SetVoxelAtWorldPosition(buildPos, new Voxel()
                        {
                            State = VoxelState.Active,
                            Color = _selectedColour,
                            Value = 128
                        });
                    }
                    found = true;
                }
                if (found) break;
            }
        }
    }
    /// <summary>
    /// Gets the currently selected Pico Voxel Volume
    /// </summary>
    /// <returns>It returns an int value</returns>
    public int GetSelectedVolume(){    return _SelectedVolume; }
    /// <summary>
    /// It accepts an integer value as an array pointer to select the current volume
    /// </summary>
    /// <param name="pointer"></param>
    public void SetSelectedVolume(int pointer) { _SelectedVolume = pointer;  }
    /// <summary>
    /// It returns the colour that is presently selected and return the same
    /// </summary>
    /// <returns> Color type variable is returned </returns>
    public Color GetSelectedColour() { return _selectedColour; }
    /// <summary>
    /// It sets the current colour of the block that is to be added
    /// </summary>
    /// <param name="color"></param>
    public void SetSelectedColour(Color color) {  _selectedColour = color; }

}
