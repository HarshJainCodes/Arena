using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIToolNavigation : MonoBehaviour
{
    [SerializeField] VoxelRunTimeManipulation _ManipScript;

    public void AddBlock()
    {
        _ManipScript.SetTypeOfOperation(VoxelRunTimeManipulation.OperationType.add);
    }

    public void RemoveBlock()
    {
        _ManipScript.SetTypeOfOperation(VoxelRunTimeManipulation.OperationType.remove);
    }

    public void MirrorBlock()
    {
        _ManipScript.MirrorChange = !_ManipScript.MirrorChange;
    }

    public void ColourBlock()
    {
        _ManipScript.SetTypeOfOperation(VoxelRunTimeManipulation.OperationType.edit);
    }

    public void BucketFill()
    {
        _ManipScript.SetTypeOfOperation(VoxelRunTimeManipulation.OperationType.bucket);
    }

    // Start is called before the first frame update


    // Update is called once per frame

}
