using System;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiBossFlyFollowState : IAiBossState
{
	public AIPath AIPath;
	private float t = 1f;
	private Transform _TargetTransform;
	public AIPath ai;
	Vector2 horizontal;

	public AiBossStateType GetStateType()
	{
		return AiBossStateType.FlyFollow;
	}

	public void Enter(AiBossAgent bossAgent)
	{
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;
		horizontal = Random.insideUnitCircle * bossAgent.BossFlyFollowRadius;
		bossAgent.GetComponentInParent<Rigidbody>().isKinematic = true;
		// bossAgent.GetComponentInParent<Rigidbody>().useGravity = false;
		ai = bossAgent.GetComponent<AIPath>();
	}

	public void Update(AiBossAgent bossAgent)
	{
		bossAgent.GetComponentInParent<Rigidbody>().isKinematic = true;
		bossAgent.GetComponentInParent<Rigidbody>().useGravity = false;
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;
		var position = bossAgent.PlayerTransform.position;
		t -= Time.deltaTime;
		if(t <= 0)
		{
			var horizontal = Random.insideUnitCircle * bossAgent.BossFlyFollowRadius;
			t = 3;
		}
		
		Vector3 targetPos = new Vector3(position.x + horizontal.x, 0, position.z + horizontal.y) + bossAgent.PlayerTransform.up * (bossAgent.BossFlyHeight + position.y);

		var parent = bossAgent.transform.parent;
		parent.transform.position = Vector3.MoveTowards(parent.transform.position, targetPos, bossAgent.BossSpeed * Time.deltaTime);
		// bossAgent.gameObject.transform.position = _TargetTransform.position;

		if (bossAgent.InRange || bossAgent.Sensor.IsInSight(bossAgent.PlayerTransform.gameObject))
		{
			// bossAgent.StateMachine.ChangeState(AiBossStateType.SpawnMinions);
		};
	}

	public void Exit(AiBossAgent bossAgent)
	{
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;
		bossAgent.GetComponentInParent<Rigidbody>().isKinematic = false;
	}
}
