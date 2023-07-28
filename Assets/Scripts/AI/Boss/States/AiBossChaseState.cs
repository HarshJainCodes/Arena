using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AiBossChaseState : IAiBossState
{
	public AIPath AIPath; // Reference to the AIPath component for boss movement

	private float t = 1f; // Time variable for controlling boss movement speed

	// Function to get the state type of this boss state
	public AiBossStateType GetStateType()
	{
		return AiBossStateType.Chase; // Return the state type as Chase
	}

	// Function called when entering the Chase state
	public void Enter(AiBossAgent bossAgent)
	{
		// Enable AI-related components for pathfinding and movement
		bossAgent.GetComponent<AIDestinationSetter>().enabled = true;
		bossAgent.GetComponent<AIPath>().enabled = true;
		bossAgent.GetComponent<Seeker>().enabled = true;
	}

	// Function called during the Chase state's update
	public void Update(AiBossAgent bossAgent)
	{
		// Enable AI-related components for pathfinding and movement
		bossAgent.GetComponent<AIDestinationSetter>().enabled = true;
		bossAgent.GetComponent<AIPath>().enabled = true;
		bossAgent.GetComponent<Seeker>().enabled = true;

		// If the player is within the stop distance or is in the boss's sight, transition to the Punch state
		if (bossAgent.InRange || bossAgent.Sensor.IsInSight(bossAgent.PlayerTransform.gameObject))
		{
			bossAgent.StateMachine.ChangeState(AiBossStateType.Punch);
		}
	}

	// Function called when exiting the Chase state
	public void Exit(AiBossAgent bossAgent)
	{
		// Disable AI-related components for pathfinding and movement
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;
	}
}