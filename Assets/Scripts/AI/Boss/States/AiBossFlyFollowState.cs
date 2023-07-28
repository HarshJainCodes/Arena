using System;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiBossFlyFollowState : IAiBossState
{
	public AIPath AIPath; // Reference to the AIPath component for boss movement
	private float t = 1f; // Time variable for controlling movement interval
	private Transform _TargetTransform; // Reference to the target's transform (not used in the current code)
	public AIPath ai; // Reference to the AIPath component (not used in the current code)
	Vector2 horizontal; // Vector for storing random horizontal offset

	// Function to get the state type of this boss state
	public AiBossStateType GetStateType()
	{
		return AiBossStateType.FlyFollow; // Return the state type as FlyFollow
	}

	// Function called when entering the FlyFollow state
	public void Enter(AiBossAgent bossAgent)
	{
		// Disable AI-related components for pathfinding and movement
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;

		// Generate a random horizontal offset within the boss's fly follow radius
		horizontal = Random.insideUnitCircle * bossAgent.BossFlyFollowRadius;

		// Disable rigidbody's kinematic and gravity to allow free movement
		bossAgent.GetComponentInParent<Rigidbody>().isKinematic = true;
		bossAgent.GetComponentInParent<Rigidbody>().useGravity = false;

		// Get the reference to the AIPath component (not used in the current code)
		ai = bossAgent.GetComponent<AIPath>();
	}

	// Function called during the FlyFollow state's update
	public void Update(AiBossAgent bossAgent)
	{
		// Disable AI-related components for pathfinding and movement
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;

		// Get the player's position
		var position = bossAgent.PlayerTransform.position;

		// Decrease the time interval 't' until the next random horizontal offset update
		t -= Time.deltaTime;

		// When the interval reaches zero, update the horizontal offset and reset the interval
		if (t <= 0)
		{
			horizontal = Random.insideUnitCircle * bossAgent.BossFlyFollowRadius;
			t = 3; // Set the interval to 3 seconds (adjust as needed)
		}

		// Calculate the target position based on the player's position and the random horizontal offset
		Vector3 targetPos = new Vector3(position.x + horizontal.x, 0, position.z + horizontal.y) + bossAgent.PlayerTransform.up * (bossAgent.BossFlyHeight + position.y);

		// Move the boss parent object towards the target position at a certain speed
		var parent = bossAgent.transform.parent;
		parent.transform.position = Vector3.MoveTowards(parent.transform.position, targetPos, bossAgent.BossSpeed * Time.deltaTime);

		// Check if the boss is within range of the player or if the player is in the boss's sight
		// (Note: The following lines are commented out, so the behavior doesn't change state based on range or sight)
		if (bossAgent.InRange || bossAgent.Sensor.IsInSight(bossAgent.PlayerTransform.gameObject))
		{
			// Perform actions to change state (e.g., bossAgent.StateMachine.ChangeState(...))
			// bossAgent.StateMachine.ChangeState(AiBossStateType.SpawnMinions);
		}
	}

	// Function called when exiting the FlyFollow state
	public void Exit(AiBossAgent bossAgent)
	{
		// Disable AI-related components for pathfinding and movement
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;

		// Enable rigidbody's kinematic to allow normal physics simulation
		bossAgent.GetComponentInParent<Rigidbody>().isKinematic = false;
	}
}
