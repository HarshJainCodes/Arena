using PicaVoxel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBlocks : MonoBehaviour
{
    private static GetBlocks _instance;
    public  static GetBlocks Instance { get { return _instance; } private set { _instance = value; } }

    [SerializeField]
    public static Volume[] blocks=new Volume[7];

    public bool export = false;

    private void Awake()
    {
        if(Instance!=null && Instance!=this)
        {
            Destroy(this);
        }
        else
        {
           Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    // Start is called before the first frame update

    // Update is called once per frame
}
