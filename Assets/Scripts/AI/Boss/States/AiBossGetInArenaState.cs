using Pathfinding;
using UnityEngine;

public class AiBossGetInArenaState : IAiBossState
{
	bool jumpStart = true;
	private float t = 5f;
	public AiBossStateType GetStateType()
	{
	    return AiBossStateType.GetInArena;
	}

	public void Enter(AiBossAgent bossAgent)
	{
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;
		bossAgent.BossHealth.IsInvulnerable = true;
		bossAgent.transform.position = bossAgent.ArenaCentre.position;
	}

	public void Update(AiBossAgent bossAgent)
	{
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;
		//trajectory jump motion with initial speed and angle to arena centre to be added

		// bossAgent.transform.position = Vector3.MoveTowards(bossAgent.transform.position, bossAgent.ArenaCentre.position, bossAgent.BossSpeed * Time.deltaTime);
		t -= Time.deltaTime;
		if (t <= 0)
		{
			bossAgent.StateMachine.ChangeState(AiBossStateType.Patrol);
		}
	}

	public void Exit(AiBossAgent bossAgent)
	{
	    bossAgent.BossHealth.IsInvulnerable = false;
	    bossAgent.GetComponent<AIDestinationSetter>().enabled = true;
	    bossAgent.GetComponent<AIPath>().enabled = true;
	    bossAgent.GetComponent<Seeker>().enabled = true;
	}
}