using PicaVoxel;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GetBlocks : MonoBehaviour
{
    private GetBlocks _instance=new GetBlocks();
    public GetBlocks Instance { get { return _instance; } }

    public Volume[] blocks=new Volume[7];

    public bool export = false;

    private void Awake()
    {
        if(!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
           Destroy(gameObject);
        }
    }
    // Start is called before the first frame update

    // Update is called once per frame
}
