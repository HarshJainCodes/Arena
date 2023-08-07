using Pathfinding.RVO.Sampled;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AiBossObservePlayerState : IAiBossState
{
	// Function to get the state type of this boss state
	public AiBossStateType GetStateType()
	{
		return AiBossStateType.ObservePlayer;
	}

	// Function called when entering the ObservePlayer state
	public void Enter(AiBossAgent bossAgent)
	{
		// Disable AI-related components for pathfinding and movement
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;

		// Make the boss invulnerable while observing the player
		bossAgent.BossHealth.IsInvulnerable = true;
	}

	// Function called during the ObservePlayer state's update
	public void Update(AiBossAgent bossAgent)
	{
		// Make sure the boss remains invulnerable while observing the player
		bossAgent.BossHealth.IsInvulnerable = true;

		// Disable AI-related components for pathfinding and movement
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;

		// Calculate the direction to look at the player and adjust the boss's rotation smoothly
		Vector3 lookPos = bossAgent.PlayerTransform.position - bossAgent.transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		bossAgent.transform.rotation = Quaternion.Slerp(bossAgent.transform.rotation, rotation, 0.5f);

        if (bossAgent.CurrentEnemy.transform.childCount == 0 && bossAgent.SpawnManager.CurrentWave == bossAgent.SpawnManager.NumberOfWaves && bossAgent.SpawnManager.Enemies.Count == 0)
        {
            bossAgent.StateMachine.ChangeState(AiBossStateType.SpawnMobs);
        }
        // If there are no other enemies and it's the final wave of the game, transition to the GetInArena state //changes made to next state transition by Malhar Choure
        /*if (bossAgent.SpawnManager.Enemies.Count == 0 && bossAgent.SpawnManager.CurrentWave == bossAgent.SpawnManager.NumberOfWaves)
			bossAgent.StateMachine.ChangeState(AiBossStateType.SpawnMobs);*/
		
	}

	// Function called when exiting the ObservePlayer state
	public void Exit(AiBossAgent bossAgent)
	{
		// Make the boss no longer invulnerable when exiting the ObservePlayer state
		bossAgent.BossHealth.IsInvulnerable = false;

		// Re-enable AI-related components for pathfinding and movement
		bossAgent.GetComponent<AIDestinationSetter>().enabled = false;
		bossAgent.GetComponent<AIPath>().enabled = false;
		bossAgent.GetComponent<Seeker>().enabled = false;
		bossAgent.GetComponentInParent<aiFollow>().enabled = false;
	}
}
