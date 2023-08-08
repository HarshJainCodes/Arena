using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Originally used to test the inputs without using a UI but now kept as shotcut keys to edit blocks
/// </summary>
public class TestInputScript : MonoBehaviour
{
    /// <summary>
    /// Handle to CamToPerspectiveScript
    /// </summary>
    [SerializeField] private CamPerspectiveScript _CamManager;
    /// <summary>
    /// Handle to VoxelRunTimeManipulation Script
    /// </summary>
    [SerializeField] private VoxelRunTimeManipulation _ToolManipulation;

    private void Update()
    {
        InputScript();
    }

    /// <summary>
    /// Originally used to test the inputs without using a UI but now kept as shotcut keys to edit blocks
    /// </summary>
    private void InputScript()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            _ToolManipulation.SetTypeOfOperation(VoxelRunTimeManipulation.OperationType.none);
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            _ToolManipulation.SetTypeOfOperation(VoxelRunTimeManipulation.OperationType.add);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _ToolManipulation.SetTypeOfOperation(VoxelRunTimeManipulation.OperationType.remove);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            _ToolManipulation.SetTypeOfOperation(VoxelRunTimeManipulation.OperationType.edit);
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            _ToolManipulation.SetTypeOfOperation(VoxelRunTimeManipulation.OperationType.bucket);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            _CamManager.SwapToOrthographic();
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            _CamManager.SwapToPerspective();
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            _ToolManipulation.MirrorChange = !_ToolManipulation.MirrorChange;
        }
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L))
        {
            _ToolManipulation.RevertChange();
        }
    }
}
