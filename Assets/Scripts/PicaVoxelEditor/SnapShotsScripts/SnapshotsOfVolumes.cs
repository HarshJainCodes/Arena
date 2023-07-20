using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapshotsOfVolumes : MonoBehaviour
{

    [SerializeField] private Camera _Cam;
    [SerializeField] private RawImage[] _Snaps;
    [SerializeField] private RenderTexture[] _Render;


    private void Start()
    {
        _Render[0] = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        _Cam.targetTexture = _Render[0];
        _Snaps[0].texture = _Render[0];
    }
    public void grabsnap(int tempIndex)
    {
        _Render[tempIndex] = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        _Cam.targetTexture = _Render[tempIndex];
        _Snaps[tempIndex].texture = _Render[tempIndex];
    }

}
