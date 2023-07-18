using System;
using System.Collections;
using System.Collections.Generic;
using AI.Lev_1_AI.AI_State_Machine.States;
using Pathfinding;
using UnityEngine;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateType InitialStateType;
    public AiStateType CurrentStateType;
    public Transform playerTransform;
    public Transform patrolPoint;
    public SpawnManager spawnManager;
    public float dotProduct;
    public float StopDistance = 6f;
    public float MinWanderSpeed = 0.1f;
    public bool InRange = false;
    public int Level = 1;
	[SerializeField] public float maxSightDistance = 17f;
    [HideInInspector]public AiSensor sensor;

    // Start is called before the first frame update
    void Start() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        spawnManager = GameObject.FindGameObjectWithTag("SpawnMan").GetComponent<SpawnManager>();
        sensor = GetComponent<AiSensor>();
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiChaseState());
        stateMachine.RegisterState(new AiAttackSurroundState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiIdeState());
        stateMachine.RegisterState(new AiAttackState());
        stateMachine.RegisterState(new AiPatrolState());
        stateMachine.ChangeState(InitialStateType);
    }

    // Update is called once per frame
    void Update()
    {
	    stateMachine.Update();
        CurrentStateType = stateMachine.currentStateType;
        if (stateMachine.currentStateType != CurrentStateType)
        {
	        stateMachine.ChangeState(CurrentStateType);
        }
		// if the distance between the player and the enemy is lesser than the endReachedDistance
		InRange = StopDistance > Math.Abs(
			(playerTransform.position - transform.position).magnitude);
		// InRange = inRange();
		// if (sensor.IsInSight(playerTransform.gameObject) && InRange)
		// {
		//  if (Level == 1)
		//   stateMachine.ChangeState(AiStateType.Attack);
		//  else if (Level == 2)
		//   stateMachine.ChangeState(AiStateType.AttackSurround);
		// }

	}

    bool inRange()
    {
	    Vector3 playerDirection = playerTransform.position - transform.position;
	    if (playerDirection.magnitude > StopDistance)
	    {
		    return false;
	    }
	    Vector3 agentDirection = transform.forward;

	    playerDirection.Normalize();

	    dotProduct = Vector3.Dot(playerDirection, agentDirection);
	    if (dotProduct > 0.8660254037f) return true;
        return false;
	}
}
