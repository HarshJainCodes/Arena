using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdeState : IAiState
{
	// Variable to store the dot product between player direction and agent direction
	public float dotProduct;

	// Method to get the type of the AI state
	public AiStateType GetStateType()
	{
		return AiStateType.Idle;
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

		// Calculate the direction from the agent to the player
		Vector3 playerDirection = agent.PlayerTransform.position - agent.transform.position;

		// Check if the player is within the max sight distance of the agent
		if (playerDirection.magnitude > agent.maxSightDistance)
		{
			return; // Player is too far, do nothing
		}

		// Calculate the direction the agent is facing
		Vector3 agentDirection = agent.transform.forward;

		// Normalize the direction vectors
		playerDirection.Normalize();

		// Calculate the dot product between player direction and agent direction
		dotProduct = Vector3.Dot(playerDirection, agentDirection);

		// If the dot product is greater than a threshold value (approximately 30 degrees),
		// switch to the chase state and enable AIPath and Seeker components
		if (dotProduct > 0.8660254037f)
		{
			agent.GetComponent<AIDestinationSetter>().enabled = true;
			agent.GetComponent<AIPath>().enabled = true;
			agent.GetComponent<Seeker>().enabled = true;
			agent.StateMachine.ChangeState(AiStateType.Chase);
		}
	}

	// Method called when exiting the AI state
	public void Exit(AiAgent agent)
	{
		// Enable AIPath and Seeker components
		agent.GetComponent<AIDestinationSetter>().enabled = true;
		agent.GetComponent<AIPath>().enabled = true;
		agent.GetComponent<Seeker>().enabled = true;
	}
}
