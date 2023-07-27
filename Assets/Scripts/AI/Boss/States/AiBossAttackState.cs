using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AiBossAttackState : IAiBossState
{
	private float t = 0.5f;
	public AIPath aiPath;

	public AiBossStateType GetStateType()
	{
		return AiBossStateType.Attack;
	}

	public void Enter(AiBossAgent bossAgent)
	{
		aiPath = bossAgent.GetComponent<AIPath>();
		aiPath.maxSpeed = bossAgent.BossWalkingShootSpeed;
		bossAgent.GetComponent<AIDestinationSetter>().enabled = true;
		bossAgent.GetComponent<AIPath>().enabled = true;
		bossAgent.GetComponent<Seeker>().enabled = true;
	}

	public void Update(AiBossAgent bossAgent)
	{
		bossAgent.GetComponent<AIDestinationSetter>().enabled = true;
		bossAgent.GetComponent<AIPath>().enabled = true;
		bossAgent.GetComponent<Seeker>().enabled = true;
		
		Vector3 lookPos = bossAgent.GetComponent<AIDestinationSetter>().target.position - bossAgent.transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		bossAgent.transform.rotation = Quaternion.Slerp(bossAgent.transform.rotation, rotation, 0.5f);
		t -= Time.deltaTime;
		if (t <= 0)
		{
			if(bossAgent.Sensor.IsInSight(bossAgent.PlayerTransform.gameObject))
				Punch(bossAgent);
			t = 1;
		}

		if (!bossAgent.InRange && !bossAgent.Sensor.IsInSight(bossAgent.PlayerTransform.gameObject))
		{
			bossAgent.StateMachine.ChangeState(AiBossStateType.Chase);
		}
	}

	public void Exit(AiBossAgent bossAgent)
	{
		aiPath.maxSpeed = bossAgent.BossSpeed;
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;
	}

	private void Punch(AiBossAgent bossAgent)
	{
		// throw new System.NotImplementedException();
		bossAgent.GetComponentInParent<Animator>().Play("BossBoxingAnim");
		bossAgent.PlayerTransform.GetComponentInParent<PlayerHealth>().DamagePlayer(bossAgent.BossPunchDamage);
		bossAgent.PlayerTransform.GetComponentInParent<Rigidbody>().AddForce(bossAgent.transform.forward * 5000);
	}
}
