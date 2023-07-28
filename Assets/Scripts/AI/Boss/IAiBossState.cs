// Credits: Ayush Gupta

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enumeration representing different states for the boss AI
public enum AiBossStateType
{
	ObservePlayer,
	GetInArena,
	Idle,
	Patrol,
	Chase,
	Punch,
	Attack,
	FlyFollow,
	Dead
}

// Interface for boss AI state classes
public interface IAiBossState
{
	// Method to get the type of the current AI state
	AiBossStateType GetStateType();

	// Method called when entering the AI state
	void Enter(AiBossAgent bossAgent);

	// Method called to update the AI state
	void Update(AiBossAgent bossAgent);

	// Method called when exiting the AI state
	void Exit(AiBossAgent bossAgent);
}