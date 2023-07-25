using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{

    List<List<GameObject>> Grid = new List<List<GameObject>>();

    [SerializeField] public int GRID_SIZE = 10;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            List<GameObject> l = new List<GameObject>();
            for (int j = 0; j < GRID_SIZE; j++)
            {
                l.Add(new GameObject(i.ToString() + "__" + j.ToString()));
            }
            Grid.Add(l);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
