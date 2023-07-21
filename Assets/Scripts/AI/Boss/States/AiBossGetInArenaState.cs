using UnityEngine;

public class AiBossGetInArenaState : IAiBossState
{
	public AiBossStateType GetStateType()
	{
	    return AiBossStateType.GetInArena;
	}

	public void Enter(AiBossAgent bossAgent)
	{
	    bossAgent.BossHealth.IsInvulnerable = true;
		// bossAgent.gameObject.GetComponent<Animator>().Play("BossIdle1Anim");
	}

	public void Update(AiBossAgent bossAgent)
	{
		//trajectory jump motion with initial speed and angle to arena centre

	}

	public void Exit(AiBossAgent bossAgent)
	{
	    bossAgent.BossHealth.IsInvulnerable = false;
	}
}