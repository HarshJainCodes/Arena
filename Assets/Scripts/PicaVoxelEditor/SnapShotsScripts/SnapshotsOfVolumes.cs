using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapshotsOfVolumes : MonoBehaviour
{

    [SerializeField] private Camera _Cam;
    [SerializeField] private int _Index=0;
    [SerializeField] private RawImage[] _Snaps;
    private RenderTexture render;
    [SerializeField] private Material _Material;

    public int index{ get { return _Index; } set { _Index=value; } }

    public void grabsnap()
    {
        render = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        _Cam.targetTexture = render;
        //RenderTexture render= _Cam.targetTexture;
        Texture2D camTexture = new Texture2D(render.width, render.height, TextureFormat.ARGB32,false);
        RenderTexture.active = render;
        _Material.mainTexture = render;
        _Snaps[_Index].material =_Material;
    }
    // Start is called before the first frame update

}
