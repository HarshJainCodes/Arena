using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateType
{
	Idle,
	Patrol,
	Chase,
	Attack,
	AttackSurround,
	Dead
}

public interface IAiState 
{
    AiStateType GetStateType();
	void Enter(AiAgent agent);
	void Update(AiAgent agent);
	void Exit(AiAgent agent);
}
