using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBossStateMachine
{
	public IAiBossState[] states; // Array to store different boss states
	public AiBossAgent bossAgent; // Reference to the boss agent script (controller)
	public AiBossStateType CurrentBossStateType; // The current active boss state type

	// Constructor for the AI Boss State Machine
	public AiBossStateMachine(AiBossAgent bossAgent)
	{
		this.bossAgent = bossAgent; // Assign the boss agent reference to the script

		// Calculate the number of possible states using the AiBossStateType enumeration
		int numStates = System.Enum.GetNames(typeof(AiBossStateType)).Length;

		// Create an array to store the states, initializing all elements to null
		states = new IAiBossState[numStates];
	}

	// Function to register a specific state to the state machine
	public void RegisterState(IAiBossState state)
	{
		int index = (int)state.GetStateType(); // Get the index corresponding to the state's type
		states[index] = state; // Store the state in the array at the calculated index
	}

	// Function to get the state associated with a given state type
	public IAiBossState GetState(AiBossStateType stateType)
	{
		int index = (int)stateType; // Get the index corresponding to the provided state type
		return states[index]; // Return the state associated with the provided state type
	}

	// Function to update the current active state
	public void Update()
	{
		// Call the Update function of the current active state, if it exists
		GetState(CurrentBossStateType)?.Update(bossAgent);
	}

	// Function to change the state of the boss agent
	public void ChangeState(AiBossStateType stateType)
	{
		// Check if the requested state is different from the current state
		if (CurrentBossStateType != stateType)
		{
			// Call the Exit function of the current state, if it exists
			GetState(CurrentBossStateType)?.Exit(bossAgent);

			// Set the current state type to the new state type
			CurrentBossStateType = stateType;

			// Call the Enter function of the new state, if it exists
			GetState(CurrentBossStateType)?.Enter(bossAgent);
		}
	}
}
