using System;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackSurroundState : AiState
{
	private float t = 1f;
	public AIPath aiPath;
	public AiStateType GetStateType()
	{
		return AiStateType.AttackSurround;
	}

	public void Enter(AiAgent agent)
	{
		aiPath = agent.GetComponent<AIPath>();
		agent.GetComponent<AIDestinationSetter>().enabled = true;
		agent.GetComponent<AIPath>().enabled = true;
		agent.GetComponent<Seeker>().enabled = true;
		SurroundManager.Instance.Units.Add(agent.GetComponent<EnemyUnit>());
	}

	public void Update(AiAgent agent)
	{
		agent.GetComponent<AIDestinationSetter>().enabled = true;
		agent.GetComponent<AIPath>().enabled = true;
		agent.GetComponent<Seeker>().enabled = true;

		Vector3 lookPos = SurroundManager.Instance.Target.position - agent.transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, 0.5f);
		t -= Time.deltaTime;
		if (t <= 0)
		{
			if (agent.sensor.IsInSight(agent.playerTransform.gameObject))
				agent.GetComponentInChildren<Shooter>().Shoot();
			t = 1;
		}

		if (!agent.InRange)
		{
			agent.stateMachine.ChangeState(AiStateType.Chase);
		}
	}

	public void Exit(AiAgent agent)
	{
		agent.GetComponent<AIDestinationSetter>().enabled = false;
		agent.GetComponent<AIPath>().enabled = false;
		agent.GetComponent<Seeker>().enabled = false;
	}
}