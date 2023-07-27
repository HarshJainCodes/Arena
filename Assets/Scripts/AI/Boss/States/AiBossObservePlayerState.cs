using Pathfinding.RVO.Sampled;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AiBossObservePlayerState : IAiBossState
{
    public AiBossStateType GetStateType()
    {
	    return AiBossStateType.ObservePlayer;
    }

    public void Enter(AiBossAgent bossAgent)
    {
	    bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
	    bossAgent.GetComponent<AIPath>().enabled = false;
	    bossAgent.GetComponent<Seeker>().enabled = false;
		bossAgent.BossHealth.IsInvulnerable = true;
	}

    public void Update(AiBossAgent bossAgent)
	{
		bossAgent.BossHealth.IsInvulnerable = true;
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;
		Vector3 lookPos = bossAgent.PlayerTransform.position - bossAgent.transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		bossAgent.transform.rotation = Quaternion.Slerp(bossAgent.transform.rotation, rotation, 0.5f);
		if (bossAgent.SpawnManager.Enemies.Count == 0 && bossAgent.SpawnManager.CurrentWave == bossAgent.SpawnManager.NumberOfWaves)
			bossAgent.StateMachine.ChangeState(AiBossStateType.GetInArena);
	}
	public void Exit(AiBossAgent bossAgent)
	{
	    bossAgent.BossHealth.IsInvulnerable = false;
	    bossAgent.GetComponent<AIDestinationSetter>().enabled = true;
	    bossAgent.GetComponent<AIPath>().enabled = true;
	    bossAgent.GetComponent<Seeker>().enabled = true;
	}
}
