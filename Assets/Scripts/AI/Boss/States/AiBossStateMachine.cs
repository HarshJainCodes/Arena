
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBossStateMachine 
{
    public IAiBossState[] states;
    public AiBossAgent bossAgent;
    public AiBossStateType CurrentBossStateType;

    public AiBossStateMachine(AiBossAgent bossAgent)
    {
	    this.bossAgent = bossAgent;
		int numStates = System.Enum.GetNames(typeof(AiBossStateType)).Length;
		states = new IAiBossState[numStates];
	}

    public void RegisterState(IAiBossState state)
    {
        int index = (int)state.GetStateType();
        states[index] = state;
    }

    public IAiBossState GetState(AiBossStateType stateType)
    {
		int index = (int)stateType;
		return states[index];
	}
    
    public void Update()
    {
        GetState(CurrentBossStateType)?.Update(bossAgent);
    }

    public void ChangeState(AiBossStateType stateType)
    {
	    if (CurrentBossStateType != stateType)
	    {
            GetState(CurrentBossStateType)?.Exit(bossAgent);
            CurrentBossStateType = stateType;
            GetState(CurrentBossStateType)?.Enter(bossAgent);
	    }
    }
}
