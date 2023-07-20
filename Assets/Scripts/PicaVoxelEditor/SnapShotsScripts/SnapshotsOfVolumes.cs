using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapshotsOfVolumes : MonoBehaviour
{

    [SerializeField] private Camera _Cam;
    [SerializeField] private RawImage[] _Snaps;
    [SerializeField] private RenderTexture[] _Render;


    public void grabsnap(int tempIndex)
    {
        _Render[tempIndex] = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        _Cam.targetTexture = _Render[tempIndex];
        _Snaps[tempIndex].texture = _Render[tempIndex];
        /*//RenderTexture render= _Cam.targetTexture;
        Texture2D camTexture = new Texture2D(render.width, render.height, TextureFormat.ARGB32,false);
        RenderTexture.active = render;
        _Material.mainTexture = render;
        _Snaps[_Index].material =_Material;*/
    }
    // Start is called before the first frame update

}
