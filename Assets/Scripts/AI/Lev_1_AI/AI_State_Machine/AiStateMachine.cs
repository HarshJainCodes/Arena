using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStateMachine 
{
    public IAiState[] states;
    public AiAgent agent;
    public AiStateType currentStateType;

    public AiStateMachine(AiAgent agent)
    {
	    this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(AiStateType)).Length;
        states = new IAiState[numStates];
    }

    public void RegisterState(IAiState state)
    {
		int index = (int)state.GetStateType();
        states[index] = state;
	}

    public IAiState GetState(AiStateType stateType)
    {
        int index = (int)stateType;
        return states[index];
    }

    public void Update()
    {
		GetState(currentStateType)?.Update(agent);
	}

    public void ChangeState(AiStateType stateType)
    {
	    if (currentStateType != stateType)
	    {
			GetState(currentStateType)?.Exit(agent);
			currentStateType = stateType;
			GetState(currentStateType)?.Enter(agent);
		}
	}
}
