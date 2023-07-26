using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdeState : IAiState
{
	public float dotProduct;

	public AiStateType GetStateType()
	{
		return AiStateType.Idle;
	}

	public void Enter(AiAgent agent)
	{
		agent.GetComponent<AIDestinationSetter>().enabled = false;
		agent.GetComponent<AIPath>().enabled = false;
		agent.GetComponent<Seeker>().enabled = false;
	}

	public void Update(AiAgent agent)
	{
		agent.GetComponent<AIDestinationSetter>().enabled = false;
		agent.GetComponent<AIPath>().enabled = false;
		agent.GetComponent<Seeker>().enabled = false;

		Vector3 playerDirection = agent.PlayerTransform.position - agent.transform.position;
		if (playerDirection.magnitude > agent.maxSightDistance)
		{
			return;
		}
		Vector3 agentDirection = agent.transform.forward;

		playerDirection.Normalize();

		dotProduct = Vector3.Dot(playerDirection, agentDirection);
		if (dotProduct > 0.8660254037f)
		{
			agent.GetComponent<AIDestinationSetter>().enabled = true;
			agent.GetComponent<AIPath>().enabled = true;
			agent.GetComponent<Seeker>().enabled = true;
			agent.StateMachine.ChangeState(AiStateType.Chase);
		}
	}

	public void Exit(AiAgent agent)
	{
		agent.GetComponent<AIDestinationSetter>().enabled = true;
		agent.GetComponent<AIPath>().enabled = true;
		agent.GetComponent<Seeker>().enabled = true;
	}
}
