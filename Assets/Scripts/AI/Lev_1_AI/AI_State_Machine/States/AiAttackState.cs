using System;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AiAttackState : IAiState
{
	private float t = 0.5f;
	public AIPath aiPath;

	// Method to get the type of the AI state
	public AiStateType GetStateType()
	{
		return AiStateType.Attack;
	}

	// Method called when entering the AI state
	public void Enter(AiAgent agent)
	{
		aiPath = agent.GetComponent<AIPath>();
		aiPath.maxSpeed = agent.WalkingShootSpeed;
		agent.GetComponent<AIDestinationSetter>().enabled = true;
		agent.GetComponent<AIPath>().enabled = true;
		agent.GetComponent<Seeker>().enabled = true;
	}

	// Method called each frame to update the AI state
	public void Update(AiAgent agent)
	{
		agent.GetComponent<AIDestinationSetter>().enabled = true;
		agent.GetComponent<AIPath>().enabled = true;
		agent.GetComponent<Seeker>().enabled = true;

		// Rotate the agent to face the target (player)
		Vector3 lookPos = agent.GetComponent<AIDestinationSetter>().target.position - agent.transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, 0.5f);

		// Delay before performing the attack action
		t -= Time.deltaTime;
		if (t <= 0)
		{
			if (agent.sensor.IsInSight(agent.PlayerTransform.gameObject))
			{
				// Perform the attack action (e.g., shooting)
				agent.GetComponentInChildren<Shooter>().Shoot();
			}
			t = 1;
		}

		// If the player is out of attack range and not in sight, transition back to the Chase state
		if (!agent.InRange && !agent.sensor.IsInSight(agent.PlayerTransform.gameObject))
		{
			agent.StateMachine.ChangeState(AiStateType.Chase);
		}
	}

	// Method called when exiting the AI state
	public void Exit(AiAgent agent)
	{
		// Reset the AIPath maxSpeed to the default speed
		aiPath.maxSpeed = agent.Speed;
		// Disable the AIDestinationSetter, AIPath, and Seeker components
		agent.GetComponent<AIDestinationSetter>().enabled = false;
		agent.GetComponent<AIPath>().enabled = false;
		agent.GetComponent<Seeker>().enabled = false;
	}
}
