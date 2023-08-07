using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AiBossAgent : MonoBehaviour
{
	// Boss state machine and initial boss state type
	public AiBossStateMachine StateMachine;
	public AiBossStateType InitialBossStateType;

	// Current boss state type and references to important game objects
	public AiBossStateType CurrentBossStateType;
	public Transform PlayerTransform;
	public Transform PatrolPoint; // Reference to the patrol point transform for random wandering
	public Transform ArenaCentre; // Reference to the arena centre transform for getting in the arena state
	public SpawnManager SpawnManager;
	public BossHealth BossHealth;
	public GameObject Sliders;

	// Boss movement and behavior settings
	public float StopDistance = 6f;
	public bool InRange = false;
	public float BossSpeed = 4f;
	public float BossPatrolSpeed = 4f;
	public float BossWalkingShootSpeed = 4f;
	public float BossPunchDamage = 6f;
	public float BossFlyFollowRadius = 6f;
	public float BossFlyHeight = 30f;

	//Addtions by Malhar Choure for the boss minions
	[SerializeField] protected GameObject _spawnedEnemyPrefab;
	public GameObject SpawnedEnemy;
	public GameObject CurrentEnemy;
	//Additions end here

	[HideInInspector]
	public AiSensor Sensor; // Reference to the AI sensor script



	void Start()
	{
		// Get references to required components and objects
		BossHealth = GetComponent<BossHealth>(); // Reference to the boss health script
		Sensor = GetComponent<AiSensor>(); // Reference to the AI sensor script
		Sliders.SetActive(false); // Deactivate the sliders initially
		StateMachine = new AiBossStateMachine(this); // Create the boss state machine
													 // Register various boss states with the state machine
		StateMachine.RegisterState(new AiBossFlyFollowState());
		StateMachine.RegisterState(new AiBossObservePlayerState());
		StateMachine.RegisterState(new AiBossGetInArenaState());
		StateMachine.RegisterState(new AiBossPatrolState());
		StateMachine.RegisterState(new AiBossChaseState());
		StateMachine.RegisterState(new AiBossPunchState());
		StateMachine.RegisterState(new AiBossSpawnMobsState());
		StateMachine.ChangeState(InitialBossStateType); // Change to the initial boss state
	}

	void Update()
	{
		StateMachine.Update(); // Update the boss state machine
		CurrentBossStateType = StateMachine.CurrentBossStateType; // Get the current boss state
		if (StateMachine.CurrentBossStateType != CurrentBossStateType)
		{
			// Change to the new state if the current state has changed
			StateMachine.ChangeState(CurrentBossStateType);
		}
		// If it's the final wave, activate the sliders
		if (SpawnManager.CurrentWave == SpawnManager.NumberOfWaves)
			Sliders.SetActive(true);
	}

    //Malhar Choure was here
    public GameObject createInstance(GameObject obj)
	{ 
		GameObject created=Instantiate(obj,gameObject.transform);

		return created;
    }
}
