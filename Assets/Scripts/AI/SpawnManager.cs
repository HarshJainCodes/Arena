using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Serialization;

public class SpawnManager : MonoBehaviour
{//harsh
    [FormerlySerializedAs("numberOfWaves")] public int NumberOfWaves = 3;

    [FormerlySerializedAs("timeBetweenWaves")] [Tooltip("In Seconds")]
    public float TimeBetweenWaves = 10;
    
    private int _CurrentWave = 0;
    
    private float _CurrentTime = 300f;
    
    private bool _IsTimerRunning = false;
    
    [FormerlySerializedAs("_generator")] public ChunkCreator Generator;
    
    private Transform _Player;
    
    [FormerlySerializedAs("numberOfEnemiesPerWave")] public int NumberOfEnemiesPerWave = 15;

    [FormerlySerializedAs("enemyPrefab")] public Transform EnemyPrefab; 
    
    [FormerlySerializedAs("spawnPoints")] public List<GameObject> SpawnPoints;
    
    private List<GameObject> _Enemies;
    
    bool _IsGeneratingEnemies = false;

    [FormerlySerializedAs("minEnemyDistance")] public int MinEnemyDistance = 20;

    [FormerlySerializedAs("maxEnemyDistance")] public int MaxEnemyDistance = 30;
    void Start()
    {
        _CurrentTime = TimeBetweenWaves;
        _IsTimerRunning = true;
        _Enemies = new List<GameObject>();
        _Player = GameObject.FindGameObjectWithTag("Player").transform;
        //GenerateNewWave();
        Debug.Log("generate wave called");
    }

    // Update is called once per frame
    void Update()
    {
        if (_CurrentWave == NumberOfWaves)
            return;
        if(Generator.IsGridGenerated)
        {
            if(_IsTimerRunning)
            {
                _CurrentTime -= Time.deltaTime;
                if(_CurrentTime<=0 && _CurrentWave<NumberOfWaves)
                {
                    _IsTimerRunning = false;

                    if (!_IsGeneratingEnemies)
                    {
                        GenerateNewWave();

                    }

                }
                
            }
        }
    }


    private void GenerateNewWave()
    {
        _IsGeneratingEnemies = true;
        _CurrentTime = TimeBetweenWaves;
        _CurrentWave++;
        SpawnPoints = new List<GameObject>();
        SpawnPoints = Generator.GetSpawnPoints(_Player, NumberOfEnemiesPerWave, MinEnemyDistance,MaxEnemyDistance);

        foreach (GameObject t in SpawnPoints)
        {
            Transform enemy = Instantiate(EnemyPrefab, t.transform.position, Quaternion.identity);
            enemy.gameObject.GetComponentInChildren<AIDestinationSetter>().target = _Player;
           _Enemies.Add(enemy.gameObject);
        }

        _IsGeneratingEnemies = false;
        _IsTimerRunning = true;
    }

    private void OnDrawGizmos()
    {

        foreach (GameObject t in SpawnPoints)
        {
            //Transform enemy = Instantiate(enemyPrefab, t.transform.position, Quaternion.identity);
            //enemy.gameObject.GetComponent<AIDestinationSetter>().target = player;
            //enemies.Add(enemy.gameObject);
            Gizmos.color = Color.white;
            Gizmos.DrawCube(t.transform.position,Vector3.one);
        }
    }
}
