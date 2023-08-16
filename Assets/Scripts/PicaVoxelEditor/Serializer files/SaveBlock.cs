using PicaVoxel;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/// <summary>
/// This class caters to storing the data of a Voxel Volume. To be specific it converts the data of Voxel Volume into serializable format by saving each voxel in primitive datatypes
/// </summary>
public class SaveBlock : MonoBehaviour
{
    /// <summary>
    /// Stores the volumes in the scene
    /// </summary>
    [SerializeField] private Volume[] _Volumes;
    /// <summary>
    /// Custom class array to store the individual voxel data.
    /// </summary>
    private VolumeStorage[] _VolumeStorages=new VolumeStorage[10];
    /// <summary>
    /// Creates an instance of the dataserializier scipt so that we can serialize the data 
    /// </summary>
    private DataSerializer _DataSerializer=new DataSerializer();
    /// <summary>
    /// Set determines what set we are currently serializing it as we can serialize nigh infinite amount of sets.
    /// </summary>
    [SerializeField] private int set = 0;
    /// <summary>
    /// Handle to <see cref="VoxelRunTimeManipulation"/>
    /// </summary>
    [SerializeField] private VoxelRunTimeManipulation _manipScript;

    private void Awake()
    {
        for(int i=0;i<_Volumes.Length;i++)
        {
            _VolumeStorages[i] = new VolumeStorage(_Volumes[i].XSize, _Volumes[i].YSize,_Volumes[i].ZSize); 
        }
    }

    private void _StoreInSerializable()
    {
        for(int i=0; i < _Volumes.Length; i++)
        {
            _accessindividualvoxel(_Volumes[i], i);
        }
    }
    // Start is called before the first frame update
    /// <summary>
    /// This funciton accesses individual volumes and stores them in <see cref="VolumeStorage"/>
    /// </summary>
    /// <param name="currentVolume"></param>
    /// <param name="index"></param>
    private void _accessindividualvoxel(Volume currentVolume,int index)
    {
        for (int x = 0; x < currentVolume.XSize; x++)
        {
            for (int y = 0; y < currentVolume.YSize; y++)
            {
                for (int z = 0; z < currentVolume.ZSize; z++)
                {
                    Voxel? temp = currentVolume.GetVoxelAtArrayPosition(x, y, z);
                    Color c = temp.Value.Color;
                    _VolumeStorages[index].store[x, y, z] = new VolumeStorage.voxel(c.r, c.g, c.b, c.a, (int)temp.Value.State, temp.Value.Value);
                }
            }
        }
    }
    /// <summary>
    /// This function sets thte 
    /// </summary>
    public void SerializeData()
    {
        AudioManager.instance?.PlayOneShot(FMODEvents.instance.UI_Confirmation, this.transform.position);
        getVolumes();
        _StoreInSerializable();
        for(int i=0;i< _Volumes.Length;i++)
        {
            string _currentSet = $"/{set}block{i}.json";
            _DataSerializer.SaveData(_currentSet, _VolumeStorages[i]);
        }
    }
    /// <summary>
    /// This function references the volumes that are present in the edit mode to store the volume info
    /// </summary>
    public void getVolumes()
    {
        for (int i = 0; i < _Volumes.Length; i++)
        {
            _Volumes[i] = _manipScript.getVolume(i);
        }
    }
}
/// <summary>
/// The below given class contains the primitive datatype data version of a voxel volume
/// </summary>
[System.Serializable]
public class VolumeStorage
{
    public class voxel
    {
        public float r, g, b, a;
        public enum state
        {
            Inactive = 0,
            Active = 1,
            Hidden = 2
        }

        public state currentstate = state.Inactive;

        public byte val;

        public voxel(float r, float g, float b, float a, int state, byte val)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
            currentstate = (state)state;
            this.val = val;
        }
    }
    public int sizex, sizey, sizez;
    public voxel[,,] store;

    public VolumeStorage(int x, int y, int z)
    {
        sizex = x;
        sizey = y;
        sizez = z;
        store = new voxel[x, y, z];
    }

    
}

