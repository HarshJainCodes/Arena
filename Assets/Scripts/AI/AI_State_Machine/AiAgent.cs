using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateType InitialStateType;
    public Transform playerTransform;
    // private Vector3 = new Vector3(32, 23, 21);
    [SerializeField] public float maxSightDistance = 17f;

    // Start is called before the first frame update
    void Start() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiChaseState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiIdeState());
        stateMachine.RegisterState(new AiAttackState());
        stateMachine.ChangeState(InitialStateType);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
