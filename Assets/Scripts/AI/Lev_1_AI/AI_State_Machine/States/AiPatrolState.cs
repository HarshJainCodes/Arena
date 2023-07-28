using System;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI.Lev_1_AI.AI_State_Machine.States
{
	// AI patrol state class implementing IAiState interface
	public class AiPatrolState : IAiState
	{
		// Reference to the AI agent
		public AiAgent Agent;

		// Reference to the AIPath component
		public AIPath AiPath;

		// Shortcut property to access AIPath component
		public AIPath ai => Agent.GetComponent<AIPath>();

		// Range within which to pick random patrol points
		public float range = 10f;

		// The distance from the patrol point considered as the end of path
		public float endDistance;

		// Time delay for picking new patrol point
		public float t = 8f;

		// The position where the AI agent was spawned
		private Vector3 _Spawn;

		// Method to get the type of the AI state
		public AiStateType GetStateType()
		{
			return AiStateType.Patrol;
		}

		// Method called when entering the AI state
		public void Enter(AiAgent agent)
		{
			Agent = agent;
			_Spawn = Agent.transform.position;
			endDistance = Agent.GetComponent<AIPath>().endReachedDistance;
			AiPath = Agent.GetComponent<AIPath>();
			Agent.GetComponent<AIDestinationSetter>().enabled = true;
			Agent.GetComponent<AIPath>().enabled = true;
			Agent.GetComponent<Seeker>().enabled = true;
			AiPath.maxSpeed = agent.PatrolSpeed;
			MoveTo(PickRandomPoint());
		}

		// Method called each frame to update the AI state
		public void Update(AiAgent agent)
		{
			Agent.GetComponent<AIDestinationSetter>().enabled = true;
			Agent.GetComponent<AIPath>().enabled = true;
			Agent.GetComponent<Seeker>().enabled = true;

			// If the player is in sight or within attack range, switch to chase state
			if (Agent.sensor.IsInSight(Agent.PlayerTransform.gameObject) || Agent.InRange)
			{
				AiPath.maxSpeed = Agent.Speed;
				Agent.GetComponent<AIPath>().endReachedDistance = endDistance;
				Agent.GetComponent<AIDestinationSetter>().target = Agent.PlayerTransform;
				Agent.StateMachine.ChangeState(AiStateType.Chase);
			}

			// If the AI agent has reached the end of the path or has no path, pick a new patrol point
			if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))
			{
				MoveTo(PickRandomPoint());
				ai.SearchPath();
			}
		}

		// Method called when exiting the AI state
		public void Exit(AiAgent agent)
		{
			AiPath.maxSpeed = Agent.Speed;
			Agent.GetComponent<AIPath>().endReachedDistance = endDistance;
			Agent.GetComponent<AIDestinationSetter>().target = Agent.PlayerTransform;
			Agent.GetComponent<AIDestinationSetter>().enabled = false;
			Agent.GetComponent<AIPath>().enabled = false;
			Agent.GetComponent<Seeker>().enabled = false;
		}

		// Method to pick a random patrol point within the specified range
		Vector3 PickRandomPoint()
		{
			Vector3 point = Random.insideUnitSphere * range;
			point.y = 0;
			point += Agent.transform.position;
			while (DistanceFromSpawn(point) >= 25f)
			{
				point = Random.insideUnitSphere * range;
				point.y = 0;
				point += Agent.transform.position;
			}
			return point;
		}

		// Method to move the AI agent to the given position
		public void MoveTo(Vector3 position)
		{
			Agent.PatrolPoint.position = position;

			if (!Agent.sensor.IsInSight(Agent.PlayerTransform.gameObject) && !Agent.InRange)
			{
				Agent.GetComponent<AIPath>().endReachedDistance = 1f;
				Agent.GetComponent<AIDestinationSetter>().target = Agent.PatrolPoint;
			}
			else
			{
				AiPath.maxSpeed = Agent.Speed;
				Agent.GetComponent<AIPath>().endReachedDistance = endDistance;
				Agent.GetComponent<AIDestinationSetter>().target = Agent.PlayerTransform;
				Agent.StateMachine.ChangeState(AiStateType.Chase);
			}
		}

		// Method to calculate the distance between a given point and the spawn point of the AI agent
		private float DistanceFromSpawn(Vector3 point)
		{
			return (float)(Math.Sqrt(Math.Pow(point.x - _Spawn.x, 2) + Math.Pow(point.z - _Spawn.z, 2)));
		}
	}
}
