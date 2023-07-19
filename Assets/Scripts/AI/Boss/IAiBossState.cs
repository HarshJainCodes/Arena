using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiBossStateType
{
	ObservePlayer,
	Idle,
	Patrol,
	Chase,
	Attack,
	AttackSurround,
	Dead
}

public interface IAiBossState
{
	AiBossStateType GetStateType();
	void Enter(AiBossAgent bossAgent);
	void Update(AiBossAgent bossAgent);
	void Exit(AiBossAgent bossAgent);
}