using System;
using System.Collections;
using System.Collections.Generic;
using AI.Lev_1_AI.AI_State_Machine.States;
using Pathfinding;
using UnityEngine;

public class AiAgent : MonoBehaviour
{
	// References to components and managers
	public AiStateMachine stateMachine;
	public AiStateType InitialStateType;
	public AiStateType CurrentStateType;
	public Transform playerTransform;
	public Transform patrolPoint;
	public SpawnManager spawnManager;
	public float dotProduct;

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
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		spawnManager = GameObject.FindGameObjectWithTag("SpawnMan").GetComponent<SpawnManager>();
		GetComponent<AIPath>().maxSpeed = Speed;

		// Creating the state machine and registering different AI states
		stateMachine = new AiStateMachine(this);
		stateMachine.RegisterState(new AiChaseState());
		stateMachine.RegisterState(new AiAttackSurroundState());
		stateMachine.RegisterState(new AiDeathState());
		stateMachine.RegisterState(new AiIdeState());
		stateMachine.RegisterState(new AiAttackState());
		stateMachine.RegisterState(new AiPatrolState());
		stateMachine.ChangeState(InitialStateType);
	}

	// Update is called once per frame
	void Update()
	{
		// Update the current state of the state machine
		stateMachine.Update();
		CurrentStateType = stateMachine.currentStateType;

		// Check if the state has changed and update the state machine accordingly
		if (stateMachine.currentStateType != CurrentStateType)
		{
			stateMachine.ChangeState(CurrentStateType);
		}

		// Check if the player is in range based on the StopDistance parameter
		InRange = StopDistance > Math.Abs((playerTransform.position - transform.position).magnitude);
		// Alternatively, you can use the inRange() function to achieve the same result
	}

	// Function to check if the player is in range based on dot product calculations
	bool inRange()
	{
		Vector3 playerDirection = playerTransform.position - transform.position;
		if (playerDirection.magnitude > StopDistance)
		{
			return false;
		}
		Vector3 agentDirection = transform.forward;

		playerDirection.Normalize();

		dotProduct = Vector3.Dot(playerDirection, agentDirection);
		if (dotProduct > 0.8660254037f) // Approximately cos(30 degrees) for 180-degree cone
			return true;
		return false;
	}
}
