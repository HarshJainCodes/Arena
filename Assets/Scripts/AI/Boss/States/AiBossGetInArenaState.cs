using Pathfinding;
using UnityEngine;

public class AiBossGetInArenaState : IAiBossState
{
	bool jumpStart = true; // A flag to indicate if the boss is starting the jump motion (not used in the current code)
	private float t = 5f; // Time variable for controlling the duration of the GetInArena state

	// Function to get the state type of this boss state
	public AiBossStateType GetStateType()
	{
		return AiBossStateType.GetInArena; // Return the state type as GetInArena
	}

	// Function called when entering the GetInArena state
	public void Enter(AiBossAgent bossAgent)
	{
		// Disable AI-related components for pathfinding and movement
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;

		// Make the boss invulnerable during this state
		bossAgent.BossHealth.IsInvulnerable = true;

		// Teleport the boss to the center of the arena (assuming the arena center is specified by bossAgent.ArenaCentre.position)
		bossAgent.transform.position = bossAgent.ArenaCentre.position;
	}

	// Function called during the GetInArena state's update
	public void Update(AiBossAgent bossAgent)
	{
		// Disable AI-related components for pathfinding and movement
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;

		// Perform a trajectory jump motion with initial speed and angle towards the arena center (to be added, not implemented in the current code)

		//dummy code to make the boss jump towards the arena center
		// Reduce the time 't' until the next state transition (to the Patrol state in this case)
		t -= Time.deltaTime;
		if (t <= 0)
		{
			bossAgent.StateMachine.ChangeState(AiBossStateType.Patrol); // Transition to the Patrol state after the specified time (5 seconds in this case)
		}
	}

	// Function called when exiting the GetInArena state
	public void Exit(AiBossAgent bossAgent)
	{
		// Make the boss vulnerable again when exiting the state
		bossAgent.BossHealth.IsInvulnerable = false;

		// Enable AI-related components for pathfinding and movement
		bossAgent.GetComponent<AIDestinationSetter>().enabled = true;
		bossAgent.GetComponent<AIPath>().enabled = true;
		bossAgent.GetComponent<Seeker>().enabled = true;
	}
}
