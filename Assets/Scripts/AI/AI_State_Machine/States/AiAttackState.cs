using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AiAttackState : AiState
{
	private float t = 1f;
	public AIPath aiPath;
	public AiStateType GetStateType()
	{
		return AiStateType.Attack;
	}

	public void Enter(AiAgent agent)
	{
		aiPath = agent.GetComponent<AIPath>();	
		agent.GetComponent<AIDestinationSetter>().enabled = false;
		agent.GetComponent<AIPath>().enabled = false;
		agent.GetComponent<Seeker>().enabled = false;
	}

	public void Update(AiAgent agent)
	{
		agent.GetComponent<AIDestinationSetter>().enabled = false;
		agent.GetComponent<AIPath>().enabled = false;
		agent.GetComponent<Seeker>().enabled = false;

		agent.transform.LookAt(agent.GetComponent<AIDestinationSetter>().target);
		Vector3 lookPos = agent.GetComponent<AIDestinationSetter>().target.position - agent.transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, 0.5f);
		t -= Time.deltaTime;
		if (t <= 0)
		{
			agent.GetComponentInChildren<Shooter>().Shoot();
			t = 1;
		}

		if (!aiPath.reachedEndOfPath)
		{
			agent.stateMachine.ChangeState(AiStateType.Chase);
		}
	}

	public void Exit(AiAgent agent)
	{
		agent.GetComponent<AIDestinationSetter>().enabled = true;
		agent.GetComponent<AIPath>().enabled = true;
		agent.GetComponent<Seeker>().enabled = true;
	}
}
