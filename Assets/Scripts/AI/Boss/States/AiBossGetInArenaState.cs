using UnityEngine;

public class AiBossGetInArenaState : IAiBossState
{
	bool jumpStart = true;
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
		//trajectory jump motion with initial speed and angle to arena centre to be added

	}

	public void Exit(AiBossAgent bossAgent)
	{
	    bossAgent.BossHealth.IsInvulnerable = false;
	}
}