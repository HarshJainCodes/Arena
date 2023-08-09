using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAIStateMachine : MonoBehaviour
{
    private bool _globalInterrupt = false;
    public bool GlobalInterrupt { get { return _globalInterrupt; } set { _globalInterrupt = value; } }

    public BossState CurrentState = BossState.None;

    public BossState NextState= BossState.None;

    private BossMain Boss;

    private BossStateInterface[] BossStates;
    public BossAIStateMachine(BossMain agent)
    {
        Boss = agent;
        BossStates = new BossStateInterface[20];
    }

    // Update is called once per frame
    public void update()
    {
        if(_globalInterrupt)
        {
            
            Boss.Aipath.enabled = false;
            BossStates[(int)CurrentState].exit(Boss);
            CurrentState = BossState.None;
            _globalInterrupt = false;
            BossStates[(int)CurrentState].enter(Boss);
            //set everything to default here. So all states become equally accessible.
        }
        BossStates[(int)CurrentState].update(Boss);
    }

    public void addStates(BossStateInterface state,BossState index)
    {
        BossStates[(int)index] = state;
    }

    public void ChangeState(BossState state)
    {
        BossStates[(int)CurrentState].exit(Boss);
        CurrentState = state;
        BossStates[(int)CurrentState].enter(Boss);
    }
}
