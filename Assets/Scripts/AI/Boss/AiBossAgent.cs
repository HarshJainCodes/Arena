using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AiBossAgent : MonoBehaviour
{

    public AiBossStateMachine StateMachine;
    public AiBossStateType InitialBossStateType;
    public AiBossStateType CurrentBossStateType;
    public Transform PlayerTransform;
    public Transform PatrolPoint;
    public Transform ArenaCentre;
    public SpawnManager SpawnManager;
	public BossHealth BossHealth;
    public float StopDistance = 6f;
    public bool InRange = false;


	void Start()
    {
		BossHealth = GetComponent<BossHealth>();
	    StateMachine = new AiBossStateMachine(this);
		StateMachine.RegisterState(new AiBossObservePlayerState());
	}

	void Update()
    {
		StateMachine.Update();
		CurrentBossStateType = StateMachine.CurrentBossStateType;
		if (StateMachine.CurrentBossStateType != CurrentBossStateType)
		{
			StateMachine.ChangeState(CurrentBossStateType);
		}
	}
}
