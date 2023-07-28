using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiBossPatrolState : IAiBossState
{
	// Reference to the boss agent
	public AiBossAgent BossAgent;

	// Reference to the AIPath component for movement
	public AIPath AiPath;

	// Range for picking random patrol points
	public float range = 120f;

	// Distance from the patrol point at which the boss considers reaching the destination
	public float endDistance;

	// Time interval for picking a new patrol point
	public float t = 8f;

	// Store the spawn point position
	private Vector3 _Spawn;

	// Get the state type (used to identify this state)
	public AiBossStateType GetStateType()
	{
		return AiBossStateType.Patrol;
	}

	// Called when entering this state
	public void Enter(AiBossAgent bossAgent)
	{
		BossAgent = bossAgent;
		_Spawn = BossAgent.transform.position;

		// Store the current endReachedDistance value of AIPath
		endDistance = BossAgent.GetComponent<AIPath>().endReachedDistance;

		// Get the AIPath component and enable necessary components
		AiPath = BossAgent.GetComponent<AIPath>();
		BossAgent.GetComponent<AIDestinationSetter>().enabled = true;
		BossAgent.GetComponent<AIPath>().enabled = true;
		BossAgent.GetComponent<Seeker>().enabled = true;

		// Set the maximum speed for patrolling
		AiPath.maxSpeed = bossAgent.BossPatrolSpeed;

		// Move to a random patrol point initially
		MoveTo(PickRandomPoint());
	}

	// Called each frame while in this state
	public void Update(AiBossAgent bossAgent)
	{
		// Enable necessary components each frame
		bossAgent.GetComponent<AIDestinationSetter>().enabled = true;
		bossAgent.GetComponent<AIPath>().enabled = true;
		bossAgent.GetComponent<Seeker>().enabled = true;

		// Check if the player is in sight or within attack range
		if (BossAgent.Sensor.IsInSight(BossAgent.PlayerTransform.gameObject) || BossAgent.InRange)
		{
			// If player is in sight or within range, switch to the chase state
			AiPath.maxSpeed = BossAgent.BossSpeed;
			BossAgent.GetComponent<AIPath>().endReachedDistance = endDistance;
			BossAgent.GetComponent<AIDestinationSetter>().target = BossAgent.PlayerTransform;
			BossAgent.StateMachine.ChangeState(AiBossStateType.Chase);
		}

		// Check if the patrol path is completed
		if (!AiPath.pathPending && (AiPath.reachedEndOfPath || !AiPath.hasPath))
		{
			// If patrol path is completed, pick a new random point and search for a new path
			MoveTo(PickRandomPoint());
			AiPath.SearchPath();
		}
	}

	// Called when exiting this state
	public void Exit(AiBossAgent bossAgent)
	{
		// Restore the maximum speed and endReachedDistance of AIPath
		AiPath.maxSpeed = bossAgent.BossSpeed;
		BossAgent.GetComponent<AIPath>().endReachedDistance = endDistance;

		// Set the target to the player's transform and disable AIPath components
		BossAgent.GetComponent<AIDestinationSetter>().target = BossAgent.PlayerTransform;
		BossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		BossAgent.GetComponent<AIPath>().enabled = false;
		BossAgent.GetComponent<Seeker>().enabled = false;
	}

	// Pick a random point within a range from the current position
	Vector3 PickRandomPoint()
	{
		Vector3 point = Random.insideUnitSphere * range;
		point.y = 0;
		point += BossAgent.transform.position;

		// Ensure the point is not too far from the spawn point to avoid going out of bounds
		while (DistanceFromSpawn(point) >= 800f)
		{
			point = Random.insideUnitSphere * range;
			point.y = 0;
			point += BossAgent.transform.position;
		}
		return point;
	}

	// Calculate the distance between a point and the spawn point
	private float DistanceFromSpawn(Vector3 point)
	{
		return (float)(Math.Sqrt(Math.Pow(point.x - _Spawn.x, 2) + Math.Pow(point.z - _Spawn.z, 2)));
	}

	// Move the boss to a specific position
	public void MoveTo(Vector3 position)
	{
		// Set the patrol point position
		BossAgent.PatrolPoint.position = position;

		// Set the endReachedDistance to a small value to reach the patrol point precisely
		BossAgent.GetComponent<AIPath>().endReachedDistance = 1f;
		BossAgent.GetComponent<AIDestinationSetter>().target = BossAgent.PatrolPoint;

		// If player is not in sight and not within range, continue patrolling
		if (!BossAgent.Sensor.IsInSight(BossAgent.PlayerTransform.gameObject) && !BossAgent.InRange)
		{
			BossAgent.GetComponent<AIPath>().endReachedDistance = 1f;
			BossAgent.GetComponent<AIDestinationSetter>().target = BossAgent.PatrolPoint;
		}
		else
		{
			// If player is in sight or within range, switch to the chase state
			AiPath.maxSpeed = BossAgent.BossSpeed;
			BossAgent.GetComponent<AIPath>().endReachedDistance = endDistance;
			BossAgent.GetComponent<AIDestinationSetter>().target = BossAgent.PlayerTransform;
			BossAgent.StateMachine.ChangeState(AiBossStateType.Chase);
		}
	}
}
