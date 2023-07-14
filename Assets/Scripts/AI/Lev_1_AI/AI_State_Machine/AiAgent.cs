using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateType InitialStateType;
    public Transform playerTransform;
    public float StopDistance = 6f;
    public bool InRange = false;
    public int Level = 1;
	[SerializeField] public float maxSightDistance = 17f;
    [HideInInspector]public AiSensor sensor;

    // Start is called before the first frame update
    void Start() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        sensor = GetComponent<AiSensor>();
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiChaseState());
        stateMachine.RegisterState(new AiAttackSurroundState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiIdeState());
        stateMachine.RegisterState(new AiAttackState());
        stateMachine.ChangeState(InitialStateType);
    }

    // Update is called once per frame
    void Update()
    {
	    stateMachine.Update();

	    // if the distance between the player and the enemy is lesser than the endReachedDistance
	    InRange = StopDistance > Math.Abs(
		    (GetComponent<AIDestinationSetter>().target.position - transform.position).magnitude);
    }
}
