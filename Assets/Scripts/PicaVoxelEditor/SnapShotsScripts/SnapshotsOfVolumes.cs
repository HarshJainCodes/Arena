using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component allows us to grab the snapshots of the voxel volumes during runtime
/// </summary>
public class SnapshotsOfVolumes : MonoBehaviour
{

    /// <summary>
    /// Handle to a scene camera that is fixed and grabs pics of the changes made to block
    /// </summary>
    [SerializeField] private Camera _Cam;
    /// <summary>
    /// Handle to the images on the UI of the scene
    /// </summary>
    [SerializeField] private RawImage[] _Snaps;
    /// <summary>
    /// Array of rendertextures so that we can have different snaps of different blocks 
    /// </summary>
    [SerializeField] private RenderTexture[] _Render;


    private void Start()
    {
        _Render[0] = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        _Cam.targetTexture = _Render[0];
        _Snaps[0].texture = _Render[0];
    }
    /// <summary>
    /// Grabs a snapshot of the current volume and places it in the image 
    /// </summary>
    /// <param name="tempIndex"></param>
    public void grabsnap(int tempIndex)
    {
        _Render[tempIndex] = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        _Cam.targetTexture = _Render[tempIndex];
        _Snaps[tempIndex].texture = _Render[tempIndex];
    }

}
