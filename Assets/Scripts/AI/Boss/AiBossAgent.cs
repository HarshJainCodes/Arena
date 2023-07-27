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
	public GameObject Sliders;
    public float StopDistance = 6f;
    public bool InRange = false;
    public float BossSpeed = 4f;
    public float BossPatrolSpeed = 4f;
    public float BossWalkingShootSpeed = 4f;
	public float BossPunchDamage = 6f;
    [HideInInspector] public AiSensor Sensor;


	void Start()
    {
		BossHealth = GetComponent<BossHealth>();
        Sensor = GetComponent<AiSensor>();
		Sliders.SetActive(false);
	    StateMachine = new AiBossStateMachine(this);
	    StateMachine.RegisterState(new AiBossObservePlayerState());
		StateMachine.RegisterState(new AiBossGetInArenaState());
		StateMachine.RegisterState(new AiBossPatrolState());
		StateMachine.RegisterState(new AiBossChaseState());
		StateMachine.RegisterState(new AiBossAttackState());
	}

	void Update()
    {
		StateMachine.Update();
		CurrentBossStateType = StateMachine.CurrentBossStateType;
		if (StateMachine.CurrentBossStateType != CurrentBossStateType)
		{
			StateMachine.ChangeState(CurrentBossStateType);
		}
		if(SpawnManager.CurrentWave == SpawnManager.NumberOfWaves)
			Sliders.SetActive(true);
	}
}
