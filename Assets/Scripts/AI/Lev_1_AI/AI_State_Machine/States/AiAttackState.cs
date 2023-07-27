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
	public AiStateType GetStateType()
	{
		return AiStateType.Attack;
	}

	public void Enter(AiAgent agent)
	{
		aiPath = agent.GetComponent<AIPath>();
		aiPath.maxSpeed = agent.WalkingShootSpeed;
		agent.GetComponent<AIDestinationSetter>().enabled = true;
		agent.GetComponent<AIPath>().enabled = true;
		agent.GetComponent<Seeker>().enabled = true;
	}

	public void Update(AiAgent agent)
	{
		agent.GetComponent<AIDestinationSetter>().enabled = true;
		agent.GetComponent<AIPath>().enabled = true;
		agent.GetComponent<Seeker>().enabled = true;
		
		Vector3 lookPos = agent.GetComponent<AIDestinationSetter>().target.position - agent.transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, 0.5f);
		t -= Time.deltaTime;
		if (t <= 0)
		{
			if(agent.sensor.IsInSight(agent.PlayerTransform.gameObject))
				agent.GetComponentInChildren<Shooter>().Shoot();
			t = 1;
		}

		if (!agent.InRange && !agent.sensor.IsInSight(agent.PlayerTransform.gameObject))
		{
			agent.StateMachine.ChangeState(AiStateType.Chase);
		}
	}

	public void Exit(AiAgent agent)
	{
		aiPath.maxSpeed = agent.Speed;
		agent.GetComponent<AIDestinationSetter>().enabled = false;
		agent.GetComponent<AIPath>().enabled = false;
		agent.GetComponent<Seeker>().enabled = false;
	}
}
