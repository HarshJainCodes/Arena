using System;
using System.Collections;
using System.Collections.Generic;
using AI.Lev_1_AI.AI_State_Machine.States;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

public class AiAgent : MonoBehaviour
{
	// References to components and managers
	public AiStateMachine StateMachine;
	public AiStateType InitialStateType;
	public AiStateType CurrentStateType;
	[FormerlySerializedAs("playerTransform")] public Transform PlayerTransform;
	[FormerlySerializedAs("patrolPoint")] public Transform PatrolPoint;
	[FormerlySerializedAs("spawnManager")] public SpawnManager SpawnManager;

	// Movement and AI parameters
	// this is the only place you need to change the speed of the enemy and all animations and movement will be updated
	public float StopDistance = 6f;
	public bool InRange = false;
	public float Speed = 4f;
	public float PatrolSpeed = 2f;
	public float WalkingShootSpeed = 3.5f;
	public int Level = 1;
	public float Inaccuracy = 0.0f;
	[SerializeField] public float maxSightDistance = 17f;

	// Sensor component for enemy detection
	public AiSensor sensor;

	// Start is called before the first frame update
	void Start()
	{
		// Getting references to other components and initializing the state machine
		PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		SpawnManager = GameObject.FindGameObjectWithTag("SpawnMan").GetComponent<SpawnManager>();
		GetComponent<AIPath>().maxSpeed = Speed;

		// Creating the state machine and registering different AI states
		StateMachine = new AiStateMachine(this);
		StateMachine.RegisterState(new AiChaseState());
		StateMachine.RegisterState(new AiAttackSurroundState());
		StateMachine.RegisterState(new AiDeathState());
		StateMachine.RegisterState(new AiIdeState());
		StateMachine.RegisterState(new AiAttackState());
		StateMachine.RegisterState(new AiPatrolState());
		StateMachine.ChangeState(InitialStateType);
	}

	// Update is called once per frame
	void Update()
	{
		// Update the current state of the state machine
		StateMachine.Update();
		CurrentStateType = StateMachine.currentStateType;

		// Check if the state has changed and update the state machine accordingly
		if (StateMachine.currentStateType != CurrentStateType)
		{
			StateMachine.ChangeState(CurrentStateType);
		}

		// Check if the player is in range based on the StopDistance parameter
		InRange = StopDistance > Math.Abs((PlayerTransform.position - transform.position).magnitude);
	}
}
