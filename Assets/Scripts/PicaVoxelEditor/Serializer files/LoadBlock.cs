using PicaVoxel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class tailors to loading the block data from JSON file onto a volume of a prefab of the same size
/// </summary>
public class LoadBlock : MonoBehaviour
{
    private DataSerializer _DataSerializer = new DataSerializer();
    private VolumeStorage[] _VolumeStorages = new VolumeStorage[10];
    [SerializeField]private Volume[] _Volumes;
    [SerializeField] private int set = 0;
    [SerializeField] private VoxelRunTimeManipulation _manipScript;
    // Start is called before the first frame update


    private void Awake()
    {

    }

    /// <summary>
    /// It converts the JSON data. The JSON holds value of each individual voxel which is then set on a standard prefab voxel volume
    /// </summary>
    private void _setVoxelBlock()
    {
        for(int i=0;i<_Volumes.Length;i++)
        {
            _Volumes[i].XSize = _VolumeStorages[i].sizex;
            _Volumes[i].YSize = _VolumeStorages[i].sizey;
            _Volumes[i].ZSize = _VolumeStorages[i].sizez;
            /*_Volumes[i].Pivot = new Vector3(_Volumes[i].XSize/2, _Volumes[i].YSize / 2, _Volumes[i].ZSize / 2);
            _Volumes[i].UpdatePivot();*/
            _Volumes[i].UpdateAllChunks();
            for (int x = 0; x < _Volumes[i].XSize; x++)
            {
                for (int y = 0; y <_Volumes[i].YSize; y++)
                {
                    for (int z = 0; z < _Volumes[i].ZSize; z++)
                    {
                        PicaVoxelPoint p = new PicaVoxelPoint(x,y,z);
                        Color current = new Color(_VolumeStorages[i].store[x, y, z].r, _VolumeStorages[i].store[x, y, z].g, _VolumeStorages[i].store[x, y, z].b, _VolumeStorages[i].store[x, y, z].a);
                        _Volumes[i].SetVoxelAtArrayPosition(p, new Voxel()
                        {
                            State = (VoxelState)_VolumeStorages[i].store[x,y,z].currentstate,
                            Color = current,
                            Value = _VolumeStorages[i].store[x, y, z].val
                        });
                    }
                }
            }
        }
    }

    /// <summary>
    /// This function grabs the actual JSON file and stores it into the <see cref="VolumeStorage"/> class which is tailor made for storing serializable data regarding 
    /// <see cref="PicaVoxel.Volume"/>. It also calls the <see cref="_setVoxelBlock"/> and the <see cref="SetExport"/> functions.
    /// </summary>
    public void LoadData()
    {
        getVolumes();
        for(int i=0;i<_Volumes.Length;i++)
        {
            string _currentSet = $"/{set}block{i}.json";
            _VolumeStorages[i] = _DataSerializer.LoadData<VolumeStorage>(_currentSet);
        }
        _setVoxelBlock();
        SetExport();
    }
    public void getVolumes()
    {
        for (int i = 0; i < _Volumes.Length; i++)
        {
            _Volumes[i] = _manipScript.getVolume(i);
        }
    }

    /// <summary>
    /// This sets the export value in the <see cref="GetBlocks"/> singleton object as true so that in play mode the custom blocks are loaded in.
    /// </summary>
    public void SetExport()
    {
        GetBlocks.blocks= _Volumes;
        GetBlocks.Instance.export = true;
    }
}
