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
	public const string BossIdleAnim = "BossIdle1Anim";

	AiBossStateType GetStateType();
	void Enter(AiBossAgent bossAgent);
	void Update(AiBossAgent bossAgent);
	void Exit(AiBossAgent bossAgent);
}