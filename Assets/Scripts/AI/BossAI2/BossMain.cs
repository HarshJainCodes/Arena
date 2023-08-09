using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMain : MonoBehaviour
{
    public AIPath Aipath;
    public AIDestinationSetter DestinationSetter;
    //public AiSensor aiSensor;
    public SpawnManager SpawnManager;
    public Transform[] JumpSpots;
    public Transform Target;
    public float topFloorHeight;
    public Animator _animationController;

    /// <summary>
    /// This is the state machine that holds and switches between various states.
    /// </summary>
    public BossAIStateMachine _stateMachine;
    private BossStateInterface _none=new NoneState();
    private BossStateInterface _idle= new IdleState();
    private BossStateInterface _observe=new ObserveState();
    private BossStateInterface _chase=new ChaseState();
    private BossStateInterface _attack=new AttackState();
    private BossStateInterface _jump=new JumpState();
    private BossStateInterface _death=new DeathState();
    private BossStateInterface _spawn=new SpawnState();
    private BossStateInterface _dash=new DashState();
    // Start is called before the first frame update
    void Start()
    {
        DestinationSetter.target = Target;
        _stateMachine = new BossAIStateMachine(this);
        _stateMachine.addStates(_none, BossState.None);
        _stateMachine.addStates(_idle, BossState.Idle);
        _stateMachine.addStates(_observe, BossState.Observe);
        _stateMachine.addStates(_chase, BossState.Chase);
        _stateMachine.addStates(_attack, BossState.Attack);
        _stateMachine.addStates(_jump, BossState.Jump);
        _stateMachine.addStates(_death, BossState.Death);
        _stateMachine.addStates(_death, BossState.Spawn);
        _stateMachine.addStates(_dash, BossState.Dash);
        _stateMachine.GlobalInterrupt = true;
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.update();
    }

    public void changeState(BossState state)
    {
        _stateMachine.ChangeState(state);
    }
}
