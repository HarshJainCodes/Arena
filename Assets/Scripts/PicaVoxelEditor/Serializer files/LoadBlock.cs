using PicaVoxel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBlock : MonoBehaviour
{
    private DataSerializer _DataSerializer = new DataSerializer();
    private VolumeStorage[] _VolumeStorages = new VolumeStorage[10];
    [SerializeField]private Volume[] _Volumes;
    [SerializeField] private int set = 0;
    // Start is called before the first frame update


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
    public void LoadData()
    {
        for(int i=0;i<_Volumes.Length;i++)
        {
            string _currentSet = $"/{set}block{i}.json";
            _VolumeStorages[i] = _DataSerializer.LoadData<VolumeStorage>(_currentSet);
        }
        _setVoxelBlock();
    }
}
