using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDeathState : IAiState
{
	// Method to get the type of the AI state
	public AiStateType GetStateType()
	{
		return AiStateType.Dead;
	}

	// Method called when entering the AI state
	public void Enter(AiAgent agent)
	{
		// Disable AIPath and Seeker components
		agent.GetComponent<AIDestinationSetter>().enabled = false;
		agent.GetComponent<AIPath>().enabled = false;
		agent.GetComponent<Seeker>().enabled = false;
	}

	// Method called each frame to update the AI state
	public void Update(AiAgent agent)
	{
		// Disable AIPath and Seeker components
		agent.GetComponent<AIDestinationSetter>().enabled = false;
		agent.GetComponent<AIPath>().enabled = false;
		agent.GetComponent<Seeker>().enabled = false;
	}

	// Method called when exiting the AI state
	public void Exit(AiAgent agent)
	{
		// This method is empty for the death state, as no behavior is needed when exiting this state
		// You may add additional functionality here if required, such as respawning the agent or
		// triggering any post-death actions.
	}
}