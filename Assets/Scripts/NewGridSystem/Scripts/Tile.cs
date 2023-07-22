using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    GameObject SelfTile;

    List<string> edges = new List<string>(4);

    public List<int> UpOptions = new List<int>();
    public List<int> DownOptions = new List<int>();
    public List<int> LeftOptions = new List<int>();
    public List<int> RightOptions = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
