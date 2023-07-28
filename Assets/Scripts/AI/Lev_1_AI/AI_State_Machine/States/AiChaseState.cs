using System;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChaseState : IAiState
{
	public AIPath aiPath;
	private float t = 1f;

	// Method to get the type of the AI state
	public AiStateType GetStateType()
	{
		return AiStateType.Chase;
	}

	// Method called when entering the AI state
	public void Enter(AiAgent agent)
	{
		aiPath = agent.GetComponent<AIPath>();
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

		// Check if the player is within attack range or in sight of the agent
		if (agent.InRange || agent.sensor.IsInSight(agent.PlayerTransform.gameObject))
		{
			// Depending on the agent's level, change state to Attack or AttackSurround
			if (agent.Level == 1)
			{
				agent.StateMachine.ChangeState(AiStateType.Attack);
			}
			else if (agent.Level == 2)
			{
				agent.StateMachine.ChangeState(AiStateType.AttackSurround);
			}
		}
	}

	// Method called when exiting the AI state
	public void Exit(AiAgent agent)
	{
		// Disable AIPath and Seeker components
		agent.GetComponent<AIDestinationSetter>().enabled = false;
		agent.GetComponent<AIPath>().enabled = false;
		agent.GetComponent<Seeker>().enabled = false;
	}
}