using Pathfinding.RVO.Sampled;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBossObservePlayerState : IAiBossState
{
    public AiBossStateType GetStateType()
    {
	    return AiBossStateType.ObservePlayer;
    }

    public void Enter(AiBossAgent bossAgent)
    { 
	    bossAgent.BossHealth.IsInvulnerable = true;
		bossAgent.GetComponent<Rigidbody>().AddForce(bossAgent.transform.up * 20f);
       bossAgent.gameObject.GetComponent<Animator>().Play("BossIdle1Anim");
    }

    public void Update(AiBossAgent bossAgent)
    {
		Vector3 lookPos = bossAgent.PlayerTransform.position - bossAgent.transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		bossAgent.transform.rotation = Quaternion.Slerp(bossAgent.transform.rotation, rotation, 0.5f);
	}
	public void Exit(AiBossAgent bossAgent)
	{
	    bossAgent.BossHealth.IsInvulnerable = false;
    }
}
