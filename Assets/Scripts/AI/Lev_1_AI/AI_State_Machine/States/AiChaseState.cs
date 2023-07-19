using System;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChaseState : IAiState
{
	public AIPath aiPath;
	private float t = 1f;
	public AiStateType GetStateType()
	{
		return AiStateType.Chase;
	}

	public void Enter(AiAgent agent)
	{
		aiPath = agent.GetComponent<AIPath>();
		agent.GetComponent<AIDestinationSetter>().enabled = true;
		agent.GetComponent<AIPath>().enabled = true;
		agent.GetComponent<Seeker>().enabled = true;
	}

	public void Update(AiAgent agent)
	{
		agent.GetComponent<AIDestinationSetter>().enabled = true;
		agent.GetComponent<AIPath>().enabled = true;
		agent.GetComponent<Seeker>().enabled = true;

		if (agent.InRange)
		{
			if(agent.Level == 1)
				agent.stateMachine.ChangeState(AiStateType.Attack);
			else if(agent.Level == 2)
				agent.stateMachine.ChangeState(AiStateType.AttackSurround);
		}
	}

	public void Exit(AiAgent agent)
	{
        agent.GetComponent<AIDestinationSetter>().enabled = false;
        agent.GetComponent<AIPath>().enabled = false;
        agent.GetComponent<Seeker>().enabled = false;
    }
}
