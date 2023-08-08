using PicaVoxel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that tells the play scene whether to use the created blocks or the default blocks
/// </summary>
public class GetBlocks : MonoBehaviour
{
    /// <summary>
    /// Singleton instance is created here and this scripts checks if the blocks were loaded to run ingame.
    /// </summary>
    private static GetBlocks _instance;
    /// <summary>
    /// returns the private instance to call functions
    /// </summary>
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
    // currently the script is only using a single bool export and yes i can just use player prefs instead but originally the scrip had more purpose.
}
