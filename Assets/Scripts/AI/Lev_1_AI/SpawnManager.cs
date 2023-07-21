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
    
    [FormerlySerializedAs("_CurrentWave")] public int CurrentWave = 0;
    
    private float _CurrentTime = 300f;
    
    private bool _IsTimerRunning = false;
    
    [FormerlySerializedAs("_generator")] public ChunkCreator Generator;
    
    private Transform _Player;
    
    [FormerlySerializedAs("numberOfEnemiesPerWave")] public int NumberOfEnemiesPerWave = 15;
	public List<Transform> EnemyPrefabs;
	private int _CurrentEnemyIndex = 0;
    public Transform EnemyPrefab;    
    
    [FormerlySerializedAs("spawnPoints")] public List<GameObject> SpawnPoints;
    
    [FormerlySerializedAs("_Enemies")] public List<GameObject> Enemies;
    
    bool _IsGeneratingEnemies = false;

    [FormerlySerializedAs("minEnemyDistance")] public int MinEnemyDistance = 20;

    [FormerlySerializedAs("maxEnemyDistance")] public int MaxEnemyDistance = 30;
    void Start()
    {
        // EnemyPrefabs = GetComponent<List<Transform>>();
        EnemyPrefab = EnemyPrefabs[_CurrentEnemyIndex];
		_CurrentTime = TimeBetweenWaves;
        _IsTimerRunning = true;
        Enemies = new List<GameObject>();
        _Player = GameObject.FindGameObjectWithTag("Player").transform;
        //GenerateNewWave();
        Debug.Log("generate wave called");
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentWave == NumberOfWaves)
            return;
        if(Generator.IsGridGenerated)
        {
	        if (Enemies.Count == 0)
	        {
		        if(_IsTimerRunning)
		        {
			        _CurrentTime -= Time.deltaTime;
			        if(_CurrentTime<=0 && CurrentWave<NumberOfWaves)
			        {
				        _IsTimerRunning = false;

				        if (!_IsGeneratingEnemies)
				        {
					        EnemyPrefab = EnemyPrefabs[_CurrentEnemyIndex];
					        GenerateNewWave();
					        _CurrentEnemyIndex++;
				        }

			        }
                
		        }
	        }
        }
    }


    private void GenerateNewWave()
    {
        _IsGeneratingEnemies = true;
        _CurrentTime = TimeBetweenWaves;
        CurrentWave++;
        SpawnPoints = new List<GameObject>();
        SpawnPoints = Generator.GetSpawnPoints(_Player, NumberOfEnemiesPerWave, MinEnemyDistance,MaxEnemyDistance);

        foreach (GameObject t in SpawnPoints)
        {
            Transform enemy = Instantiate(EnemyPrefab, t.transform.position, Quaternion.identity);
            enemy.gameObject.GetComponentInChildren<AIDestinationSetter>().target = _Player;
           Enemies.Add(enemy.gameObject);
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
