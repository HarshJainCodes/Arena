using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AiBossPunchState : IAiBossState
{
	private float t = 0.5f;
	public AIPath aiPath;

	// Get the state type (used to identify this state)
	public AiBossStateType GetStateType()
	{
		return AiBossStateType.Punch;
	}

	// Called when entering this state
	public void Enter(AiBossAgent bossAgent)
	{
		// Get AIPath component and set its maxSpeed to BossWalkingShootSpeed
		aiPath = bossAgent.GetComponent<AIPath>();
		aiPath.maxSpeed = bossAgent.BossWalkingShootSpeed;

		// Enable necessary components for movement and seeking
		bossAgent.GetComponent<AIDestinationSetter>().enabled = true;
		bossAgent.GetComponent<AIPath>().enabled = true;
		bossAgent.GetComponent<Seeker>().enabled = true;
	}

	// Called each frame while in this state
	public void Update(AiBossAgent bossAgent)
	{
		// Enable necessary components each frame
		bossAgent.GetComponent<AIDestinationSetter>().enabled = true;
		bossAgent.GetComponent<AIPath>().enabled = true;
		bossAgent.GetComponent<Seeker>().enabled = true;

		// Rotate the boss to face the target (player)
		Vector3 lookPos = bossAgent.GetComponent<AIDestinationSetter>().target.position - bossAgent.transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		bossAgent.transform.rotation = Quaternion.Slerp(bossAgent.transform.rotation, rotation, 0.5f);

		// Perform the punch action periodically
		t -= Time.deltaTime;
		if (t <= 0)
		{
			if (bossAgent.Sensor.IsInSight(bossAgent.PlayerTransform.gameObject))
				Punch(bossAgent);
			t = 1;
		}

		// Check if the player is no longer in range and not in sight, switch to the chase state
		if (!bossAgent.InRange && !bossAgent.Sensor.IsInSight(bossAgent.PlayerTransform.gameObject))
		{
			bossAgent.StateMachine.ChangeState(AiBossStateType.Chase);
		}
	}

	// Called when exiting this state
	public void Exit(AiBossAgent bossAgent)
	{
		// Restore AIPath's maxSpeed to BossSpeed
		aiPath.maxSpeed = bossAgent.BossSpeed;

		// Disable AIPath components
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;
	}

	// Perform the punch action
	private void Punch(AiBossAgent bossAgent)
	{
		// Play the punch animation
		bossAgent.GetComponentInParent<Animator>().Play("BossBoxingAnim");
		
		// Damage the player and apply a forward force to the player
		bossAgent.PlayerTransform.GetComponentInParent<PlayerHealth>().DamagePlayer(bossAgent.BossPunchDamage);
		bossAgent.PlayerTransform.GetComponentInParent<Rigidbody>().AddForce(bossAgent.transform.forward * 5000);
	}
}
