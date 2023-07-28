using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum representing different AI state types
public enum AiStateType
{
	Idle,
	Patrol,
	Chase,
	Attack,
	AttackSurround,
	Dead
}

// Interface for AI states
public interface IAiState
{
	// Method to get the type of the AI state
	AiStateType GetStateType();

	// Method called when entering the AI state
	void Enter(AiAgent agent);

	// Method called each frame to update the AI state
	void Update(AiAgent agent);

	// Method called when exiting the AI state
	void Exit(AiAgent agent);
}