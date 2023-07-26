using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AiBossChaseState : IAiBossState
{
	public AIPath AIPath;
	private float t = 1f;

	public AiBossStateType GetStateType()
	{
		return AiBossStateType.Chase;
	}

	public void Enter(AiBossAgent bossAgent)
	{
		bossAgent.GetComponent<AIDestinationSetter>().enabled = true;
		bossAgent.GetComponent<AIPath>().enabled = true;
		bossAgent.GetComponent<Seeker>().enabled = true;
	}

	public void Update(AiBossAgent bossAgent)
	{
		bossAgent.GetComponent<AIDestinationSetter>().enabled = true;
		bossAgent.GetComponent<AIPath>().enabled = true;
		bossAgent.GetComponent<Seeker>().enabled = true;

		if (bossAgent.InRange || bossAgent.Sensor.IsInSight(bossAgent.PlayerTransform.gameObject))
		{
			// bossAgent.StateMachine.ChangeState(AiBossStateType.Attack);
		}
	}

	public void Exit(AiBossAgent bossAgent)
	{
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;
	}
}
