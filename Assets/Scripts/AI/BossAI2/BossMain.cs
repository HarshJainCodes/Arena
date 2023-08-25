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
    public float DashTime=6f;
    public float AttackTime = 4f;
    public float JumpTime = 4f;
    public CharacterController CharController;
    public GameObject enemySpawn;
    public GameObject flyingSpawn;
    public GameObject currentSpawn;
    public int waveNo;
    public Transform minionsParent;
    public Transform bossWeaponTransform;
    public GameObject explosion;
    public GameObject particleTrail;
    public GameObject LandingExplosion;

    /// <summary>
    /// This is the state machine that holds and switches between various states.
    /// </summary>
    [Tooltip("This is already set by script")]
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
    private BossStateInterface _start = new StartState();

    private void Awake()
    {
        currentSpawn = enemySpawn;
        _stateMachine = new BossAIStateMachine(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        DestinationSetter.target = Target;
        _stateMachine.addStates(_none, BossState.None);
        _stateMachine.addStates(_idle, BossState.Idle);
        _stateMachine.addStates(_observe, BossState.Observe);
        _stateMachine.addStates(_chase, BossState.Chase);
        _stateMachine.addStates(_attack, BossState.Attack);
        _stateMachine.addStates(_jump, BossState.Jump);
        _stateMachine.addStates(_death, BossState.Death);
        _stateMachine.addStates(_spawn, BossState.Spawn);
        _stateMachine.addStates(_dash, BossState.Dash);
        _stateMachine.addStates(_start,BossState.Start);
        _stateMachine.ChangeState(BossState.Observe);
       
        //_stateMachine.GlobalInterrupt = true;
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.update();
        //Debug.Log(_stateMachine.CurrentState);
    }

    public void changeState(BossState state)
    {
        Debug.Log(_stateMachine.CurrentState +" to "+ state);
        _stateMachine.ChangeState(state);
    }
}
