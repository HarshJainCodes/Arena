using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStateMachine
{
	// Array of all possible AI states
	public IAiState[] states;

	// Reference to the agent using the state machine
	public AiAgent agent;

	// Current state type of the agent
	public AiStateType currentStateType;

	// Constructor for the AI state machine
	public AiStateMachine(AiAgent agent)
	{
		this.agent = agent;

		// Calculate the number of states based on the enumeration
		int numStates = System.Enum.GetNames(typeof(AiStateType)).Length;

		// Create an array to store the states
		states = new IAiState[numStates];
	}

	// Method to register a new state with the state machine
	public void RegisterState(IAiState state)
	{
		// Get the index of the state in the array based on its state type
		int index = (int)state.GetStateType();

		// Add the state to the array at the calculated index
		states[index] = state;
	}

	// Method to get the state based on its state type
	public IAiState GetState(AiStateType stateType)
	{
		// Get the index of the state in the array based on its state type
		int index = (int)stateType;

		// Return the state from the array at the calculated index
		return states[index];
	}

	// Method called each frame to update the current state
	public void Update()
	{
		// Call the Update method of the current state passing the agent as a parameter
		GetState(currentStateType)?.Update(agent);
	}

	// Method to change the current state
	public void ChangeState(AiStateType stateType)
	{
		// Check if the new state is different from the current state
		if (currentStateType != stateType)
		{
			// Exit the current state by calling its Exit method
			GetState(currentStateType)?.Exit(agent);

			// Set the current state type to the new state type
			currentStateType = stateType;

			// Enter the new state by calling its Enter method
			GetState(currentStateType)?.Enter(agent);
		}
	}
}
