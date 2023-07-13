using PicaVoxel;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VoxelRunTimeManipulation : MonoBehaviour
{
    /// <summary>
    /// The enum deternmines the type of operation the RunTimeVoxelManipulation Script is set to.
    /// </summary>
    public enum OperationType
    {
        none,
        add,
        remove,
        edit,
        bucket
    }
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
    private int _SelectedVolume = 0;
    /// <summary>
    /// This colour will be used to select a colour from the editor and use the same
    /// </summary>
    [SerializeField]
    private Color _selectedColour;

    /// <summary>
    /// This enumerator determines which operation is to be executed and has getter method 
    /// <see cref="GetTypeOfOperation"/>
    /// and setter method
    /// <see cref="SetTypeOfOperation(OperationType)"/>
    /// </summary>
    [SerializeField]
    private OperationType _typeOfOperation = OperationType.none;
    

    void Update()
    {
        _VoxelManipulation();
    }

    /// <summary>
    /// This function holds code for manipulating individual voxels 
    /// </summary>
    private void _VoxelManipulation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // bool found is used to stop the loop when we actually find a voxel;
            bool found = false;

            Ray screenRay=Camera.main.ScreenPointToRay(Input.mousePosition);

            // Casting a ray to detect whether we hit a voxel or not with the cam centre
            Ray r = new Ray(_CamTransform.position, _CamTransform.forward);

            // Debugging and checking the ray if it is working properly
            Debug.DrawRay(r.origin, r.direction * 100f, Color.red, 10f);

            // The for loop runs to find the exact spot at which the voxel is found
            for (float d = 0; d < 50f; d += 0.05f)
            {
                // This is a temporary variable that stores the volume that is currently being edited
                Volume currentVoxelVolume = _VoxelVolumes[_SelectedVolume];

                // The Voxel v stores the position of the current voxel that is selected by the ray
                Voxel? v = currentVoxelVolume.GetVoxelAtWorldPosition(screenRay.GetPoint(d));

                //checks if that voxel is active
                if (v.HasValue && v.Value.Active)
                { 
                    switch (_typeOfOperation)
                    {
                        case OperationType.none:

                            // This state exists in case that the player wants to move the blocks and so on.
                            break;

                        case OperationType.add:

                            //gets the position of the voxel to be built
                            Vector3 buildPos = screenRay.GetPoint(d - 0.05f);

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
                            break;

                        case OperationType.remove:
                            // We simply set the satus of that particular voxel as inactive thus it does not render
                            currentVoxelVolume.SetVoxelAtWorldPosition(screenRay.GetPoint(d), new Voxel()
                            {
                                State = VoxelState.Inactive,
                                Color = _selectedColour,
                                Value = 128
                            });
                            break;

                        case OperationType.edit:

                            // We simply change the colour of the voxel here.
                            currentVoxelVolume.SetVoxelAtWorldPosition(screenRay.GetPoint(d), new Voxel()
                            {
                                State = VoxelState.Active,
                                Color = _selectedColour,
                                Value = 128
                            });
                            break;

                        case OperationType.bucket:
                            for (int i = 0; i < currentVoxelVolume.XSize; i++)
                                for (int j = 0; j < currentVoxelVolume.YSize; j++)
                                    for (int k = 0; k < currentVoxelVolume.ZSize; k++)
                                    {
                                        Voxel? a = currentVoxelVolume.GetVoxelAtArrayPosition(i, j, k);
                                        if (a.Value.Active)
                                        {
                                            currentVoxelVolume.SetVoxelAtArrayPosition(new PicaVoxelPoint(new Vector3(i,j,k)), new Voxel()
                                            {
                                                State = VoxelState.Active,
                                                Color = _selectedColour,
                                                Value = 128
                                            });
                                        }
                                        
                                    }
                            break;
                    }
                    found = true;
                }
                if (found) break;
            }
        }
    }

    /// <summary>
    /// This function returns the current operation that is being performed.
    /// </summary>
    /// <returns>OperationType enum</returns>
    public OperationType GetTypeOfOperation() { return _typeOfOperation; }
    /// <summary>
    /// This function sets the opertaion that is to be performed on a voxel.
    /// </summary>
    /// <param name="op"></param>
    public void SetTypeOfOperation(OperationType op) { _typeOfOperation = op; }

    /// <summary>
    /// This function returns the currently chosen color 
    /// </summary>
    /// <returns>return type is Color</returns>
    public Color GetColor() { return _selectedColour; }

    /// <summary>
    /// This function sets the user requested color to be put on the blocks
    /// </summary>
    /// <param name="choice"></param>
    public void SetColor(Color choice) { _selectedColour = choice; }
}
