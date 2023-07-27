using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputScript : MonoBehaviour
{
    [SerializeField] private CamPerspectiveScript _CamManager;
    [SerializeField] private VoxelRunTimeManipulation _ToolManipulation;

    private void Update()
    {
        InputScript();
    }

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
