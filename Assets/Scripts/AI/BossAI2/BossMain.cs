using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMain : MonoBehaviour
{

    private BossAIStateMachine _stateMachine;
    [SerializeField] private BossStateInterface _none;
    [SerializeField] private BossStateInterface _idle;
    [SerializeField] private BossStateInterface _observe;
    [SerializeField] private BossStateInterface _chase;
    [SerializeField] private BossStateInterface _attack;
    [SerializeField] private BossStateInterface _jump;
    [SerializeField] private BossStateInterface _death;
    [SerializeField] private BossStateInterface _spawn;
    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = new BossAIStateMachine(this);
        _stateMachine.addStates(_none, BossState.None);
        _stateMachine.addStates(_idle, BossState.Idle);
        _stateMachine.addStates(_observe, BossState.Observe);
        _stateMachine.addStates(_chase, BossState.Chase);
        _stateMachine.addStates(_attack, BossState.Attack);
        _stateMachine.addStates(_jump, BossState.Jump);
        _stateMachine.addStates(_death, BossState.Death);
        _stateMachine.GlobalInterrupt = true;
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.update();
    }
}
