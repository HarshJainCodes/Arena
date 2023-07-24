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
    public float BossSpeed = 4f;
    public float BossPatrolSpeed = 4f;
    public float BossWalkingShootSpeed = 4f;
    [HideInInspector] public AiSensor Sensor;


	void Start()
    {
		BossHealth = GetComponent<BossHealth>();
        Sensor = GetComponent<AiSensor>();
	    StateMachine = new AiBossStateMachine(this);
	    StateMachine.RegisterState(new AiBossObservePlayerState());
		StateMachine.RegisterState(new AiBossGetInArenaState());
		StateMachine.RegisterState(new AiBossPatrolState());
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
