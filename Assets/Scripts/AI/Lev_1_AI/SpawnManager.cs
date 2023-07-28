using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Serialization;

public class SpawnManager : MonoBehaviour
{
	// Number of waves to be spawned
	[FormerlySerializedAs("numberOfWaves")] public int NumberOfWaves = 3;

	// Time between waves in seconds
	[FormerlySerializedAs("timeBetweenWaves")]
	[Tooltip("In Seconds")]
	public float TimeBetweenWaves = 10;

	// Current wave number
	[FormerlySerializedAs("_CurrentWave")] public int CurrentWave = 0;

	// Current time for wave countdown
	[FormerlySerializedAs("_CurrentTime")] public float CurrentTime = 300f;

	// Flag to check if the timer is running
	private bool _IsTimerRunning = false;

	// Reference to the ChunkScriptV2 or ChunkCreator (for generating the level)
	public ChunkScriptV2 Generator;

	// Reference to the player's transform
	private Transform _Player;

	// Number of enemies to be spawned per wave
	[FormerlySerializedAs("numberOfEnemiesPerWave")] public int NumberOfEnemiesPerWave = 15;

	// List of enemy prefabs to be spawned
	public List<Transform> EnemyPrefabs;

	// Index of the current enemy prefab to be spawned
	private int _CurrentEnemyIndex = 0;

	// Reference to the current enemy prefab to be spawned
	public Transform EnemyPrefab;

	// List of spawn points for enemies
	[FormerlySerializedAs("spawnPoints")] public List<GameObject> SpawnPoints;

	// List of active enemy game objects
	[FormerlySerializedAs("_Enemies")] public List<GameObject> Enemies;

	// Flag to check if enemies are being generated
	bool _IsGeneratingEnemies = false;

	// Minimum distance for enemy spawn from the player
	[FormerlySerializedAs("minEnemyDistance")] public int MinEnemyDistance = 10;

	// Maximum distance for enemy spawn from the player
	[FormerlySerializedAs("maxEnemyDistance")] public int MaxEnemyDistance = 60;

	void Start()
	{
		// Set initial values
		EnemyPrefab = EnemyPrefabs[_CurrentEnemyIndex];
		CurrentTime = TimeBetweenWaves;
		_IsTimerRunning = true;
		Enemies = new List<GameObject>();
		_Player = GameObject.FindGameObjectWithTag("Player").transform;
		//GenerateNewWave();
	}

	void Update()
	{
		// Check if all waves are completed
		if (CurrentWave == NumberOfWaves)
			return;

		// Check if the grid is generated
		if (Generator.IsGridGenerated)
		{
			// Check if there are no enemies and the timer is running
			if (Enemies.Count == 0 && _IsTimerRunning)
			{
				// Decrease the wave countdown timer
				CurrentTime -= Time.deltaTime;

				// If the countdown timer reaches zero and there are still waves left
				if (CurrentTime <= 0 && CurrentWave < NumberOfWaves)
				{
					_IsTimerRunning = false;

					// Check if enemies are not currently being generated
					if (!_IsGeneratingEnemies)
					{
						// Get the current enemy prefab to spawn
						EnemyPrefab = EnemyPrefabs[_CurrentEnemyIndex];

						// Generate a new wave of enemies
						GenerateNewWave();

						// Move to the next enemy prefab for the next wave
						_CurrentEnemyIndex++;
					}
				}
			}
		}
	}

	// Method to generate a new wave of enemies
	private void GenerateNewWave()
	{
		Debug.Log("generate wave called");

		_IsGeneratingEnemies = true;
		CurrentTime = TimeBetweenWaves;
		CurrentWave++;
		SpawnPoints = new List<GameObject>();

		// Get spawn points for enemies using the ChunkScriptV2/ChunkCreator
		SpawnPoints = Generator.GetSpawnPoints(_Player, NumberOfEnemiesPerWave, MinEnemyDistance, MaxEnemyDistance);

		// Spawn enemies at the determined spawn points
		foreach (GameObject t in SpawnPoints)
		{
			Transform enemy = Instantiate(EnemyPrefab, t.transform.position, Quaternion.identity);
			enemy.gameObject.GetComponentInChildren<AIDestinationSetter>().target = _Player;
			Enemies.Add(enemy.gameObject);
		}

		_IsGeneratingEnemies = false;
		_IsTimerRunning = true;
	}

	// Draw spawn points using Gizmos (visual debugging)
	private void OnDrawGizmos()
	{
		foreach (GameObject t in SpawnPoints)
		{
			Gizmos.color = Color.white;
			Gizmos.DrawCube(t.transform.position, Vector3.one);
		}
	}
}
